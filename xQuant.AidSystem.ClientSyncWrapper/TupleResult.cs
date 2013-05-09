using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.ClientSyncWrapper
{
    /// <summary>
    /// 多结果返回结构
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    public struct TupleResult<T1, T2> : IEquatable<TupleResult<T1,T2>>
    {
        private readonly T1 _first;
        public T1 First
        {
            get 
            {
                return _first; 
            }
        }
        private readonly T2 _second;
        public T2 Second
        {
            get 
            {
                return _second; 
            }
        }
        public TupleResult(T1 f, T2 s)
        {
            _first = f;
            _second = s;
        }
        #region IEquatable<TupleResult<T1,T2>> Members

        public bool Equals(TupleResult<T1, T2> other)
        {
            try
            {
                return this.First.Equals(other.First) && this.Second.Equals(other.Second);
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }

    /// <summary>
    /// 基本核心返回对象
    /// </summary>
    public struct RegularResult : IEquatable<RegularResult>
    {
        private readonly bool _succeed;
        public bool Succeed
        {
            get
            {
                return _succeed;
            }
        }

        private readonly string _exception;
        public string ExceptionMsg
        {
            get
            {
                return _exception;
            }
        }

        public RegularResult(bool successed, string exception)
        {
            _succeed = successed;
            _exception = exception;
        }
    
        #region IEquatable<RegularResult> Members

        public bool  Equals(RegularResult other)
        {
            return this.Succeed == other.Succeed && ExceptionMsg.CompareTo(other.ExceptionMsg) == 0;
        }

        #endregion
}
}
