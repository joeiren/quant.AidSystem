using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.CoreMessageData
{
    public abstract class BizMsgDataBase
    {
        public abstract UInt32 RQ_TOTAL_WIDTH
        {
            get;
            set;
        }

        public abstract UInt32 RP_TOTAL_WIDTH
        {
            get;
            set;
        }

        public virtual bool OnArgumentsValidation()
        {
            return true;
        }
    }
}
