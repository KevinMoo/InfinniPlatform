﻿using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace InfinniPlatform.Scheduler.Contract
{
    /// <summary>
    /// Планировщик заданий.
    /// </summary>
    public interface IJobScheduler
    {
        /// <summary>
        /// Возвращает список уникальных идентификаторов заданий.
        /// </summary>
        /// <param name="group">Регулярное выражение для поиска заданий по группе.</param>
        Task<IEnumerable<string>> GetJobIds(Regex group = null);

        /// <summary>
        /// Возвращает информацию о задании.
        /// </summary>
        /// <param name="jobId">Уникальный идентификатор задания.</param>
        Task<IJobInfo> GetJobInfo(string jobId);


        /// <summary>
        /// Добавляет или обновляет задание.
        /// </summary>
        /// <param name="jobInfo">Информация о задании.</param>
        Task AddOrUpdateJob(IJobInfo jobInfo);

        /// <summary>
        /// Добавляет или обновляет список заданий.
        /// </summary>
        /// <param name="jobInfos">Список с информацией о заданиях.</param>
        Task AddOrUpdateJobs(IEnumerable<IJobInfo> jobInfos);


        /// <summary>
        /// Удаляет указанное задание.
        /// </summary>
        /// <param name="jobId">Уникальный идентификатор задания.</param>
        Task DeleteJob(string jobId);

        /// <summary>
        /// Удаляет указанные задания.
        /// </summary>
        /// <param name="jobIds">Список с уникальными идентификаторами заданий.</param>
        Task DeleteJobs(IEnumerable<string> jobIds);

        /// <summary>
        /// Удаляет все задания.
        /// </summary>
        Task DeleteAllJobs();


        /// <summary>
        /// Приостанавливает планирование указанного задания.
        /// </summary>
        /// <param name="jobId">Уникальный идентификатор задания.</param>
        Task PauseJob(string jobId);

        /// <summary>
        /// Приостанавливает планирование указанных заданий.
        /// </summary>
        /// <param name="jobIds">Список с уникальными идентификаторами заданий.</param>
        Task PauseJobs(IEnumerable<string> jobIds);

        /// <summary>
        /// Приостанавливает планирование всех заданий.
        /// </summary>
        Task PauseAllJobs();


        /// <summary>
        /// Возобновляет планирование указанного задания.
        /// </summary>
        /// <param name="jobId">Уникальный идентификатор задания.</param>
        Task ResumeJob(string jobId);

        /// <summary>
        /// Возобновляет планирование указанных заданий.
        /// </summary>
        /// <param name="jobIds">Список с уникальными идентификаторами заданий.</param>
        Task ResumeJobs(IEnumerable<string> jobIds);

        /// <summary>
        /// Возобновляет планирование всех заданий.
        /// </summary>
        Task ResumeAllJobs();


        /// <summary>
        /// Вызывает досрочное выполнение указанного задания.
        /// </summary>
        /// <param name="jobId">Уникальный идентификатор задания.</param>
        /// <param name="data">Данные для выполнения задания.</param>
        Task TriggerJob(string jobId, object data = null);

        /// <summary>
        /// Вызывает досрочное выполнение указанных заданий.
        /// </summary>
        /// <param name="jobIds">Список с уникальными идентификаторами заданий.</param>
        /// <param name="data">Данные для выполнения заданий.</param>
        Task TriggerJobs(IEnumerable<string> jobIds, object data = null);

        /// <summary>
        /// Вызывает досрочное выполнение всех заданий.
        /// </summary>
        /// <param name="data">Данные для выполнения заданий.</param>
        Task TriggerAllJob(object data = null);
    }
}