using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pPrimer.Monitor.Loggers;
using pPrimer.Monitor.Properties;

namespace pPrimer.Monitor
{
    class Monitor
    {
        static void Main()
        {
            log4net.Config.XmlConfigurator.Configure();
            log4net.ILog log = log4net.LogManager.GetLogger(typeof(Monitor));

            var logWrapper = new LogWrapper(log);
            var performanceMonitor = new PerformanceMonitor(logWrapper, Settings.Default.StateService);

            performanceMonitor.Start();

            Console.ReadLine();

            performanceMonitor.Stop();
        }
    }
}
