using System.Threading.Tasks;
using Xunit.Abstractions;

namespace TestProject1
{
    public class UnitTest1
    {
        private readonly ITestOutputHelper _output;

        public UnitTest1(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task Test1()
        {
            var semaphore = new SemaphoreSlim(3);

            var tasks = Enumerable.Range(0, 100).Select(async _ =>
            {
                await semaphore.WaitAsync();

                try
                {
                    await AsyncMethod();
                }
                finally
                {
                    semaphore.Release();
                }
            });

            await Task.WhenAll(tasks);
        }

        private async Task AsyncMethod()
        {
            _output.WriteLine("Before uploading");
            await Task.Delay(2000);
            _output.WriteLine("After uploading");
        }
    }
}