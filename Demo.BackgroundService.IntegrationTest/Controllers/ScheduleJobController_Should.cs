
using Demo.BackgroundService.Shared.Model;
using Maersk.StarterKit.IntegrationTests;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Xunit.Extensions.Ordering;


namespace Demo.BackgroundService.IntegrationTests.Controllers
{

    public class ScheduleJobController_Should : IClassFixture<SetupServer>
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly SetupServer _server;

        private const string _allJobGetRelativeUri = "api/ScheduledJob";
        private const string _specificJobByIdGetRelativeUri = "api/ScheduledJob/{0}";
        private const string _addScheduleJobPostRelativeUri = "api/ScheduledJob/";
        private const string _nonExistingTesGetRelativetUri = "api/nonexisting";

        public ScheduleJobController_Should(ITestOutputHelper testOutputHelper,
            SetupServer setupServer)
        {
            _testOutputHelper = testOutputHelper;
            _server = setupServer;
        }

        [Order(1)]
        [Fact]
        public async Task ScheduleJobController_Return_200_If_OK()
        {
            var result = await _server.Client.GetAsync(_allJobGetRelativeUri);
            var body = await result.Content.ReadAsStringAsync();
            AssertExtension.AssertStatusCode(HttpStatusCode.OK, result.StatusCode, body, _testOutputHelper);
        }

        [Order(2)]
        [Fact]
        public async Task ScheduleJobController_Return_404_If_NotFound()
        {
            var result = await _server.Client.GetAsync(_nonExistingTesGetRelativetUri);
            var body = await result.Content.ReadAsStringAsync();
            AssertExtension.AssertStatusCode(HttpStatusCode.NotFound, result.StatusCode, body, _testOutputHelper);
        }

        [Order(4)]
        [Fact]
        public async Task ScheduleJobController_AddJob_Return_Successful()
        {
            int[] data = new int[] { 1, 2, 3, 4, 5, 5, 4, 3, 2, 1 };

            var requestContent = JsonConvert.SerializeObject(data);
            var httpContent = new StringContent(requestContent, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Post, _addScheduleJobPostRelativeUri);
            request.Content = httpContent;

            var result = await _server.Client.SendAsync(request);
            var actualResponse = await result.Content.ReadAsAsync<string>();

            Guid responseGuidId = Guid.Parse(actualResponse);
            Assert.True(Guid.TryParse(actualResponse, out responseGuidId));
        }

        [Order(5)]
        [Fact]
        public async Task ScheduleJobController_AddMultipleJob_Return_Successful()
        {
            int[] data1 = new int[] { 1, 2, 3, 4, 5, 5, 4, 3, 2, 1 };
            int[] data2 = new int[] { 5, 4, 3, 2, 1, 1, 2, 3, 4, 5 };
            int[] data3 = new int[] { 101, 201, -101, -201, 1000, 2000, -1000, -2000, 0, -0 };

            var requestContent1 = JsonConvert.SerializeObject(data1);
            var httpContent1 = new StringContent(requestContent1, Encoding.UTF8, "application/json");
            var request1 = new HttpRequestMessage(HttpMethod.Post, _addScheduleJobPostRelativeUri);
            request1.Content = httpContent1;
            var result1 = await _server.Client.SendAsync(request1);
            var actualResponse1 = await result1.Content.ReadAsAsync<string>();
            Guid responseGuidId1 = Guid.Parse(actualResponse1);
            Assert.True(Guid.TryParse(actualResponse1, out responseGuidId1));

            var requestContent2 = JsonConvert.SerializeObject(data2);
            var httpContent2 = new StringContent(requestContent2, Encoding.UTF8, "application/json");
            var request2 = new HttpRequestMessage(HttpMethod.Post, _addScheduleJobPostRelativeUri);
            request2.Content = httpContent2;
            var result2 = await _server.Client.SendAsync(request2);
            var actualResponse2 = await result2.Content.ReadAsAsync<string>();
            Guid responseGuidId2 = Guid.Parse(actualResponse2);
            Assert.True(Guid.TryParse(actualResponse2, out responseGuidId2));


            var requestContent3 = JsonConvert.SerializeObject(data3);
            var httpContent3 = new StringContent(requestContent3, Encoding.UTF8, "application/json");
            var request3 = new HttpRequestMessage(HttpMethod.Post, _addScheduleJobPostRelativeUri);
            request3.Content = httpContent3;
            var result3 = await _server.Client.SendAsync(request3);
            var actualResponse3 = await result3.Content.ReadAsAsync<string>();
            Guid responseGuidId3 = Guid.Parse(actualResponse3);
            Assert.True(Guid.TryParse(actualResponse3, out responseGuidId3));
        }

