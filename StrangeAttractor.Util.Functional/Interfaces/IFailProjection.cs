﻿/// This implementation was partially inspired by scalaz.validation.Validation.FailProjection from the scalaz library
using System;

namespace StrangeAttractor.Util.Functional.Interfaces
{
	public interface IFailProjection<out TError, out TValue> : IValue<TError>, IOptionalValue<TError>
	{
		IValidation<TResultError, TValue> Select<TResultError>(Func<TError, TResultError> selector);
		IValidation<TResultError, TResultRight> SelectMany<TResultError, TResultRight>(Func<TError, IValidation<TResultError, TResultRight>> selector);
        //IValidation<TResultError, TResultRight> SelectMany<TResultError, TIntermediate, TResultRight>(Func<TError, IValidation<TIntermediate, TResultRight>> intermediate, Func<TError, TIntermediate, TResultError> selector);
	}
}