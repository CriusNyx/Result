namespace CriusNyx.Results.Extensions;

/// <summary>
/// Extensions for result.
/// </summary>
public static class ResultExtensions
{
  /// <summary>
  /// Unwrap the result or return null if the result is err.
  /// </summary>
  /// <typeparam name="Value"></typeparam>
  /// <typeparam name="Exception"></typeparam>
  /// <param name="result"></param>
  /// <returns></returns>
  public static Value? UnwrapOrNull<Value, Exception>(this Result<Value, Exception> result)
    where Value : struct
  {
    if (result.IsOk())
    {
      return result.Unwrap();
    }
    return null;
  }

  /// <summary>
  /// Convert value to an ok result.
  /// </summary>
  /// <typeparam name="Value"></typeparam>
  /// <param name="value"></param>
  /// <returns></returns>
  public static Ok<Value> AsOk<Value>(this Value value)
  {
    return Result.Ok(value);
  }

  /// <summary>
  /// Convert value to an ok result.
  /// </summary>
  /// <typeparam name="Value"></typeparam>
  /// <typeparam name="Error"></typeparam>
  /// <param name="value"></param>
  /// <returns></returns>
  public static Result<Value, Error> AsOk<Value, Error>(this Value value)
  {
    return Result.Ok<Value, Error>(value);
  }

  /// <summary>
  /// Convert value to an error result.
  /// </summary>
  /// <typeparam name="Error"></typeparam>
  /// <param name="error"></param>
  /// <returns></returns>
  public static Err<Error> AsErr<Error>(this Error error)
  {
    return Result.Err(error);
  }

  /// <summary>
  /// Convert value to an error result.
  /// </summary>
  /// <typeparam name="Value"></typeparam>
  /// <typeparam name="Error"></typeparam>
  /// <param name="error"></param>
  /// <returns></returns>
  public static Result<Value, Error> AsErr<Value, Error>(this Error error)
  {
    return Result.Err<Value, Error>(error);
  }

  /// <summary>
  /// If any result is an error, return Err&lt;IEnumerable&lt;Error&gt;&gt;.
  /// If all results are okay then return Ok&lt;IEnumerable&lt;Value&gt;&gt;
  /// </summary>
  /// <typeparam name="Value"></typeparam>
  /// <typeparam name="Error"></typeparam>
  /// <param name="results"></param>
  /// <returns></returns>
  public static Result<IEnumerable<Value>, IEnumerable<Error>> Collect<Value, Error>(
    this IEnumerable<Result<Value, Error>> results
  )
  {
    if (results.Any(x => x.IsErr()))
    {
      return results.Where(x => x.IsErr()).Select(x => x.UnwrapErr()).AsErr();
    }
    return results.Select(x => x.Unwrap()).AsOk();
  }
}
