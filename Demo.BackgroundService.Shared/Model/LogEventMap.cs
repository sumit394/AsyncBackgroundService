namespace Demo.BackgroundService.Shared.Model
{
    public enum LogEventMap
    {
        HostingService_Started = 1,
        HostingService_ItemReceived,
        HostingService_ItemSortingInProgress,
        HostingService_ItemSortingCompleted,

        WebApi_ScheduledJob_AllJobStatus = 1000,
        WebApi_ScheduledJob_SpecificJobStatus,
        WebApi_ScheduledJob_SpecificJob_NotFound,

        WebApi_ScheduledJob_ItemReceived = 2000,
        WebApi_ScheduledJob_ItemReceived_IsEmpty,
        WebApi_ScheduledJob_ItemReceived_Returned,
        WebApi_ScheduledJob_ItemReceived_Scheduled,

        SortService_ListReceived = 10000,
        SortService_ListReceivedSent,
        SortService_ListEnqueued,
        SortService_InProgress,
        SortService_Completed
    }
}