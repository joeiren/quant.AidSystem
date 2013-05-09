using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.CoreMessageData
{
    public interface IMessageReqHandler
    {
         byte[] ToBytes();
    }

    public interface IMessageRespHandler
    {
         object FromBytes(byte[] messagebytes);
    }

    public interface IMessageMultiReqHandler
    {
        List<byte[]> ToMultiBytes();
    }
}
