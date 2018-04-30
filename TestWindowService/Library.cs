using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWindowService
{
    class Library
    {
        /// <summary>
        /// Si une exception est déclenchée , on l'écrit dans le journal de log.
        /// </summary>
        /// <param name="ex"></param>
        public static void WriteErrorLog(Exception ex)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogFile.txt", true);
                sw.WriteLine(DateTime.Now.ToString() + " : " + ex.Source.ToString().Trim());
                sw.Flush();
                sw.Close();
            }
            catch
            {

            }
        }

        /// <summary>
        /// Fonction permettant de remplir le journal de log.
        /// </summary>
        /// <param name="Message">Message à inséré dans le journal de log</param>
        public static void WriteErrorLog(String Message)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogFile.txt", true);
                sw.WriteLine(DateTime.Now.ToString() + " : " + Message);
                sw.Flush();
                sw.Close();
            }
            catch
            {

            }
        }
    }
}
