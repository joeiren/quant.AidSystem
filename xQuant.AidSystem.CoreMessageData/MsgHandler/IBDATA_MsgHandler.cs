using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.CoreMessageData
{
    /// <summary>
    /// 多包上传中的一个包对象
    /// </summary>
    public abstract class IBDATA_MsgHandler : IMessageReqHandler
    {
        public int TOTAL_WIDTH
        {
            get
            {
                return 24 + RecordLength * RecordCollection.Count;
            }
        }
        /// <summary>
        /// 记录格式ID,8
        /// </summary>
        public String RecordID
        {
            get;
            set;
        }
        /// <summary>
        /// 每条记录长度,8
        /// </summary>
        public int RecordLength
        {
            get;
            set;
        }
        /// <summary>
        /// 记录条数,8
        /// </summary>
        public int RecordNumber
        {
            get
            {
                return RecordCollection.Count;
            }
            set
            {
                
            }
        }

        /// <summary>
        /// 记录
        /// </summary>
        public List<IBDATA_Record> RecordCollection
        {
            get;
            set;
        }


        #region IMessageReqHandler Members

        public byte[] ToBytes()
        {
            int totalLen = 0;
            byte[] bytes = new byte[TOTAL_WIDTH];
            StringBuilder sb = new StringBuilder();
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(RecordID, 8));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthFigure(RecordLength, 8));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthFigure(RecordNumber, 8));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            if (RecordCollection != null)
            {
                foreach (var record in RecordCollection)
                {
                    Array.Copy(record.ToBytes(), 0, bytes, totalLen, record.TOTAL_WIDTH);
                    totalLen += record.TOTAL_WIDTH;
                }                
            }
            return bytes;

        }

        #endregion
    }

    /// <summary>
    /// 单个包里面的一个记录
    /// </summary>
    public abstract class IBDATA_Record : IMessageReqHandler
    {
        public IBDATA_Record()
        {
        }
        public int TOTAL_WIDTH
        {
            get;
            set;
        }

        #region IMessageReqHandler Members

        public virtual byte[] ToBytes()
        {
            return null;
        }

        #endregion
    }
}
