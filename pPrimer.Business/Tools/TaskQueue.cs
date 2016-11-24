using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pPrimer.Business.Tools
{
    using System.Collections.Concurrent;
    using System.Threading;

    public class TaskQueue : IDisposable
    {
        BlockingCollection<Task> _taskQueue = new BlockingCollection<Task>();

        public TaskQueue(int wokersCount)
        {
            for (int i = 0; i < wokersCount; i++) Task.Factory.StartNew(DoWork);
        }

        public Task Enqueue(Action action, CancellationToken cancellationToken = default(CancellationToken))
        {
            var newTask = new Task(action, cancellationToken);
            _taskQueue.Add(newTask);
            return newTask;
        }

        public Task<T> Enqueue<T>(Func<T> func, CancellationToken cancellationToken = default(CancellationToken))
        {
            var newTask = new Task<T>(func, cancellationToken);
            _taskQueue.Add(newTask);
            return newTask;
        }

        private void DoWork()
        {
            foreach (var task in _taskQueue.GetConsumingEnumerable())
            {
                try
                {
                    if(!task.IsCanceled)
                        task.RunSynchronously();
                }
                catch (InvalidOperationException)
                {
                    // Ignore race condition
                }
            }
        }

        public void Dispose()
        {
            this._taskQueue.CompleteAdding();
        }
    }
}
