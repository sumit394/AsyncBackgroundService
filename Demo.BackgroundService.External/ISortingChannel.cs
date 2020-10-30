using Demo.BackgroundService.Shared.Model;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.BackgroundService
{
    public interface ISortingChannel
    {
        public Task<bool> AddItemAsync(JobItem jobItem, CancellationToken ct = default);

        public IAsyncEnumerable<JobItem> ReadAllAsync(CancellationToken ct = default);

        public IEnumerable<JobItem> GetAllJob();
        public JobItem GetJobById(string id);


        void CompleteWriter(Exception ex = null);
        bool TryCompleteWriter(Exception ex = null);
    }
}
