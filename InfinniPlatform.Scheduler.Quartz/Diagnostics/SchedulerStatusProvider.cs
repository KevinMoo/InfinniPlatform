﻿using System;
using System.Linq;
using System.Threading.Tasks;

using InfinniPlatform.Diagnostics;
using InfinniPlatform.Dynamic;
using InfinniPlatform.Http;

using Microsoft.Extensions.Logging;

namespace InfinniPlatform.Scheduler.Diagnostics
{
    /// <summary>
    /// Provides the scheduler status.
    /// </summary>
    internal class SchedulerStatusProvider : ISubsystemStatusProvider
    {
        public SchedulerStatusProvider(IJobScheduler jobScheduler,
                                       IHostAddressParser hostAddressParser,
                                       ILogger<SchedulerStatusProvider> logger)
        {
            _jobScheduler = jobScheduler;
            _hostAddressParser = hostAddressParser;
            _logger = logger;
        }


        private readonly IJobScheduler _jobScheduler;
        private readonly IHostAddressParser _hostAddressParser;
        private readonly ILogger _logger;


        public string Name => "scheduler";


        public void Load(IHttpServiceBuilder builder)
        {
            builder.ServicePath = Name;

            builder.OnBefore = async r =>
                               {
                                   // Запрос статуса разрешен только с локального узла
                                   if (!await _hostAddressParser.IsLocalAddress(r.UserHostAddress))
                                   {
                                       return HttpResponse.Forbidden;
                                   }

                                   return null;
                               };

            // Состояние планировщика заданий
            builder.Get["/"] = r => HandleRequest(r, GetStatus);
            builder.Post["/"] = r => HandleRequest(r, GetStatus);
        }


        /// <summary>
        /// Gets the scheduler status.
        /// </summary>
        public async Task<object> GetStatus(IHttpRequest request)
        {
            var isStarted = await _jobScheduler.IsStarted();
            var totalCount = await _jobScheduler.GetStatus(i => i.Count());
            var plannedCount = await _jobScheduler.GetStatus(i => i.Count(j => j.State == JobState.Planned));
            var pausedCount = await _jobScheduler.GetStatus(i => i.Count(j => j.State == JobState.Paused));

            var status = new DynamicDocument
                         {
                             {
                                 "isStarted", isStarted
                             },
                             {
                                 "all", new DynamicDocument
                                        {
                                            { "count", totalCount },
                                            { "ref", $"{request.BasePath}/{Name}/jobs?skip=0&take=10" }
                                        }
                             },
                             {
                                 "planned", new DynamicDocument
                                            {
                                                { "count", plannedCount },
                                                { "ref", $"{request.BasePath}/{Name}/jobs?state=planned&skip=0&take=10" }
                                            }
                             },
                             {
                                 "paused", new DynamicDocument
                                           {
                                               { "count", pausedCount },
                                               { "ref", $"{request.BasePath}/{Name}/jobs?state=paused&skip=0&take=10" }
                                           }
                             }
                         };

            return new ServiceResult<DynamicDocument> { Success = true, Result = status };
        }


        private async Task<object> HandleRequest(IHttpRequest request, Func<IHttpRequest, Task<object>> requestHandler)
        {
            try
            {
                return await requestHandler(request);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception);

                return new ServiceResult<object>
                {
                    Success = false,
                    Error = exception.GetFullMessage()
                };
            }
        }
    }
}