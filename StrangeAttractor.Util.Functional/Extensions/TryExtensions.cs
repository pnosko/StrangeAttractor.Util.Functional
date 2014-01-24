﻿using System;
using StrangeAttractor.Util.Functional.Interfaces;
using StrangeAttractor.Util.Functional.Implementation;
using StrangeAttractor.Util.Functional.Singletons;

namespace StrangeAttractor.Util.Functional.Extensions
{
	public static class TryExtensions
	{
		/// <summary>
		/// Retrieves the encapsulated value (on success), or the provided default value (on failure).
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="self"></param>
		/// <param name="default">The default value in case of failure.</param>
		/// <returns></returns>
		public static T GetOrDefault<T>(this ITry<T> self, T @default)
		{
			return self.IsSuccess ? self.Value : @default;
		}

		public static ITry<T> OnFailure<T>(this ITry<T> self, Func<T> invoke)
		{
			return self.IsSuccess ? self : Try.Invoke(invoke);
		}

		public static void OnFailure<T>(this ITry<T> self, Action invoke)
		{
			if (self.IsFailure)
				Try.InvokeAction(invoke);
		}

		/// <summary>
		/// Creates an instance of Either from Try, projecting the exception as a 'left' (on failure) and value as a 'right' (on success).
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="self"></param>
		/// <returns>Either of an exception and a value.</returns>
		public static IEither<Exception, T> ToEither<T>(this ITry<T> self)
		{
			return self.Fold(Either.Left<Exception, T>, Either.Right<Exception, T>);
		}
	}
}