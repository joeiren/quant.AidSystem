using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TestService
{
    public class CommonMethods
    {
        public static bool WriteLocalGBKFile(string fullPath, string[] content)
        {
            if (string.IsNullOrEmpty(fullPath))
            {
                return false;
            }
            try
            {
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }
                //using (StreamWriter sw = File.AppendText(fullPath))
                //{
                //    foreach (string c in content)
                //    {
                //        sw.WriteLine(c);
                //    }
                //}
                File.WriteAllLines(fullPath, content, Encoding.GetEncoding(936));
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
