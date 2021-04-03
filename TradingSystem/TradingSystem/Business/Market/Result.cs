using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Business.Market
{
    class Result<T>
    {
        private T ret;
        private bool isErr;
        private string err;

        public Result(T ret, bool isErr, string err)
        {
            this.ret = ret;
            this.isErr = isErr;
            this.err = err;
        }

        public T Ret { get => ret; set => ret = value; }
        public bool IsErr { get => isErr; set => isErr = value; }
        public string Err { get => err; set => err = value; }
    }
}