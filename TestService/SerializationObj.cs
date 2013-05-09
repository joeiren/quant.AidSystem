using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestService
{
    [Serializable]
    public class SerializationObj
    {
        public SerializationObj()
        {
            NameList = new List<string>();
        }
        public DateTime NewDate
        {
            get;
            set;
        }

        public List<String> NameList
        {
            get;
            set;
        }
    }
}
