using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;

using Newtonsoft.Json;
using System.Threading;

using log4net;

using pPrimer.Monitor.Loggers;
using pPrimer.Business;

namespace pPrimer.Monitor
{
    public class PerformanceMonitor
    {
        private readonly TimeSpan pollInterval = TimeSpan.FromSeconds(1);

        private readonly TimeSpan waitTimeout = TimeSpan.FromSeconds(3);

        private CancellationTokenSource _cancellationSource;

        private readonly ILogWrapper _log;

        private Task _monitoringTask;

        private readonly string _serviceUrl;

        public PerformanceMonitor(ILogWrapper log, string stateServiceUrl)
        {
            _log = log;
            _serviceUrl = stateServiceUrl;
        }

        public void Start()
        {
            _cancellationSource = new CancellationTokenSource();
            var cancellationToken = _cancellationSource.Token;

            // Use ThreadPool for long running task, 1 thread won't affect ThreadPool performance
            _monitoringTask = Task.Run(
                               async () =>
                               {
                                   var isHeaderPosted = false;
                                   while (!_cancellationSource.IsCancellationRequested)
                                   {
                                       var states = await GetState(cancellationToken);

                                       var results = JsonConvert.DeserializeObject<IEnumerable<PerformanceState>>(states);

                                       if (results != null)
                                       {
                                           foreach (var result in results)
                                           {
                                               if (!isHeaderPosted)
                                               {
                                                   _log.PostToStateLog(PerformanceStateFormatter.GetMessageForStateLogHeader(result));
                                                   isHeaderPosted = true;
                                               }

                                               _log.PostToConcole(PerformanceStateFormatter.GetMessageForConcole(result));
                                               _log.PostToStateLog(PerformanceStateFormatter.GetMessageForStateLog(result));
                                           }
                                       }

                                       await Task.Delay(pollInterval, cancellationToken);
                                   }
                               },
                               cancellationToken);
        }


        public void Stop()
        {
            try
            {
                _cancellationSource.Cancel();

                _monitoringTask.Wait(waitTimeout);
            }
            catch (AggregateException aex)
            {
                foreach (var ex in aex.Flatten().InnerExceptions)
                    _log.PostToLog($"Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                _log.PostToLog($"Unexpected error: {ex.Message}");
            }
            finally
            {
                _cancellationSource.Dispose();
            }
        }

        private async Task<string> GetState(CancellationToken cancellationToken)
        {
            HttpWebResponse response = null;
            string result = string.Empty;
            
            try
            {
                var request = WebRequest.Create(_serviceUrl);
                request.Method = "POST";
                request.ContentLength = 0;

                response = (HttpWebResponse)await request.GetResponseAsync();

                if(response != null)
                    using (var sr = new StreamReader(response.GetResponseStream()))
                        result = sr.ReadToEnd();
            }
            catch (WebException we)
            {
                if (we.Status == WebExceptionStatus.ProtocolError)
                {
                    response = (HttpWebResponse)we.Response;
                    _log.PostToLog($"Errorcode: {(int)response.StatusCode}");
                }
                else
                {
                    _log.PostToLog($"Error: {we.Status}");
                }
            }
            catch (Exception ex)
            {
                _log.PostToLog($"Unexpected error: {ex.Message}");
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
            }

            return result;
        }
    }
}
