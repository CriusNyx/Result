namespace CriusNyx.Results;

/// <summary>
/// Contains an okay value.
/// </summary>
/// <typeparam name="Value"></typeparam>
/// <param name="_value"></param>
public class Ok<Value>(Value _value)
{
  /// <summary>
  /// Okay value.
  /// </summary>
  public Value value => _value;
}

/// <summary>
/// Contains an error value.
/// </summary>
/// <typeparam name="Error"></typeparam>
/// <param name="_error"></param>
public class Err<Error>(Error _error)
{
  /// <summary>
  /// Error value.
  /// </summary>
  public Error error => _error;
}

/// <summary>
/// Helper class to generate result values.
/// </summary>
public static class Result
{
  /// <summary>
  /// Create a result with a value.
  /// </summary>
  /// <typeparam name="Value"></typeparam>
  /// <typeparam name="Error"></typeparam>
  /// <param name="value"></param>
  /// <returns></returns>
  public static Result<Value, Error> Ok<Value, Error>(Value value)
  {
    return new Result<Value, Error>(true, value, default!);
  }

  /// <summary>
  /// Create a result with a value.
  /// </summary>
  /// <typeparam name="Value"></typeparam>
  /// <param name="value"></param>
  /// <returns></returns>
  public static Ok<Value> Ok<Value>(Value value)
  {
    return new Ok<Value>(value);
  }

  /// <summary>
  /// Create a result with an error.
  /// </summary>
  /// <typeparam name="Value"></typeparam>
  /// <param name="value"></param>
  /// <returns></returns>
  public static Err<Value> Err<Value>(Value value)
  {
    return new Err<Value>(value);
  }

  /// <summary>
  /// Create a result with an error.
  /// </summary>
  /// <typeparam name="Value"></typeparam>
  /// <typeparam name="Error"></typeparam>
  /// <param name="error"></param>
  /// <returns></returns>
  public static Result<Value, Error> Err<Value, Error>(Error error)
  {
    return new Result<Value, Error>(false, default!, error);
  }

  /// <summary>
  /// Returns an okay result if value is defined. Otherwise an error.
  /// </summary>
  /// <typeparam name="Value"></typeparam>
  /// <typeparam name="Error"></typeparam>
  /// <param name="value"></param>
  /// <param name="error"></param>
  /// <returns></returns>
  public static Result<Value, Error> NotNull<Value, Error>(Value? value, Error error)
    where Value : class
  {
    if (value == null)
    {
      return Err<Value, Error>(error);
    }
    return Ok<Value, Error>(value);
  }

  /// <summary>
  /// Returns an okay result if value is defined. Otherwise return an null reference exception.
  /// </summary>
  /// <typeparam name="Value"></typeparam>
  /// <param name="value"></param>
  /// <returns></returns>
  public static Result<Value, Exception> NotNull<Value>(Value? value)
    where Value : class
  {
    if (value == null)
    {
      return Err<Value, Exception>(new NullReferenceException());
    }
    return Ok<Value, Exception>(value);
  }
}

/// <summary>
/// Represents a result that can contain either a value or an error.
/// </summary>
/// <typeparam name="Value"></typeparam>
/// <typeparam name="Error"></typeparam>
public class Result<Value, Error>
{
  bool isSuccess;
  Value? value;
  Error? error;

  /// <summary>
  /// Create a new result.
  /// </summary>
  /// <param name="isSuccess"></param>
  /// <param name="value"></param>
  /// <param name="error"></param>
  internal Result(bool isSuccess, Value value, Error error)
  {
    this.isSuccess = isSuccess;
    this.value = value;
    this.error = error;
  }

  /// <summary>
  /// Returns true if result is Ok.
  /// </summary>
  /// <returns></returns>
  public bool IsOk()
  {
    return isSuccess;
  }

  /// <summary>
  /// Returns true if result is Err.
  /// </summary>
  /// <returns></returns>
  public bool IsErr()
  {
    return !isSuccess;
  }

