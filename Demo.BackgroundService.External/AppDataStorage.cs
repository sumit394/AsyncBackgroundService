using Demo.BackgroundService.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Demo.BackgroundService.External
{
    public class AppDataStorage : IAppDataStorage, IDisposable
    {
        private List<JobItem> Items { get; set; }

        public AppDataStorage()
        {
            Items = new List<JobItem>();
        }

        public void Add(JobItem jobItem)
        {
            Items.Add(jobItem);
        }

        public JobItem GetItem(string id)
        {
            return Items.Where(a => a.Id == id).FirstOrDefault();
        }

        public List<JobItem> GetAllItems()
        {
            return Items;
        }

        public void Dispose()
        {
            Items.Clear();
            Items = null;
        }
    }
}
