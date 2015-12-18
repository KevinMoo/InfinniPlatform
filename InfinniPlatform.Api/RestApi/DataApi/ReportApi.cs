﻿using InfinniPlatform.Api.Reporting;
using InfinniPlatform.Api.RestApi.CommonApi;

namespace InfinniPlatform.Api.RestApi.DataApi
{
    public sealed class ReportApi
    {
        public ReportApi(RestQueryApi restQueryApi)
        {
            _restQueryApi = restQueryApi;
        }

        private readonly RestQueryApi _restQueryApi;

        public dynamic GetReport(string configuration, string templateName, object[] parameters, ReportFileFormat reportFileFormat)
        {
            return _restQueryApi.QueryPostJsonRaw("SystemConfig", "Reporting", "getReport", null, new
                                                                                                  {
                                                                                                      Configuration = configuration,
                                                                                                      Template = templateName,
                                                                                                      Parameters = parameters,
                                                                                                      FileFormat = (int)reportFileFormat
                                                                                                  });
        }
    }
}