namespace CriusNyx.Results.Extensions;

/// <summary>
/// Extensions for the option type.
/// </summary>
public static class OptionExtensions
{
  /// <summary>
  /// Returns Some(value) if value is defined. Otherwise None
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="value"></param>
  /// <returns></returns>
  public static Option<T> AsOption<T>(this T? value)
  {
    if (value is T t)
    {
      return Option.Some(t);
    }
    return Option.None<T>();
  }

  /// <summary>
  /// Return all options that are some.
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="options"></param>
  /// <returns></returns>
  public static IEnumerable<T> WhereSome<T>(this IEnumerable<Option<T>> options)
  {
    return options.Where(x => x.IsSome()).Select(x => x.Unwrap());
  }
}
