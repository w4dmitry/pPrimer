using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace pPrimer.Web.App_Start
{
    public class LoggerConfig
    {
        public static void ConfigureLogger()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
    }
}