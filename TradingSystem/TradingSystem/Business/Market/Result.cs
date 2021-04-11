using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Business.Market
{
    public class Result<T>
    {
        private T ret;
        private bool isErr;
        private string mess;

        public Result(T ret, bool isErr, string err)
        {
            this.ret = ret;
            this.isErr = isErr;
            this.mess = mess;
        }

        public T Ret { get => ret; set => ret = value; }
        public bool IsErr { get => isErr; set => isErr = value; }
        public string Mess { get => mess; set => mess = value; }
    }
}