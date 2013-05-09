using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.CoreMessageData
{
    /// <summary>
    /// 柜员密码修改DTL(请求数据）类
    /// </summary>
    public class TellerChangePwdRQDTL : IMessageReqHandler
    {
        /// <summary>
        /// 数据块长
        /// </summary>
        public const UInt16 TOTAL_WIDTH = 48;

        #region Property
        /// <summary>
        /// 旧密码
        /// </summary>
        public string OldPwd
        {
            get;
            set;
        }
        /// <summary>
        /// 加密后的旧密码
        /// </summary>
        public byte[] EncyrptOldPwd
        {
            get;
            set;
        }

        /// <summary>
        /// 新密码
        /// </summary>
        public string NewPwd
        {
            get;
            set;
        }

        /// <summary>
        /// 加密后的新密码
        /// </summary>
        public byte[] EncyrptNewPwd
        { 
            get;
            set;
        }
        #endregion

        #region IMessageReqHandler 成员

        /// <summary>
        /// 转换成二进制流
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes()
        {
            int totalLen = 0;
            byte[] bytes = new byte[TOTAL_WIDTH];
            //StringBuilder sb = new StringBuilder();
            //sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(OldPwd, 6));
            //CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            //sb.Remove(0, sb.Length);

            //sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(NewPwd, 6));
            //CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            //sb.Remove(0, sb.Length);

            if (EncyrptOldPwd == null)
            {
                EncyrptOldPwd = new byte[24];
            }
            Array.Copy(EncyrptOldPwd, 0, bytes, totalLen, EncyrptOldPwd.Length);
            totalLen += EncyrptOldPwd.Length;

            if (EncyrptNewPwd == null)
            {
                EncyrptNewPwd = new byte[24];
            }
            Array.Copy(EncyrptNewPwd, 0, bytes, totalLen, EncyrptNewPwd.Length);
            totalLen += EncyrptNewPwd.Length;

            return bytes;
        }

        #endregion
    }
}
