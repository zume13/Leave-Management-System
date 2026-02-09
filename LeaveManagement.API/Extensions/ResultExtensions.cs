using Microsoft.EntityFrameworkCore.Query.Internal;
using Org.BouncyCastle.Utilities.IO;
using SharedKernel.Shared.Result;
using System.Runtime.CompilerServices;

namespace LeaveManagement.API.Extensions
{
    public static class ResultExtensions
    {
        public static TOut Match<TOut>(
            this Result result,
            Func<TOut> OnSuccess,
            Func<Result, TOut> OnFailure)
        {
            return result.isSuccess
                ? OnSuccess()
                : OnFailure(result);
        }

        public static TOut Match<TIn, TOut>(
            this ResultT<TIn> result,
            Func<TIn, TOut> OnSuccess,
            Func<ResultT<TIn>, TOut> OnFailure)
        {
            return result.isSuccess
                ? OnSuccess(result.Value)
                : OnFailure(result);
        }
    }
}
