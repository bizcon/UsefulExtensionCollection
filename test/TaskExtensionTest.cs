using bizconAG.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace bizconAg.Extensions.Test
{
    [TestCategory("Unit")]
    [TestClass]
    public class TaskExtensionsTest
    {
        private async Task<int> WaitAndGetIntAsync(int msWait, CancellationToken token)
        {
            int wait = 100;
            int count = msWait / wait;
            int counter = 0;
            while (counter < count)
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(wait);
                counter++;
            }
            return msWait;
        }

        private async Task<int> WaitAndGetIntDelegateAsync(CancellationToken token)
        {
            return await this.WaitAndGetIntAsync(2000, token);
        }

        private const string expectedEqual = "Expected equal";
        private const string expectedException = "Expected exception";

        [TestMethod]
        public async Task TimedOutAsyncTest()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            int wait = 2000;
            int result = await WaitAndGetIntAsync(wait, source.Token).TimedOutAsync(TimeSpan.FromMilliseconds(3000));
            Assert.AreEqual(wait, result, expectedEqual);
            result = await WaitAndGetIntAsync(wait, source.Token).TimedOutAsync(TimeSpan.FromMilliseconds(1000));
            Assert.AreEqual(default(int), result, expectedEqual);
            result = await WaitAndGetIntAsync(wait, source.Token).TimedOutAsync(TimeSpan.FromMilliseconds(1000), 33);
            Assert.AreEqual(33, result, expectedEqual);
            result = await WaitAndGetIntAsync(wait, source.Token).TimedOutAsync(TimeSpan.FromMilliseconds(3000), 77);
            Assert.AreEqual(wait, result, expectedEqual);
        }

        [TestMethod]
        public async Task CancelAfterAsyncTaskTest()
        {
            int wait = 2000;
            CancellationTokenSource source = new CancellationTokenSource();

            try
            {
                _ = await WaitAndGetIntAsync(wait, source.Token).CancelAfterAsync(TimeSpan.FromMilliseconds(1000), source);
                Assert.Fail(expectedException);
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception e)
            {
                Assert.Fail($"{expectedException}: {e.GetType()} instead of System.OperationCanceledException");
            }

            source = new CancellationTokenSource();
            int result = await WaitAndGetIntAsync(wait, source.Token).CancelAfterAsync(TimeSpan.FromMilliseconds(3000), source);
            Assert.AreEqual(wait, result, expectedEqual);
        }

        [TestMethod]
        public async Task CancelAfterAsyncDelegateTest()
        {
            int wait = 2000;
            CancellationTokenSource source = new CancellationTokenSource();

            try
            {
                await ((Func<CancellationToken, Task<int>>)this.WaitAndGetIntDelegateAsync).CancelAfterAsync(TimeSpan.FromMilliseconds(1000));
                Assert.Fail(expectedException);
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception e)
            {
                Assert.Fail($"{expectedException}: {e.GetType()} instead of System.OperationCanceledException");
            }

            int result = await ((Func<CancellationToken, Task<int>>)this.WaitAndGetIntDelegateAsync).CancelAfterAsync(TimeSpan.FromMilliseconds(3000));
            Assert.AreEqual(wait, result, expectedEqual);
        }
    }
}
