using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace xQuant.AidSystem.CoreMessageData
{
    public sealed class CommonDataHelper
    {
        private CommonDataHelper()
        { }

        #region 同业存放
        /// <summary>
        /// 同业存放计提多包中每个Message中最大包容量
        /// </summary>
        public static readonly int INTER_BANK_PREPARE_CAPABILITY = 80;

        /// <summary>
        /// 同业存放结息多包中每个Message中最大包容量
        /// </summary>
        public static readonly int INTER_BANK_INTEREST_CAPABILITY = 5;

        /// <summary>
        /// 同业存放结息接口中多包的记录最大条数
        /// </summary>
        public static readonly int INTER_BANK_INTEREST_MAX_RECORD_COUNT = 20;

        /// <summary>
        /// 同业存放结息多包中的记录ID
        /// </summary>
        public static readonly string INTER_BANK_INTEREST_RECORD_ID = "ZJYWIB01";

        /// <summary>
        /// 同业存放计提多包中的记录ID
        /// </summary>
        public static readonly string INTER_BANK_PREPARE_RECORD_ID = "ZJYWIB02";

        /// <summary>
        /// 同业存放结息接口的摘要代码
        /// </summary>
        public static readonly string INTER_BANK_INTEREST_SUMMARY_CODE = "2109";
        #endregion

        public enum SDB_TYPE_ID
        {
            SDB_TYPE_RQHDR = 1,    //请求信息头
            SDB_TYPE_RQDTL = 2,    //请求输入字段
            SDB_TYPE_RPHDR = 3,    //交易应答信息头
            SDB_TYPE_OMSG = 4,    //输出信息,包含各类型信息体
            SDB_TYPE_ODATA = 5,    //输出数据,各应用自定义数据结构
            SDB_TYPE_OBDATA = 6,    //多包下传数据
            SDB_TYPE_IBDATA = 7,    //多包上传数据
            SDB_TYPE_OPGCTL = 8,    //输出翻页控制
            SDB_TYPE_BRCTL = 9,    //输出数据浏览控制
            SDB_TYPE_AUTHREQ = 10,   //要求授权信息
            SDB_TYPE_AUTHRES = 11,   //授权柜员信息
            SDB_TYPE_AUTHCTL = 12,   //授权控制信息
            SDB_TYPE_SESSCTL = 13,   //会话控制信息
            SDB_TYPE_SECCTL = 14,   //安全控制信息
            SDB_TYPE_FLDERR = 15,   //输入字段错误信息
            SDB_TYPE_HOSTCMD = 16,   //核心命令信息
            SDB_TYPE_SYSERR = 17,   //系统错误信息
            SDB_TYPE_RESEND = 18,   //重发信息
        }

        //标准数据块 Id的关键字
        public static readonly String[] SDB_TYPE_KEY = new String[]
        {
            "@RQHDR",
            "@RQDTL",
            "@RPHDR",
            "@OMSG",
            "@ODATA",
            "@OBDATA",
            "@IBDATA",
            "@OPGCTL",
            "@BRCTL",
            "@AUTHREQ",
            "@AUTHRES",
            "@AUTHCTL",
            "@SESSCTL",
            "@SECCTL",
            "@FLDERR",
            "@HOSTCMD",
            "@SYSERR",
            "@RESEND"
        };

        public static string StrTrimer(string orignal, string trimer)
        {
            if (string.IsNullOrEmpty(orignal))
            {
                return "";
            }
            if (string.IsNullOrEmpty(trimer))
            {
                return orignal.TrimStart().TrimEnd();
            }
            else
            {
                return orignal.Trim(trimer.ToCharArray());
            }
        }

        public static string PadLeft4BizFlowNO(string orignal, char especial, int minimum)
        {
            if (string.IsNullOrEmpty(orignal))
            {
                return new string(especial, minimum);
            }
            else if (orignal.Length > minimum)
            {
                return orignal;
            }
            else
            {
                return orignal.PadLeft(minimum, especial);
            }
        }

        public static Decimal ConvertDecimal(String src, int digits)
        {
            Decimal dec = new Decimal();
            Decimal.TryParse(src, out dec);
            return Decimal.Round(dec, digits);
        }

        /// <summary>
        /// 定长空格串
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public static String SpaceString(UInt16 count)
        {
            return String.Empty.PadLeft(count);
        }
        /// <summary>
        /// 数字型字符串右对齐，左补零
        /// </summary>
        /// <returns></returns>
        public static String FillSpecifyWith0(String number, UInt16 width)
        {
            if (String.IsNullOrEmpty(number))
            {
                number = String.Empty;
            }
            if (number.Length > width)
            {
                return number.Substring(0, width);
            }
            else
            {
                return number.PadLeft(width, '0');
            }
        }
        /// <summary>
        /// 整数右对齐，左补零
        /// </summary>
        /// <returns></returns>
        public static String FillSpecifyWith0(UInt32 number, UInt16 width)
        {
            String value = number.ToString();
            if (value.Length > width)
            {
                return value.Substring(0,width);
            }
            else
            {
                return value.PadLeft(width, '0');
            }
        }
        /// <summary>
        /// 带小数右对齐，左补零
        /// </summary>
        /// <returns></returns>
        public static String FillSpecifyWith0(Double number, UInt16 width)
        {
            String value = number.ToString();
            if (value.Length > width)
            {
                return value.Substring(0, width);
            }
            else
            {
                return value.PadLeft(width, '0');
            }
        }
        
        /// <summary>
        /// 字符补足
        /// </summary>
        /// <param name="src"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public static String FillSpecifyWidthString(String src, UInt16 width)
        {
            if (src == null)
            {
                return SpaceString(width);
            }
            else
            {
                if (src.Length > width)
                {
                    return src.Substring(0, width);
                }
                else
                {
                    return src.PadRight(width);
                }
            }
        }

        /// <summary>
        /// 数字对齐补足
        /// </summary>
        /// <param name="src"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public static String FillSpecifyWidthFigure(Double src, UInt16 width)
        {
            string srcstring = src.ToString();
            if (srcstring.Length > width)
            {
                return srcstring.Substring(0, width);
            }
            else
            {
                return srcstring.PadLeft(width);
            }
        }

        /// <summary>
        /// 数字对齐补足
        /// </summary>
        /// <param name="src"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public static String FillSpecifyWidthFigure(String src, UInt16 width)
        {
            if (String.IsNullOrEmpty(src))
            {
                return SpaceString(width);
            }
            if (src.Length > width)
            {
                return src.Substring(0, width);
            }
            else
            {
                return src.PadLeft(width);
            }
        }

        public static String GetValueFromBytes(ref byte[] bytes, UInt16 width)
        {
            if (bytes.Length < width)
            {
                return String.Empty;
            }
            StringBuilder sb = new StringBuilder(width);
            int len = EBCDICEncoder.EBCDICToWideChar(EBCDICEncoder.CCSID_IBM_1388, bytes, width, sb, sb.Capacity);
            bytes = SubBytes(bytes, width, bytes.Length - width);
            if (len > 0)
            {                
                return sb.ToString().Substring(0, len);
            }

            return String.Empty;
        }

        /// <summary>
        /// 获取指定缓冲区的片段
        /// </summary>
        /// <param name="src"></param>
        /// <param name="begin"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static byte[] SubBytes(byte[] src, int begin, int length)
        {
            if (src.Length > 0 && begin < src.Length && length > 0 && src.Length - begin >= length)
            {
                byte[] newbytes = new byte[length];
                Array.Copy(src, begin, newbytes, 0, length);
                return newbytes;
            }
            return src;
        }
        
        /// <summary>
        /// 对字符进行编码，并按指定位数和左右对齐放置到缓冲区
        /// </summary>
        /// <param name="src"></param>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="leftalign"></param>
        /// <returns></returns>
        public static int ResetByteBuffer(StringBuilder src, ref byte[] buffer, ref int offset, bool leftalign) 
        {
            byte[] temp = new byte[src.Length * 2];
            int len = EBCDICEncoder.WideCharToEBCDIC(EBCDICEncoder.CCSID_IBM_1388, src.ToString(), src.Length, temp, temp.Length);
            int begin = leftalign ? 0 : src.Length;

            temp = CommonDataHelper.SubBytes(temp, begin, src.Length);
            Array.Copy(temp, 0, buffer, offset, temp.Length);
            offset += temp.Length;
            return temp.Length;
        }

        /// <summary>
        /// 把一个双字节的值转换成网络字节序
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static byte[] ToNetworkOrder(short src)
        {
            src = IPAddress.HostToNetworkOrder(src);
            return BitConverter.GetBytes(src);
        }
        
        /// <summary>
        /// 从网络字节序获得1个双字节的值
        /// </summary>
        /// <param name="network"></param>
        /// <returns></returns>
        public static short FromNetworkOrder(byte[] network, int offset)
        {
            short result = BitConverter.ToInt16(network, offset);
            return IPAddress.NetworkToHostOrder(result);
        }

        public static int ResetGBKByteBuffer(StringBuilder src, ref byte[] buffer, ref int offset, bool leftalign)
        {
            //byte[] temp = new byte[src.Length * 2];
            Encoding gbk = Encoding.GetEncoding(936);

            byte[] temp = gbk.GetBytes(src.ToString());
            int begin = leftalign ? 0 : src.Length;
            temp = CommonDataHelper.SubBytes(temp, begin, src.Length);
            Array.Copy(temp, 0, buffer, offset, temp.Length);
            offset += temp.Length;
            return temp.Length;
        }

        public static String GetValueFromGBKBytes(ref byte[] bytes, UInt16 width)
        {
            if (bytes.Length < width)
            {
                return String.Empty;
            }
            Encoding gbk = Encoding.GetEncoding(936);
            byte[] src = SubBytes(bytes, 0, width);
            byte[] dest = Encoding.Convert(gbk, Encoding.Unicode, src);
            String result = Encoding.Unicode.GetString(dest);
            bytes = SubBytes(bytes, width, bytes.Length - width);

            return result;
        }


        public static byte[] WideCharToGBK(String src)
        {
            Encoding gbk = Encoding.GetEncoding(936);
            byte[] bytes = gbk.GetBytes(src.ToString());
            //byte[] returnbyte = new byte[bytes.Length];
            //int i = 0;
            //foreach (byte b in bytes)
            //{
            //    //UInt16 x = (UInt16)((Convert.ToUInt16(b) / 16 *10)+ Convert.ToUInt16(b) % 16);

            //    returnbyte[i] = (byte)Convert.ToUInt16(Convert.ToString((UInt16)b, 16),10);
            //    i++;
            //}
            //return returnbyte;
            return bytes;
        }

        public static String GBKTOWideChar(byte[] src)
        {
            Encoding gbk = Encoding.GetEncoding(936);
            byte[] dest = Encoding.Convert(gbk, Encoding.ASCII, src);
            return Encoding.ASCII.GetString(dest);
        }
    }

    /*
    public class BufferSharing
    {
        public static readonly int MAX_LENGTH = 1024 * 1024 * 64; // 64M 共享缓存

        private byte[] ThisBuffer;
        private List<BufferInfo> LocatedList;

        private BufferSharing()
        {
            if (ThisBuffer == null)
            {
                ThisBuffer = new byte[MAX_LENGTH];
                LocatedList = new List<BufferInfo>();
            }
        }

        private static BufferSharing _instance;
        public static BufferSharing GetInstance()
        {
            if (_instance == null)
            {
                lock (typeof(BufferSharing))
                {
                    return _instance = new BufferSharing();
                }
            }
            return _instance;
        }

        public byte[] LocateNewBuffer(int size)
        {
            ThisBuffer.
        }

        public bool FreeLocated()

        


    }

    public struct BufferInfo
    {
        public int BeginIndex
        {
            get;
            set;
        }
        public int Length
        {
            get;
            set;
        }
    }
    */
}
