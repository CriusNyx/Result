using CriusNyx.Results.Extensions;

namespace CriusNyx.Results.Tests;

public class OptionExtensionsTests
{
  [Test]
  public void AsOption_Some_Works()
  {
    var expected = new object();
    var option = expected.AsOption();
    Assert.That(option, Is.EqualTo(Option.Some(expected)));
  }

  [Test]
  public void AsOption_None_Works()
  {
    object expected = null!;
    var option = expected.AsOption();
    Assert.That(option, Is.EqualTo(Option.None<object>()));
  }

  [Test]
  public void WhereSome_Works()
  {
    Option<string>[] values = [Option.Some("hello"), Option.None<string>()];
    var someValues = values.WhereSome();
    Assert.That(someValues, Is.EquivalentTo(new string[] { "hello" }));
  }

  [Test]
  public void UnwrapOrNull_Value_Works()
  {
    Option<int> opt = Option.Some(1);
    Assert.That(opt.UnwrapOrNull(), Is.EqualTo(1));
  }

  [Test]
  public void UnwrapOrNull_Null_Works()
  {
    Option<int> opt = Option.None();
    Assert.That(opt.UnwrapOrNull(), Is.Null);
  }
}
