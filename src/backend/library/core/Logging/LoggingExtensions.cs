﻿using System;
using System.Collections.Generic;
using System.Text;

using log4net;

namespace Notary.Logging
{
    public static class LoggingExtensions
    {
        public static Exception IfNotLoggedThenLog(this Exception ex, ILog log)
        {
            if (ex == null || ex.GetType() == typeof(LoggedException))
                return ex;

            log.Error(ex.Message, ex);

            return new LoggedException("An error has occured", ex);
        }
    }
}
