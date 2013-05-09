using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xQuant.AidSystem.DBAction;
using xQuant.AidSystem.CoreMessageData;

namespace xQuant.AidSystem.ClientSyncWrapper
{
    /// <summary>
    /// 客户端方法集合
    /// </summary>
    public class ClientUtility
    {
        #region 同业存放结息批号生成
        public static string GenerateBatchNO(DateTime coreDate)
        {
            return string.Format("{0}{1}{2}{3}{4}", "KP", coreDate.ToString("yy"), coreDate.ToString("MM"), coreDate.ToString("dd"), CommonDataHelper.FillSpecifyWith0(S_AUTO_CORE_KP.GetNextID().ToString(), 7));
        }
        #endregion
    }
}
