using System.Runtime;
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
}
