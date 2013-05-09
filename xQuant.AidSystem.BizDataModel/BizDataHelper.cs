using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.BizDataModel
{
    public class BizDataHelper
    {
        #region 内部帐账号生成
        private const int ZH_LEN = 32;
        private static int[] iPrime = new int[] { 53, 47, 43, 41, 37, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31 };
        public static bool GenerateInnerAcctNO(string orgno, string currency, string checkcode, string innersn, out string result)
        {
            result = "";
            if (string.IsNullOrEmpty(orgno) || string.IsNullOrEmpty(currency) || string.IsNullOrEmpty(checkcode) || string.IsNullOrEmpty(innersn))
            {
                throw new Exception(string.Format("内部账号生成要素不全，机构号：{0}，币种：{1}，核算码：{2}， 内部账序号：{3}.", orgno, currency, checkcode, innersn));
            }
            if (orgno.Length != 6 || currency.Length != 2 || checkcode.Length != 6 || innersn.Length != 4)
            {
                throw new Exception(string.Format("内部账号生成要素不全，机构号：{0}，币种：{1}，核算码：{2}， 内部账序号：{3}.", orgno, currency, checkcode, innersn));
            }
            StringBuilder innerAcct = new StringBuilder(string.Format("{0}{1}{2}{3}", orgno, currency, checkcode, innersn));
            return GenerateInnerAcctNO(innerAcct.ToString(), out result);
        }

        public static bool GenerateInnerAcctNO(string orignal, out string result)
        {
            try
            {
                int iLoop = 0;
                int iCac = 0;
                char[] acString = new char[3];
                char[] acAcctNum = new char[ZH_LEN + 1];
                bool iFlag = orignal.Length % 2 != 0;/* ZH是否偶数位 */
                int Len = 20;
                Array.Copy(orignal.ToCharArray(), acAcctNum, Len - 2);

                for (iLoop = 0; iLoop < (Len - 1) / 2; iLoop++)  /* 0 -> 6或8或9    */
                {
                    if (iLoop == (Len - 3) / 2 && iFlag)  /* 6或8或9 && Len非偶   */
                    {
                        acString[1] = acAcctNum[Len - 3];
                        acString[0] = '0';
                    }
                    else
                    {
                        //char [] temp = new char[acAcctNum.Length-iLoop*2];
                        //if (iLoop > 0)
                        {
                            //strncpy( acString, acAcctNum + ( iLoop * 2 ), 2 );
                            char[] actemp = new char[2];
                            Array.Copy(acAcctNum, iLoop * 2, actemp, 0, 2);
                            Array.Copy(actemp, acString, 2);
                        }

                    }
                    int acvalue = 0;
                    if (int.TryParse(new string(acString), out acvalue))
                    {
                        iCac += iPrime[iLoop + (32 - Len) / 2] * acvalue;
                    }
                }
                
                iCac %= 100;
                //char[] formattemp = new char[Len - 3];
                //Array.Copy(acAcctNum, formattemp, Len - 3);
                string tempstring = new string(acAcctNum, 0, Len - 2);

                result = string.Format("{0}{1:d2}", tempstring, iCac);

                return true;
            }
            catch
            {
                throw new Exception(string.Format("内部账生成出错，原18位账号是{0}，请校验！", orignal));
            }
        }

        #endregion
    }
}
