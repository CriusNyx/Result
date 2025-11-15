using System.Security.Cryptography.X509Certificates;

namespace CriusNyx.Results;

public static class Option
{
  /// <summary>
  /// An option with a value.
  /// </summary>
  /// <typeparam name="Value"></typeparam>
  /// <param name="value"></param>
  /// <returns></returns>
  public static Option<Value> Some<Value>(Value value)
  {
    return new Option<Value>(true, value);
  }

  /// <summary>
  /// An option without a value.
  /// </summary>
  /// <typeparam name="Value"></typeparam>
  /// <returns></returns>
  public static Option<Value> None<Value>()
  {
    return new Option<Value>(false, default);
  }
}

/// <summary>
/// Represents a type that might contain a value.
/// </summary>
/// <typeparam name="Value"></typeparam>
public class Option<Value>
{
  bool hasValue;
  Value? value;

  /// <summary>
  /// Don't use. Use Option.Some or Option.None
  /// </summary>
  /// <param name="hasValue"></param>
  /// <param name="value"></param>
  public Option(bool hasValue, Value? value)
  {
    this.hasValue = hasValue;
    this.value = value;
  }

  /// <summary>
  /// True if the option contains a value.
  /// </summary>
  /// <returns></returns>
  public bool IsSome()
  {
    return hasValue;
  }

  /// <summary>
  /// True if the option doesn't container a value.
  /// </summary>
  /// <returns></returns>
  public bool IsNone()
  {
    return !hasValue;
  }

  /// <summary>
  /// If the option is Some return the func. Otherwise return false.
  /// </summary>
  /// <param name="andFunc"></param>
  /// <returns></returns>
  public bool IsSomeAnd(Func<Value, bool> andFunc)
  {
    if (hasValue)
    {
      return andFunc(value!);
    }
    else
    {
      return false;
    }
  }

  /// <summary>
  /// If the option is Some return the result of orFunc. Otherwise return true.
  /// </summary>
  /// <param name="orFunc"></param>
  /// <returns></returns>
  public bool IsNoneOr(Func<Value, bool> orFunc)
  {
    if (hasValue)
    {
      return orFunc(value!);
    }
    {
      return true;
    }
  }

  /// <summary>
  /// If the Option is Some(value), run the inspect action on the value
  /// </summary>
  /// <param name="inspect"></param>
  public void Inspect(Action<Value> inspect)
  {
    if (hasValue)
    {
      inspect(value!);
    }
  }

  /// <summary>
  /// Return the value of the option, or throw the specified exception if it fails.
  /// </summary>
  /// <param name="exceptionFunc"></param>
  /// <returns></returns>
  public Value Expect(Func<Exception> exceptionFunc)
  {
    if (hasValue)
    {
      return value!;
    }
    throw exceptionFunc();
  }

  /// <summary>
  /// Return the value of the option or throw an exception with the specified message if it fails.
  /// </summary>
  /// <param name="message"></param>
  /// <returns></returns>
  public Value Expect(string message) => Expect(() => new InvalidOperationException(message));

  /// <summary>
  /// Return the value of the option or throw an exception.
  /// </summary>
  /// <returns></returns>
  public Value Unwrap() => Expect("Attempted to unwrap an option but it is none");

  /// <summary>
  /// Return the value of the option, or the orValue if it is None.
  /// </summary>
  /// <param name="orValue"></param>
  /// <returns></returns>
  public Value UnwrapOr(Value orValue)
  {
    if (hasValue)
    {
      return value!;
    }
    return orValue;
  }

  /// <summary>
  /// Return the value of the option, or default if the option is None.
  /// </summary>
  /// <returns></returns>
  public Value? UnwrapOrDefault()
  {
    if (hasValue)
    {
      return value;
    }
    return default;
  }

  /// <summary>
  /// Return the value of teh option, or the result of the elseFunc if the option is None.
  /// </summary>
  /// <param name="elseFunc"></param>
  /// <returns></returns>
  public Value UnwrapOrElse(Func<Value> elseFunc)
  {
    if (hasValue)
    {
      return value!;
    }
    else
    {
      return elseFunc();
    }
  }

