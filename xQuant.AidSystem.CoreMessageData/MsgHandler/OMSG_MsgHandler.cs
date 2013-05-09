using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace xQuant.AidSystem.CoreMessageData
{
    public class OMSG_MsgHandler : IMessageRespHandler
    {
        public UInt16 TOTAL_WIDTH
        {
            get 
            {
                return (UInt16)(OMSG_Item_Handler.TOTAL_WIDTH * NUM_ENT);
            }
        }
        // 2位
        public UInt16 NUM_ENT
        {
            get;
            set;
        }

        public List<OMSG_Item_Handler> OMSGItemList
        {
            get;
            set;
        }

        public OMSG_MsgHandler()
        {
            OMSGItemList = new List<OMSG_Item_Handler>();
        }

        #region IMessageRespHandler Members

        public object FromBytes(byte[] messagebytes)
        {
            
                byte[] bytebuffer = new byte[2];
                if (messagebytes.Length >= 2)
                {
                    UInt16 number = 0;
                    Array.Copy(messagebytes, bytebuffer, 2);
                    if (UInt16.TryParse(CommonDataHelper.GetValueFromBytes(ref bytebuffer, 2), out number))
                    {
                        NUM_ENT = number;
                    }
                }
                if (NUM_ENT > 0)
                {
                    int i = 0;
                    int offset = 2;
                    while (i++ < NUM_ENT && messagebytes.Length >= offset)
                    {
                        bytebuffer = new byte[OMSG_Item_Handler.TOTAL_WIDTH];
                        Array.Copy(messagebytes, offset, bytebuffer, 0, OMSG_Item_Handler.TOTAL_WIDTH);
                        //ms.Read(bytebuffer, offset, OMSG_Item_Handler.TOTAL_WIDTH);
                        OMSG_Item_Handler item = new OMSG_Item_Handler();
                        item = (OMSG_Item_Handler)item.FromBytes(bytebuffer);                        
                        OMSGItemList.Add(item);
                        offset += OMSG_Item_Handler.TOTAL_WIDTH;
                    }
                }

            
            return this;
        }

        #endregion
    }

    public class OMSG_Item_Handler : IMessageRespHandler
    {
        public const UInt16 TOTAL_WIDTH = 49;
        /// <summary>
        /// 应用模块缩写代码,2
        /// </summary>
        public String MOD_ID
        {
            get;
            set;
        }
        /// <summary>
        /// 信息种类,1
        /// </summary>
        public String MSG_TYPE
        {
            get;
            set;
        }
        /// <summary>
        /// 信息码,4
        /// </summary>
        public String MSG_NO
        {
            get;
            set;
        }
        /// <summary>
        /// 信息内容,42
        /// </summary>
        public String MSG_TEXT
        {
            get;
            set;
        }
        #region IMessageRespHandler Members

        public object FromBytes(byte[] messagebytes)
        {
            if (messagebytes.Length >= TOTAL_WIDTH)
            {
                
                byte[] buffer = new byte[TOTAL_WIDTH];
                //ms.Read(buffer, 0, TOTAL_WIDTH);
                Array.Copy(messagebytes, buffer, TOTAL_WIDTH);    
                String result = CommonDataHelper.GetValueFromBytes(ref buffer, TOTAL_WIDTH);
                MOD_ID = result.Substring(0, 2).TrimEnd();
                MSG_TYPE = result.Substring(2, 1).TrimEnd();
                MSG_NO = result.Substring(3, 4).TrimEnd();
                MSG_TEXT = result.Substring(7).TrimEnd();
                
            }

            return this;
        }

        #endregion
    }
}
