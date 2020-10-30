using Demo.BackgroundService.Shared.Model;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.BackgroundService
{
    public class SortingService : Microsoft.Extensions.Hosting.BackgroundService, IDisposable
    {
        private readonly ILogger _logger;

        //To implement timer based event
        //private readonly int _refreshIntervalInSeconds = 3;

        private readonly ISortingChannel _sortingChannel;
        private readonly IAppDataStorage _appDataStorage;
        
        public SortingService(ILogger logger, IHostApplicationLifetime hostApplicationLifetime, ISortingChannel sortingChannel, IAppDataStorage appDataStorage)
        {
            _logger = logger;
            _sortingChannel = sortingChannel;
            _appDataStorage = appDataStorage;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.Information($"{nameof(LogEventMap.HostingService_Started) } , Host Service has started.");

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    //To Enable timer based event but we are using IAsyncEnumerable using Channel Options
                    //await Task.Delay(TimeSpan.FromSeconds(_refreshIntervalInSeconds), stoppingToken);

                    //Either read IAsyncEnumrable and process or collections and invoke parallel sorting task.
                    //Keeping iAsyncEnumerable for the sake of simplicity at the moment.                
                    var jobId = string.Empty;
                    await foreach (var jobItem in _sortingChannel.ReadAllAsync(stoppingToken))
                    {
                        jobId = jobItem.Id;
                        _logger.Information($"{nameof(LogEventMap.HostingService_ItemReceived) } , Id = {jobId}, Message = Host Service has received this item.");

                        _appDataStorage.Add(jobItem);

                        _logger.Information($"{nameof(LogEventMap.HostingService_ItemSortingInProgress) } , Id = {jobId}, Message = Host Service has received this item.");

                        await Task.Run(() => SortNumbers(jobItem), stoppingToken);

                        _logger.Information($"{nameof(LogEventMap.HostingService_ItemSortingCompleted) } , Id = {jobId}, Message = Host Service has completed this item.");

                    }
                }
            }
            catch (OperationCanceledException ex)
            {
                _logger.Warning(ex, "Operation cancelled occured");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An unhandled exception was thrown.");
                _sortingChannel.CompleteWriter(ex);
            }
            finally
            {
                _sortingChannel.TryCompleteWriter();
            }
        }

        private void SortNumbers(JobItem jobItem)
        {
            jobItem.Items.Sort();
            jobItem.UpdateJobStatus(JobStatus.Completed);
        }

    }
}
