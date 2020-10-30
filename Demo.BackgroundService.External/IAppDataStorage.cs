using Demo.BackgroundService.Shared.Model;
using System.Collections.Generic;

namespace Demo.BackgroundService
{
    public interface IAppDataStorage
    {
        public void Add(JobItem jobItem);
        public JobItem GetItem(string id);
        public List<JobItem> GetAllItems();
    }
}
