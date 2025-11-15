using System.Runtime.CompilerServices;

namespace CriusNyx.Results;

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

  public Result(bool isSuccess, Value value, Error error)
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
  public void Inspect(Action<Value> inspect)
  {
    if (isSuccess)
    {
      inspect(value!);
    }
  }

  /// <summary>
  /// If the result is Err(error) run the inspectErr action on the error;
  /// </summary>
  /// <param name="inspectErr"></param>
  public void InspectErr(Action<Error> inspectErr)
  {
    if (!isSuccess)
    {
      inspectErr(error!);
    }
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

  public override bool Equals(object? obj)
  {
    return obj is Result<Value, Error> result
      && isSuccess == result.isSuccess
      && EqualityComparer<Value?>.Default.Equals(value, result.value)
      && EqualityComparer<Error?>.Default.Equals(error, result.error);
  }

  public override int GetHashCode()
  {
    return HashCode.Combine(isSuccess, value, error);
  }

  public static implicit operator Result<Value, Error>(Value value)
  {
    return Result.Ok<Value, Error>(value);
  }

  public static implicit operator Result<Value, Error>(Error error)
  {
    return Result.Err<Value, Error>(error);
  }
}
