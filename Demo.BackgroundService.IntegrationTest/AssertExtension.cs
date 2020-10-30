using System.Net;
using Xunit;
using Xunit.Abstractions;

namespace Maersk.StarterKit.IntegrationTests
{
    public class AssertExtension
    {
        public static void AssertStatusCode(HttpStatusCode expected, HttpStatusCode actual, string body, ITestOutputHelper testOutputHelper)
        {
            try
            {
                Assert.Equal(expected, actual);
            }
            catch
            {
                testOutputHelper.WriteLine(body);
                throw;
            }
        }

        public static void AssertResponse(object expected, object actual, ITestOutputHelper testOutputHelper)
        {
            try
            {
                Assert.Equal(expected, actual);
            }
            catch
            {
                testOutputHelper.WriteLine(actual.ToString());
                throw;
            }
        }

    }
}
