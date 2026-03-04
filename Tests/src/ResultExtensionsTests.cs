using CriusNyx.Results.Extensions;

namespace CriusNyx.Results.Tests;

public class ResultExtensionsTests
{
  [Test]
  public void UnwrapOrNull_Ok_Works()
  {
    Result<int, Exception> result = Result.Ok(1);
    Assert.That(result.UnwrapOrNull(), Is.EqualTo(1));
  }

  [Test]
  public void UnwrapOrNull_Err_Works()
  {
    Result<int, Exception> result = Result.Err(new Exception());
    Assert.That(result.UnwrapOrNull(), Is.Null);
  }
}
