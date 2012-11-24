using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LordJZ.Linq
{
    partial class Enumerable
    {
        internal static Func<TSource, TResult> CombineSelectors<TSource, TMiddle, TResult>
            (Func<TSource, TMiddle> selector1, Func<TMiddle, TResult> selector2)
        {
            return x => selector2(selector1(x));
        }
    }
}
