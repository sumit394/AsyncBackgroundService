using Autofac.Extras.Moq;
using Demo.BackgroundService;
using Demo.BackgroundService.Shared.Model;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;
namespace Maersk.StarterKit.UnitTest
{
    public class SortingChannelTest
    {

        [Fact]
        public void SortingChannel_GetItem_Succcessful()
        {
            using (var mock = AutoMock.GetLoose())
            {
                int[] data = new int[] { 1, 2, 3, 4, 5, 5, 4, 3, 2, 1 };
                var exptectedJobItem = new JobItem(data);

                mock.Mock<IAppDataStorage>().Setup(x => x.GetItem(exptectedJobItem.Id)).Returns(exptectedJobItem);

                var sortingChannel = mock.Create<SortingChannel>();
                var actualJobItem = sortingChannel.GetJobById(exptectedJobItem.Id);
                mock.Mock<IAppDataStorage>().Verify(x => x.GetItem(exptectedJobItem.Id), Times.Exactly(1));

                Assert.Equal(exptectedJobItem.Id, actualJobItem.Id);
            }
        }

        [Fact]
        public void SortingChannel_GetAllItems_Succesful()
        {
            using (var mock = AutoMock.GetLoose())
            {
                int[] data = new int[] { 1, 2, 3, 4, 5, 5, 4, 3, 2, 1 };
                var testJobItem = new JobItem(data);
                List<JobItem> actualJobItems = new List<JobItem>();
                actualJobItems.Add(new JobItem(new int[] { 1, 2, 3, 4, 5, 5, 4, 3, 2, 1 }));
                actualJobItems.Add(new JobItem(new int[] { 5, 4, 3, 2, 1, 1, 2, 3, 4, 5 }));

                mock.Mock<IAppDataStorage>().Setup(x => x.GetAllItems()).Returns(actualJobItems);
                var sortingChannel = mock.Create<SortingChannel>();
                var expectedJobItems = sortingChannel.GetAllJob();

                mock.Mock<IAppDataStorage>().Verify(x => x.GetAllItems(), Times.Exactly(1));
                Assert.Equal(actualJobItems.Count(), expectedJobItems.Count());

                Assert.Contains(expectedJobItems, a => a.Id == actualJobItems[0].Id);
                Assert.Contains(expectedJobItems, a => a.Id == actualJobItems[1].Id);
            }
        }
    }
}