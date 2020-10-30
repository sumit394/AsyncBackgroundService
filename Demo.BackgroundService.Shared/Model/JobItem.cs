using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Demo.BackgroundService.Shared.Model
{
    /// <summary>
    /// Number based job item
    /// </summary>
    public class JobItem
    {

        public string Id { get; set; }
        public DateTime? EnqueuedUtcTimeStamp { get; set; }//Define + Agree on defination of Enqueue.
        public DateTime? CompletedUtcTimeStamp { get; set; }//Define + Agree on defination of Completed.
        public TimeSpan? ExecuteDuration => CompletedUtcTimeStamp - EnqueuedUtcTimeStamp;
        public JobStatus Status { get; set; }
        public List<int> Items { get; set; }

        public JobItem()
        {
            Initialize();
        }

        public JobItem(JobItem jobItem)
        {
            Id = jobItem.Id;
            Status = jobItem.Status;
            EnqueuedUtcTimeStamp = jobItem?.EnqueuedUtcTimeStamp;
            CompletedUtcTimeStamp = jobItem?.EnqueuedUtcTimeStamp;
        }


        public void UpdateJobStatus(JobStatus jobStatus)
        {
            Status = jobStatus;

            if (jobStatus == JobStatus.Completed
                || jobStatus == JobStatus.Failed
                || jobStatus == JobStatus.Rejected)
            {
                CompletedUtcTimeStamp = DateTime.UtcNow;
            }

        }


        /// <summary>
        /// Initialize with collections of array
        /// </summary>
        /// <param name="items">Array of string</param>
        public JobItem(int[] items)
        {
            Initialize();

            Items = new List<int>(items);

            if (Items?.Count() < 1)
            {
                UpdateJobStatus(JobStatus.Rejected);
            }
            else if (Items?.Count() == 1)
            {
                UpdateJobStatus(JobStatus.Completed);
            }
        }

        private void Initialize()
        {

            Id = Guid.NewGuid().ToString();
            Status = JobStatus.Pending;
            EnqueuedUtcTimeStamp = DateTime.UtcNow;
        }

    }
}