  /// <summary>
  /// If the result is Ok(value) run the inspect action on the value.
  /// </summary>
  /// <param name="inspect"></param>
  public Result<Value, Error> Inspect(Action<Value> inspect)
  {
    if (isSuccess)
    {
      inspect(value!);
    }
    return this;
  }

  /// <summary>
  /// If the result is Err(error) run the inspectErr action on the error;
  /// </summary>
  /// <param name="inspectErr"></param>
  public Result<Value, Error> InspectErr(Action<Error> inspectErr)
  {
    if (!isSuccess)
    {
      inspectErr(error!);
    }
    return this;
  }

  /// <summary>
  /// Return the inner value or throw the provided exception.
  /// </summary>
  /// <param name="createException"></param>
  /// <returns></returns>
  public Value Expect(Func<Exception> createException)
  {
    if (isSuccess)
    {
      return value!;
    }
    else
    {
      throw createException();
    }
  }

  /// <summary>
  /// Return the inner value or throw an exception with the message.
  /// </summary>
  /// <param name="message"></param>
  /// <returns></returns>
  public Value Expect(string message) => Expect(() => new InvalidOperationException(message));

  /// <summary>
  /// Return the inner value or return an exception.
  /// </summary>
  /// <returns></returns>
  public Value Unwrap()
  {
    return Expect(() =>
      new InvalidOperationException($"Attempted to unwrap an error result. {error}")
    );
  }

  /// <summary>
  /// Return the inner value or the provided orValue.
  /// </summary>
  /// <param name="orValue"></param>
  /// <returns></returns>
  public Value UnwrapOr(Value orValue)
  {
    if (isSuccess)
    {
      return value!;
    }
    return orValue;
  }

  /// <summary>
  /// Return the inner value or default otherwise.
  /// </summary>
  /// <returns></returns>
  public Value? UnwrapOrDefault()
  {
    return UnwrapOr(default!);
  }

  /// <summary>
  /// Return the inner value or the result of the result of elseFunc.
  /// </summary>
  /// <param name="elseFunc"></param>
  /// <returns></returns>
  public Value UnwrapOrElse(Func<Error, Value> elseFunc)
  {
    if (isSuccess)
    {
      return value!;
    }
    else
    {
      return elseFunc(error!);
    }
  }

  /// <summary>
  /// Unwrap the error result.
  /// </summary>
  /// <returns></returns>
  /// <exception cref="InvalidOperationException"></exception>
  public Error UnwrapErr()
  {
    if (!isSuccess)
    {
      return error!;
    }
    else
    {
      throw new InvalidOperationException("Attempted to unwrap an error on a successful result.");
    }
  }

  /// <summary>
  /// Convert value to option.
  /// </summary>
  /// <returns></returns>
  public Option<Value> Ok()
  {
    if (isSuccess)
    {
      return Option.Some(value!);
    }
    else
    {
      return Option.None<Value>();
    }
  }

  /// <summary>
  /// Convert error to option.
  /// </summary>
  /// <returns></returns>
  public Option<Error> Err()
  {
    if (isSuccess)
    {
      return Option.None<Error>();
    }
    else
    {
      return Option.Some(error!);
    }
  }

  /// <summary>
  /// Convert the inner value using the map function, or return Err(error)
  /// </summary>
  /// <typeparam name="V"></typeparam>
  /// <param name="map"></param>
  /// <returns></returns>
  public Result<V, Error> Map<V>(Func<Value, V> map)
  {
    if (isSuccess)
    {
      return Result.Ok<V, Error>(map(value!));
    }
    else
    {
      return Result.Err<V, Error>(error!);
    }
  }

  /// <summary>
  /// Convert the error using the mapError function, or return Ok(value)
  /// </summary>
  /// <typeparam name="E"></typeparam>
  /// <param name="mapError"></param>
  /// <returns></returns>
  public Result<Value, E> MapErr<E>(Func<Error, E> mapError)
  {
    if (isSuccess)
    {
      return Result.Ok<Value, E>(value!);
    }
    else
    {
      return Result.Err<Value, E>(mapError(error!));
    }
  }