  /// <summary>
  /// Return Ok(value) if the option is Some, or Err(error) if the option is None.
  /// </summary>
  /// <typeparam name="Error"></typeparam>
  /// <param name="error"></param>
  /// <returns></returns>
  public Result<Value, Error> OkOr<Error>(Error error)
  {
    if (hasValue)
    {
      return Result.Ok<Value, Error>(value!);
    }
    else
    {
      return Result.Err<Value, Error>(error);
    }
  }

  /// <summary>
  /// Return Ok(value) if the option is Some or Err(errorFunc) if the option is None.
  /// </summary>
  /// <typeparam name="Error"></typeparam>
  /// <param name="errorFunc"></param>
  /// <returns></returns>
  public Result<Value, Error> OkOrElse<Error>(Func<Error> errorFunc)
  {
    if (hasValue)
    {
      return Result.Ok<Value, Error>(value!);
    }
    else
    {
      return Result.Err<Value, Error>(errorFunc());
    }
  }

  /// <summary>
  /// If the option is Some apply the mapFunc to value and return that. Otherwise return None.
  /// </summary>
  /// <typeparam name="V"></typeparam>
  /// <param name="mapFunc"></param>
  /// <returns></returns>
  public Option<V> Map<V>(Func<Value, V> mapFunc)
  {
    if (hasValue)
    {
      return Option.Some(mapFunc(value!));
    }
    else
    {
      return Option.None<V>();
    }
  }

  /// <summary>
  /// If the option is Some apply the mapFunc. Otherwise return the orValue.
  /// </summary>
  /// <typeparam name="V"></typeparam>
  /// <param name="mapFunc"></param>
  /// <param name="orValue"></param>
  /// <returns></returns>
  public Option<V> MapOr<V>(Func<Value, V> mapFunc, V orValue)
  {
    if (hasValue)
    {
      return Option.Some(mapFunc(value!));
    }
    else
    {
      return Option.Some(orValue);
    }
  }

  /// <summary>
  /// If the option is Some return the result of the mapFunc, or return a new option with the result of the orFunc.
  /// </summary>
  /// <typeparam name="V"></typeparam>
  /// <param name="mapFunc"></param>
  /// <param name="orFunc"></param>
  /// <returns></returns>
  public Option<V> MapOrElse<V>(Func<Value, V> mapFunc, Func<V> orFunc)
  {
    if (hasValue)
    {
      return Option.Some(mapFunc(value!));
    }
    else
    {
      return Option.Some(orFunc());
    }
  }

  /// <summary>
  /// If the first option is Some return the second option. Otherwise return None.
  /// </summary>
  /// <param name="other"></param>
  /// <returns></returns>
  public Option<Value> And(Option<Value> other)
  {
    if (hasValue)
    {
      return other;
    }
    else
    {
      return this;
    }
  }

  /// <summary>
  /// IF the first option is Some return the first option. Otherwise return the second option.
  /// </summary>
  /// <param name="other"></param>
  /// <returns></returns>
  public Option<Value> Or(Option<Value> other)
  {
    if (hasValue)
    {
      return this;
    }
    else
    {
      return other;
    }
  }

  /// <summary>
  /// If only one option is Some return that option. Otherwise return no option.
  /// </summary>
  /// <param name="other"></param>
  /// <returns></returns>
  public Option<Value> Xor(Option<Value> other)
  {
    if (this is { hasValue: true } && other is { hasValue: false })
    {
      return this;
    }
    else if (this is { hasValue: false } && other is { hasValue: true })
    {
      return other;
    }
    return Option.None<Value>();
  }

  public override bool Equals(object? obj)
  {
    return obj is Option<Value> option
      && hasValue == option.hasValue
      && EqualityComparer<Value?>.Default.Equals(value, option.value);
  }

  public override int GetHashCode()
  {
    return HashCode.Combine(hasValue, value);
  }
}
