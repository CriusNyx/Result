namespace CriusNyx.Results.Extensions;

public static class OptionExtensions
{
  public static Option<T> AsOption<T>(this T? value)
  {
    if (value is T t)
    {
      return Option.Some(t);
    }
    return Option.None<T>();
  }
}
