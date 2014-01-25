﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
//using StrangeAttractor.Util.Extensions;
using StrangeAttractor.Util.Functional.Interfaces;
using StrangeAttractor.Util.Functional.Implementation;
using StrangeAttractor.Util.Functional.Singletons;

namespace StrangeAttractor.Util.Functional.Extensions
{
	[DebuggerNonUserCode]
	public static class OptionExtensions
	{
		/// <summary>
		/// Retrieves the string representation of the encapsulated value (if exists), otherwise returns default value provided.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="self"></param>
		/// <param name="default"></param>
		/// <returns></returns>
		public static string GetString<T>(this IOption<T> self, string @default = "")
		{
			return self.HasValue ? self.Value.ToString() : @default;
		}

		/// <summary>
		/// Retrieves the encapsulated value (if exists), otherwise throws the exception provided.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="self"></param>
		/// <param name="exception">Function to generate the exception to be thrown if the value does not exist.</param>
		/// <returns></returns>
		public static T GetOrThrow<T>(this IOption<T> self, Func<Exception> exception)
		{
			if (self.IsNothing())
			{
				throw exception();
			}
			return self.Value;
		}

		/// <summary>
		/// Retrieves the encapsulated collection (if exists), or and empty collection otherwise.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="self"></param>
		/// <returns></returns>
		public static IEnumerable<T> GetOrEmpty<T>(this IOption<IEnumerable<T>> self)
		{
			return self.GetOrElse(Enumerable.Empty<T>());
		}

		/// <summary>
		/// Yields a collection with the value as a single element (if exists), or an empty collection;
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="self"></param>
		/// <returns></returns>
		public static IEnumerable<T> ToEnumerable<T>(this IOption<T> self)
		{
			if (self.IsSomething())
				yield return self.Value;
		}

		/// <summary>
		/// Retrieves 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="self"></param>
		/// <returns></returns>
		public static T GetOrNull<T>(this IOption<T> self) where T : class
		{
			return self.IsNothing() ? null : self.Value;
		}

		/// <summary>
		/// Retrieves the encapsulated value (if exists), otherwise returns the default value of type T.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="self"></param>
		/// <returns></returns>
		public static T GetOrDefault<T>(this IOption<T> self)
		{
			return self.GetOrElse(default(T));
		}

		/// <summary>
		/// Retrieves the encapsulated value (if exists), otherwise returns the default value provided.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="self"></param>
		/// <param name="default">The default value to return.</param>
		/// <returns></returns>
		public static T GetOrElse<T>(this IOption<T> self, T @default)
		{
			return self.HasValue ? self.Value : @default;
		}

		/// <summary>
		/// Retrieves the encapsulated value (if exists), otherwise returns the default value provided.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="self"></param>
		/// <param name="default">The default value to return if the value does not exist.</param>
		/// <returns></returns>
		public static T GetOrElse<T>(this IOption<T> self, Func<T> @default)
		{
			return self.HasValue ? self.Value : @default();
		}

		/// <summary>
		/// Retrieves the result of applying a provided function to the encapsulated value (if exists), otherwise returns the default value provided.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="self"></param>
		/// <param name="default">The default value to return if the value does not exist.</param>
		/// <param name="selector">The function to apply on an existent value.</param>
		/// <returns></returns>
		public static TResult SelectOrElse<T, TResult>(this IOption<T> self, Func<T, TResult> selector, Func<TResult> @default)
		{
			return self.HasValue ? selector(self.Value) : @default();
		}

		/// <summary>
		/// Retrieves the result of applying a provided function to the encapsulated value (if exists), otherwise returns the default value provided.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="self"></param>
		/// <param name="default">The default value to return if the value does not exist.</param>
		/// <param name="selector">The function to apply on an existent value.</param>
		/// <returns></returns>
		public static TResult SelectOrElse<T, TResult>(this IOption<T> self, Func<T, TResult> selector, TResult @default)
		{
			return self.HasValue ? selector(self.Value) : @default;
		}

		/// <summary>
		/// Retrieves the result of applying a provided function to the encapsulated value (if exists), otherwise returns the default value of type T.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="self"></param>
		/// <param name="default">The default value to return if the value does not exist.</param>
		/// <param name="selector">The function to apply on an existent value.</param>
		/// <returns></returns>
		public static TResult SelectOrDefault<T, TResult>(this IOption<T> self, Func<T, TResult> selector)
		{
			return self.HasValue ? selector(self.Value) : default(TResult);
		}

		/// <summary>
		/// Casts the encapsulated value to the provided type (if exists and cast successful).
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="self"></param>
		/// <returns></returns>
		public static IOption<TResult> As<T, TResult>(this IOption<T> self) where TResult : class
		{
			return from m in self
				   let t = m as TResult
				   where t != null
				   select t;
		}

		/// <summary>
		/// Casts the value to the provided type.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="self"></param>
		/// <returns>The encapsulated value, if cast was successful, otherwise nonexistent value.</returns>
		public static IOption<T> Cast<T>(this object self)
		{
			try
			{
				var t = (T)self;
				return t.ToOption();
			}
			catch
			{
				return Option.Nothing<T>();
			}
		}

