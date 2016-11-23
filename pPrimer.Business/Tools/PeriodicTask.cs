using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace pPrimer.Business.Tools
{
    public static class PeriodicTask
    { 
        public static TaskCompletionSource<T> ToPeriodicTask<T>(this Action func, TimeSpan startTimeout, TimeSpan interval)
        {
            Timer timer;
            var tcs = new TaskCompletionSource<T>();

            timer = new Timer(obj =>
                        {
                            try
                            {
                                if (!tcs.Task.IsCompleted)
                                {
                                    func();
                                }
                            }
                            catch (Exception ex)
                            {
                                tcs.TrySetException(ex);
                            }
                        }, null, startTimeout, interval);

            tcs.Task.ContinueWith(t =>
            {
                timer.Dispose();
            });

            return tcs;
        }
    }
}
