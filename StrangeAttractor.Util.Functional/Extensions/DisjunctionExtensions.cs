﻿using System;
using StrangeAttractor.Util.Functional.Interfaces;
using StrangeAttractor.Util.Functional.Singletons;

namespace StrangeAttractor.Util.Functional.Extensions
{
    public static class DisjunctionExtensions
    {
        public static IDisjunction<TResultError, TResultValue> SelectMany<TError, TValue, TResultError, TResultValue>(this IDisjunction<TError, TValue> self, Func<TValue, IDisjunction<TResultError, TResultValue>> selector)
            where TError : TResultError
        {
            return self.Fold(
                e => Disjunction.Left<TResultError, TResultValue>((TResultError)e),
                s => selector(s));
        }

        public static IDisjunction<TResultError, TResultValue> SelectMany<TError, TValue, TIntermediate, TResultError, TResultValue>(this IDisjunction<TError, TValue> self, Func<TValue, IDisjunction<TResultError, TIntermediate>> intermediate, Func<TValue, TIntermediate, TResultValue> selector)
            where TError : TResultError
        {
            return self.Fold(
                e => Disjunction.Left<TResultError, TResultValue>((TResultError)e),
                s => intermediate(s).Select(i => selector(s, i)));
        }

        public static ITry<TValue> AsTry<TError, TValue>(this IDisjunction<TError, TValue> self)
            where TError : Exception
        {
            return self.Fold(
                e => Try.Failure<TValue>(e),
                s => Try.Success<TValue>(s));
        }

        public static ITry<TValue> AsTry<TError, TValue>(this IDisjunction<TError, TValue> self, Func<TError, Exception> toFailure)
        {
            return self.Fold(
                e => Try.Failure<TValue>(toFailure(e)),
                s => Try.Success<TValue>(s));
        }

        public static TResultValue GetOrElse<TError, TValue, TResultValue>(this IDisjunction<TError, TValue> self, Func<TResultValue> @default)
            where TValue : TResultValue
            where TResultValue : class  // TODO: Remove this limitation
        {
            return self.AsOption().As<TResultValue>().GetOrElse(@default);
        }

        public static IDisjunction<TResultError, TResultValue> OrElse<TError, TValue, TResultError, TResultValue>(this IDisjunction<TError, TValue> self, Func<IDisjunction<TResultError, TResultValue>> @default)
            where TValue : TResultValue
            where TError : TResultError
        {
            // TODO: Test
            return self.Fold(
                e => @default(),
                s => (IDisjunction<TResultError, TResultValue>)self);
        }
    }
}