  /// <summary>
  /// If Ok, apply the mapFunc to value and return that, or return the or value.
  /// </summary>
  /// <typeparam name="V"></typeparam>
  /// <param name="mapFunc"></param>
  /// <param name="orValue"></param>
  /// <returns></returns>
  public V MapOr<V>(Func<Value, V> mapFunc, V orValue)
  {
    if (isSuccess)
    {
      return mapFunc(value!);
    }
    return orValue;
  }

  /// <summary>
  /// If Ok(value) apply mapFunc to value and return that, otherwise return elseFunc(error)
  /// </summary>
  /// <typeparam name="V"></typeparam>
  /// <typeparam name="E"></typeparam>
  /// <param name="mapFunc"></param>
  /// <param name="elseFunc"></param>
  /// <returns></returns>
  public V MapOrElse<V, E>(Func<Value, V> mapFunc, Func<Error, V> elseFunc)
  {
    if (isSuccess)
    {
      return mapFunc(value!);
    }
    else
    {
      return elseFunc(error!);
    }
  }

  /// <summary>
  /// If the first value is Ok return the this value. Otherwise the other value.
  /// </summary>
  /// <param name="other"></param>
  /// <returns></returns>
  public Result<Value, Error> And(Result<Value, Error> other)
  {
    if (isSuccess)
    {
      return other;
    }
    else
    {
      return this;
    }
  }

  /// <summary>
  /// If the first value is Ok return the this value, otherwise the other value.
  /// </summary>
  /// <param name="other"></param>
  /// <returns></returns>
  public Result<Value, Error> Or(Result<Value, Error> other)
  {
    if (isSuccess)
    {
      return this;
    }
    else
    {
      return other;
    }
  }

  /// <summary>
  /// If the value is a success apply the then func, otherwise return the error result.
  /// </summary>
  /// <typeparam name="V"></typeparam>
  /// <param name="thenFunc"></param>
  /// <returns></returns>
  public Result<V, Error> AndThen<V>(Func<Value, Result<V, Error>> thenFunc)
  {
    if (isSuccess)
    {
      return thenFunc(value!);
    }
    return Result.Err<V, Error>(error!);
  }

  /// <summary>
  /// If the result is an error apply the else func, otherwise return the original value.
  /// </summary>
  /// <typeparam name="E"></typeparam>
  /// <param name="elseFunc"></param>
  /// <returns></returns>
  public Result<Value, E> OrElse<E>(Func<Error, Result<Value, E>> elseFunc)
  {
    if (isSuccess)
    {
      return Result.Ok<Value, E>(value!);
    }
    else
    {
      return elseFunc(error!);
    }
  }

  /// <summary>
  /// Compare results.
  /// </summary>
  /// <param name="obj"></param>
  /// <returns></returns>
  public override bool Equals(object? obj)
  {
    return obj is Result<Value, Error> result
      && isSuccess == result.isSuccess
      && EqualityComparer<Value?>.Default.Equals(value, result.value)
      && EqualityComparer<Error?>.Default.Equals(error, result.error);
  }

  /// <summary>
  /// Get result hash code.
  /// </summary>
  /// <returns></returns>
  public override int GetHashCode()
  {
    return HashCode.Combine(isSuccess, value, error);
  }

  /// <summary>
  /// Implicitly convert an okay to a result.
  /// </summary>
  /// <param name="ok"></param>
  public static implicit operator Result<Value, Error>(Ok<Value> ok)
  {
    return Result.Ok<Value, Error>(ok.value);
  }

  /// <summary>
  /// Implicitly convert an error to a result.
  /// </summary>
  /// <param name="err"></param>
  public static implicit operator Result<Value, Error>(Err<Error> err)
  {
    return Result.Err<Value, Error>(err.error);
  }

  /// <summary>
  /// Convert value to an Ok of the same type.
  /// </summary>
  /// <param name="value"></param>
  public static implicit operator Result<Value, Error>(Value value)
  {
    return Result.Ok<Value, Error>(value);
  }

  /// <summary>
  /// Convert value to an Err of the same type.
  /// </summary>
  /// <param name="error"></param>
  public static implicit operator Result<Value, Error>(Error error)
  {
    return Result.Err<Value, Error>(error);
  }
}
