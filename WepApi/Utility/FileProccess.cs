using System;
using System.IO;

namespace WebApi.Utility
{
    public class FileProccess
    {
        public static void WriteLog(string Message)
        {
            StreamWriter sw = null;

            try
            {
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogFile-" + DateTime.Now.ToString("yyyyMMdd") + ".txt", true);

                sw.WriteLine(DateTime.Now + " : " + Message);
                sw.Flush();
                sw.Close();
            }
            catch
            { }
        }
    }
}