        [Order(6)]
        [Fact]
        public async Task ScheduleJobController_AddSortJob_Return_Successful()
        {
            int[] data1 = new int[] { 1, 2, 3, 4, 5, 5, 4, 3, 2, 1 };

            var requestContent1 = JsonConvert.SerializeObject(data1);
            var httpContent1 = new StringContent(requestContent1, Encoding.UTF8, "application/json");
            var request1 = new HttpRequestMessage(HttpMethod.Post, _addScheduleJobPostRelativeUri);
            request1.Content = httpContent1;
            var result1 = await _server.Client.SendAsync(request1);
            var actualResponse1 = await result1.Content.ReadAsAsync<string>();
            Guid responseGuidId1 = Guid.Parse(actualResponse1);
            Assert.True(Guid.TryParse(actualResponse1, out responseGuidId1));

            var allJobResponse = await _server.Client.GetAsync(_allJobGetRelativeUri);
            var allJobData = await allJobResponse.Content.ReadAsAsync<List<JobItem>>();

            var isAllCompletedJobSorted = true;
            foreach (var jobMetaData in allJobData.Where(a => a.Status == JobStatus.Completed))
            {
                var jobItemResponse = await _server.Client.GetAsync(string.Format(_specificJobByIdGetRelativeUri, jobMetaData.Id));
                var jobItem = await jobItemResponse.Content.ReadAsAsync<JobItem>();

                int totalItemsCount = jobItem.Items.Count;
                for (int i = 0; i < totalItemsCount - 1; i++)
                {
                    if (jobItem.Items[i] <= jobItem.Items[i + 1] == false)
                    {
                        isAllCompletedJobSorted = false;
                        break;
                    }
                }
                if (!isAllCompletedJobSorted)
                    break;
            }

            Assert.True(isAllCompletedJobSorted);

        }

        [Order(7)]
        [Fact]
        public async Task ScheduleJobController_AddSortMultipleJob_Return_Successful()
        {
            int[] data1 = new int[] { 1, 2, 3, 4, 5, 5, 4, 3, 2, 1 };
            int[] data2 = new int[] { 5, 4, 3, 2, 1, 1, 2, 3, 4, 5 };
            int[] data3 = new int[] { 101, 201, -101, -201, 1000, 2000, -1000, -2000, 0, -0 };

            var requestContent1 = JsonConvert.SerializeObject(data1);
            var httpContent1 = new StringContent(requestContent1, Encoding.UTF8, "application/json");
            var request1 = new HttpRequestMessage(HttpMethod.Post, _addScheduleJobPostRelativeUri);
            request1.Content = httpContent1;
            var result1 = await _server.Client.SendAsync(request1);
            var actualResponse1 = await result1.Content.ReadAsAsync<string>();
            Guid responseGuidId1 = Guid.Parse(actualResponse1);
            Assert.True(Guid.TryParse(actualResponse1, out responseGuidId1));

            var requestContent2 = JsonConvert.SerializeObject(data2);
            var httpContent2 = new StringContent(requestContent2, Encoding.UTF8, "application/json");
            var request2 = new HttpRequestMessage(HttpMethod.Post, _addScheduleJobPostRelativeUri);
            request2.Content = httpContent2;
            var result2 = await _server.Client.SendAsync(request2);
            var actualResponse2 = await result2.Content.ReadAsAsync<string>();
            Guid responseGuidId2 = Guid.Parse(actualResponse2);
            Assert.True(Guid.TryParse(actualResponse2, out responseGuidId2));


            var requestContent3 = JsonConvert.SerializeObject(data3);
            var httpContent3 = new StringContent(requestContent3, Encoding.UTF8, "application/json");
            var request3 = new HttpRequestMessage(HttpMethod.Post, _addScheduleJobPostRelativeUri);
            request3.Content = httpContent3;
            var result3 = await _server.Client.SendAsync(request3);
            var actualResponse3 = await result3.Content.ReadAsAsync<string>();
            Guid responseGuidId3 = Guid.Parse(actualResponse3);
            Assert.True(Guid.TryParse(actualResponse3, out responseGuidId3));


            var allJobResponse = await _server.Client.GetAsync(_allJobGetRelativeUri);
            var allJobData = await allJobResponse.Content.ReadAsAsync<List<JobItem>>();

            var isAllCompletedJobSorted = true;
            foreach (var jobMetaData in allJobData.Where(a => a.Status == JobStatus.Completed))
            {
                var jobItemResponse = await _server.Client.GetAsync(string.Format(_specificJobByIdGetRelativeUri, jobMetaData.Id));
                var jobItem = await jobItemResponse.Content.ReadAsAsync<JobItem>();

                int totalItemsCount = jobItem.Items.Count;
                for (int i = 0; i < totalItemsCount - 1; i++)
                {
                    if (jobItem.Items[i] <= jobItem.Items[i + 1] == false)
                    {
                        isAllCompletedJobSorted = false;
                        break;
                    }
                }
                if (!isAllCompletedJobSorted)
                    break;
            }

            Assert.True(isAllCompletedJobSorted);

        }
    }
}