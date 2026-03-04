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
}
