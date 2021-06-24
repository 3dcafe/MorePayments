using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace MorePayments.Diagnostics
{
    public class Debug
    {
        public static Log Log { get; internal set; }
    }

    public class Log
    {
        internal void Info(string v)
        {
           // throw new NotImplementedException();
        }

        internal void Error(string error, WebException ex)
        {
          //  throw new NotImplementedException();
        }

        internal void Error(WebException ex)
        {
          //  throw new NotImplementedException();
        }

        internal void Error(Exception ex)
        {
           // throw new NotImplementedException();
        }
    }
}
