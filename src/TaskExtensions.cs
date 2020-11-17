using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Bizcon.Extensions
{
    public static class TaskExtensions
    {
        public static async Task<bool> TimedOutAsync(this Task task, TimeSpan timeout)
        {
            if (timeout.TotalMilliseconds < 0 || (timeout.TotalMilliseconds > 0 && timeout.TotalMilliseconds < 100)) { throw new ArgumentOutOfRangeException(); }

            if (timeout.TotalMilliseconds == 0)
            {
                return !task.IsCompleted;
            }
            var cts = new CancellationTokenSource();
            if (await Task.WhenAny(task, Task.Delay(timeout, cts.Token)) == task)
            {
                cts.Cancel();
                await task;
                return false;
            }
            else
            {
                return true;
            }
        }

        public static async Task<T> TimedOutAsync<T>(this Task<T> task, TimeSpan timeout, T defaultReturn = default(T))
        {
            if (timeout.TotalMilliseconds < 0 || (timeout.TotalMilliseconds > 0 && timeout.TotalMilliseconds < 100)) { throw new ArgumentOutOfRangeException(); }

            if (timeout.TotalMilliseconds == 0)
            {
                if (task.IsCompleted)
                {
                    return await task;
                }
                else
                {
                    return defaultReturn;
                }
            }
            var cts = new CancellationTokenSource();
            if (await Task.WhenAny(task, Task.Delay(timeout, cts.Token)) == task)
            {
                cts.Cancel();
                return await task;
            }
            else
            {
                return defaultReturn;
            }
        }

        public static async Task<T> CancelAfterAsync<T>(this Func<CancellationToken, Task<T>> actionAsync, TimeSpan timeout)
        {
            if (timeout.TotalMilliseconds < 0 || (timeout.TotalMilliseconds > 0 && timeout.TotalMilliseconds < 100)) { throw new ArgumentOutOfRangeException(); }

            var taskCts = new CancellationTokenSource();
            var timerCts = new CancellationTokenSource();
            Task<T> task = actionAsync(taskCts.Token);
            if (await Task.WhenAny(task, Task.Delay(timeout, timerCts.Token)) == task)
            {
                timerCts.Cancel();
            }
            else
            {
                taskCts.Cancel();
            }
            return await task;
        }

        public static async Task<T> CancelAfterAsync<T>(this Task<T> task, TimeSpan timeout, CancellationTokenSource cancellationTokenSource)
        {
            if (timeout.TotalMilliseconds < 0 || (timeout.TotalMilliseconds > 0 && timeout.TotalMilliseconds < 100)) { throw new ArgumentOutOfRangeException(); }

            var timerCts = new CancellationTokenSource();
            if (await Task.WhenAny(task, Task.Delay(timeout, timerCts.Token)) == task)
            {
                timerCts.Cancel();
            }
            else
            {
                cancellationTokenSource.Cancel();
            }
            return await task;
        }

        public static async Task ForEachAsync<T>(this IEnumerable<T> source, Func<T, Task> function)
        {
            List<Exception> exceptions = null;
            foreach (var item in source)
            {
                try { await function(item); }
                catch (Exception exc)
                {
                    if (exceptions == null) exceptions = new List<Exception>();
                    exceptions.Add(exc);
                }
            }
            if (exceptions != null)
                throw new AggregateException(exceptions);
        }

        public static async Task WaitWhile(Func<bool> condition, int frequency = 25, int timeout = -1)
        {
            var waitTask = Task.Run(async () =>
            {
                while (condition()) await Task.Delay(frequency);
            });

            if (waitTask != await Task.WhenAny(waitTask, Task.Delay(timeout)))
                throw new TimeoutException();
        }

        public static async Task WaitUntil(Func<bool> condition, int frequency = 25, int timeout = -1)
        {
            var waitTask = Task.Run(async () =>
            {
                while (!condition()) await Task.Delay(frequency);
            });

            if (waitTask != await Task.WhenAny(waitTask,
                    Task.Delay(timeout)))
                throw new TimeoutException();
        }

        public static async Task WaitUntil(Func<Task<bool>> condition, int frequency = 25, int timeout = -1)
        {
            var waitTask = Task.Run(async () =>
            {
                while (!await condition()) await Task.Delay(frequency);
            });

            if (waitTask != await Task.WhenAny(waitTask,
                    Task.Delay(timeout)))
                throw new TimeoutException();
        }

        public static void FireAndForget(this Task task, ILogger logger = null)
        {
            try
            {
                Task.Factory.StartNew(async () => await task);
            }
            catch (Exception e)
            {
                if (null != logger)
                    logger.LogError(e, "FireAndForget exception={0}", e.Message);
            }
        }

        public static void FireAndForget<T>(Func<T> function, ILogger logger = null)
        {
            try
            {
                Task.Factory.StartNew(function);
            }
            catch (Exception e)
            {
                if (null != logger)
                    logger.LogError(e, "FireAndForget exception={0}", e.Message);
            }
        }
    }
}
