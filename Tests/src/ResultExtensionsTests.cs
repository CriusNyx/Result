using System.Collections.Frozen;
using System.Security.Cryptography.X509Certificates;
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

  [Test]
  public void AsOk_Works()
  {
    Result<string, Exception> result = "Hello".AsOk();
    Assert.That(result.Unwrap(), Is.EqualTo("Hello"));
  }

  [Test]
  public void AsOk_WithErrorType_Works()
  {
    Result<string, Exception> result = "Hello".AsOk<string, Exception>();
    Assert.That(result.Unwrap(), Is.EqualTo("Hello"));
  }

  [Test]
  public void AsErr_Works()
  {
    Exception exception = new NotImplementedException();
    Result<string, Exception> result = exception.AsErr();
    Assert.That(result.UnwrapErr(), Is.EqualTo(exception));
  }

  [Test]
  public void AsErr_WithValueType_Works()
  {
    Exception exception = new NotImplementedException();
    Result<string, Exception> result = exception.AsErr<string, Exception>();
    Assert.That(result.UnwrapErr(), Is.EqualTo(exception));
  }

  [Test]
  public void Collect_Ok_Works()
  {
    string[] values = ["Hello", "World"];
    Result<string, Exception>[] results = values.Select(x => x.AsOk<string, Exception>()).ToArray();
    var result = results.Collect();
    Assert.That(result.IsOk());
    Assert.That(result.Unwrap(), Is.EquivalentTo(new string[] { "Hello", "World" }));
  }

  [Test]
  public void Collect_Exception_Works()
  {
    var exception = new NotImplementedException();
    Result<string, Exception>[] results =
    [
      Result.Ok("Hello"),
      Result.Err<string, Exception>(exception),
    ];
    var result = results.Collect();
    Assert.That(result.IsErr());
    Assert.That(result.UnwrapErr(), Is.EquivalentTo(new Exception[] { exception }));
  }
}
