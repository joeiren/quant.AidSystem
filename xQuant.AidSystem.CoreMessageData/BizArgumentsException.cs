using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.CoreMessageData
{
    public class BizArgumentsException : AidException
    {
        private const string SpecTitle = "调用外围系统接口时，参数信息不对！";
        
        public BizArgumentsException() : base(SpecTitle)
        {
            base.ExceptionTitle = SpecTitle;
        }

        public BizArgumentsException(string msg)
            : base(SpecTitle, msg, null)
        {            
        }

    }

    public class AidException : Exception
    {
        public string ExceptionTitle
        {
            get;
            set;

        }
        public AidException(string title) : base()
        {       
            ExceptionTitle = title;
        }

        public AidException(string title, string msg, Exception innerex)
            : base(string.Format("{0}\r\n{1}", title, msg), innerex)
        {
            ExceptionTitle = title;
        }
    }
}