		/// <summary>
		/// Converts the Option of a struct to a Nullable.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="self"></param>
		/// <returns></returns>
		public static T? ToNullable<T>(this IOption<T> self) where T : struct
		{
			return self.IsSomething() ? self.Value : new T?();
		}

		/// <summary>
		/// Flattens a nested Option to a simple Maybe monad.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="self"></param>
		/// <returns></returns>
		public static IOption<T> Flatten<T>(this IOption<IOption<T>> self)
		{
			return self.SelectMany(x => x);
		}

		/// <summary>
		/// Determines if the encapsulated value exists.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="self"></param>
		/// <returns></returns>
		public static bool IsSomething<T>(this IOption<T> self)
		{
			return self.HasValue;
		}

		/// <summary>
		/// Determines if the encapsulated value does not exist.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="self"></param>
		/// <returns></returns>
		public static bool IsNothing<T>(this IOption<T> self)
		{
			return !self.IsSomething();
		}

		public static IOption<T2> Compose<T, T2>(this IOption<T> self, IOption<T2> that)
		{
			return self.IsNothing() ? Option.Nothing<T2>() : that;
		}

		/// <summary>
		/// Returns this Option if it encapsulates a value, otherwise the provided value lifted to a Option.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="self"></param>
		/// <param name="that"></param>
		/// <returns></returns>
		public static IOption<T> OrElse<T>(this IOption<T> self, T that)
		{
			return self.IsSomething() ? self : that.ToOption();
		}

		/// <summary>
		/// Returns the first Option that encapsulates a value (if any do), otherwise nothing.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="self"></param>
		/// <param name="that"></param>
		/// <returns></returns>
		public static IOption<T> OrElse<T>(this IOption<T> self, Func<IOption<T>> that)
		{
			return self.IsSomething() ? self : that();
		}

		/// <summary>
		/// Returns the first Option that encapsulates a value (if any do), otherwise nothing.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="self"></param>
		/// <param name="that"></param>
		/// <returns></returns>
		public static IOption<T> OrElse<T>(this IOption<T> self, IOption<T> that)
		{
			return self.IsSomething() ? self : that;
		}

		//public static void RunWhenTrue(this IOption<bool> self, Action fn)
		//{
		//	if (self.HasValue && self.Value)
		//		fn();
		//}

		//public static void RunOrThrow<T>(this IOption<T> self, Action<T> action, Exception exception = null)
		//{
		//	if (!self.HasValue)
		//	{
		//		throw exception ?? new InvalidOperationException("RunOrThrow on Option threw the default exception");
		//	}

		//	action(self.Value);
		//}

		//public static void Run<T, T2, T3>(this IOption<T> self, IOption<T2> m2, IOption<T3> m3, Action<T, T2, T3> selector)
		//{
		//	if (self.IsSomething() && m2.IsSomething() && m3.IsSomething())
		//		fn(self.Value, m2.Value, m3.Value);
		//}

		//public static void Run<T, T2>(this IOption<T> self, IOption<T2> m2, Action<T, T2> selector)
		//{
		//	if (self.IsSomething() && m2.IsSomething())
		//		fn(self.Value, m2.Value);
		//}

		/// <summary>
		/// Converts the Option to an Equatable.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="self"></param>
		/// <returns></returns>
		public static IEquatable<IOption<T>> AsEquatable<T>(this IOption<T> self)
		{
			return self.Cast<IEquatable<IOption<T>>>().GetOrDefault();
		}

		/// <summary>
		/// Applies an action to an encapsulated value (if exists).
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="self"></param>
		/// <param name="action"></param>
		public static void Run<T>(this IOption<T> self, Action<T> action)
		{
			if (self.IsSomething())
				action(self.Value);
		}

		/// <summary>
		/// Applies an action to an encapsulated value (if exists), and swallows any exceptions.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="self"></param>
		/// <param name="action"></param>
		public static void SafeRun<T>(this IOption<T> self, Action<T> action)
		{
			try
			{
				self.Run(action);
			}
			catch
			{
			}
		}

		/// <summary>
		/// Converts a Option to Try monad, with success if this encapsulates a value, or failure with the provided exception otherwise.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="self"></param>
		/// <returns></returns>
		public static ITry<T> AsTry<T>(this IOption<T> self, Exception ex)
		{
			return self.SelectOrElse<T, ITry<T>>(x => Try.Success(x), Try.Failure<T>(ex));
		}

		/// <summary>
		/// Converts a Option to Try monad, with the success result of applying a provided function to the encapsulated value, or failure .
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="self"></param>
		/// <returns></returns>
		public static ITry<TResult> TrySelect<T, TResult>(this IOption<T> self, Func<T, TResult> selector)
		{
			return self.Select(x => Try.Invoke(() => selector(x))).GetOrElse(Try.Failure<TResult>(new InvalidOperationException("Option contains no value.")));
		}
	}
}