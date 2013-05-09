using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace xQuant.AidSystem.CoreMessageData
{
    public class EBCDICEncoder
    {
        public const uint CCSID_IBM_1388 = 1388;
        [DllImport("EBCDICEncoder.dll", CharSet = CharSet.Unicode)]
        public static extern int EBCDICToWideChar(uint ccsid, byte[] ebcdicStr, int ebcdicLen, StringBuilder unicodeStr, int unicodeLen);

        [DllImport("EBCDICEncoder.dll", CharSet = CharSet.Unicode)]
        public static extern int WideCharToEBCDIC(uint ccsid, String unicodeStr, int unicodeLen, byte[] ebcdicStr, int ebcdicLen);
    }
}
