namespace CriusNyx.Results.Tests;

public class ResultTests
{
  (object o, Result<object, Exception> result) Ok()
  {
    object o = new object();
    var result = Result.Ok<object, Exception>(o);
    return (o, result);
  }

  (Exception e, Result<object, Exception> result) Err()
  {
    Exception e = new Exception();
    var result = Result.Err<object, Exception>(e);
    return (e, result);
  }

  [Test]
  public void CanCreate_Ok()
  {
    var (_, result) = Ok();
    Assert.That(result.IsOk());
  }

  [Test]
  public void CanCreate_Err()
  {
    var (_, result) = Err();
    Assert.That(result.IsErr);
  }

  [Test]
  public void Expect_Ok_Works()
  {
    var (expected, result) = Ok();
    var actual = result.Expect("Shouldn't see this");
    Assert.That(actual, Is.EqualTo(expected));
  }

  [Test]
  public void Expect_Err_Works()
  {
    var result = Result.Err<object, Exception>(new Exception());
    Assert.Throws<InvalidOperationException>(() => result.Expect("Should Throw"));
  }

  [Test]
  public void Unwrap_Ok_Works()
  {
    var (expected, result) = Ok();
    var actual = result.Unwrap();
    Assert.That(actual, Is.EqualTo(expected));
  }

  [Test]
  public void Unwrap_Err_Works()
  {
    var expected = new Exception();
    var result = Result.Err<object, Exception>(expected);
    Assert.Throws<InvalidOperationException>(() => result.Unwrap());
  }

  [Test]
  public void UnwrapOr_Ok_Works()
  {
    var (expected, result) = Ok();
    var actual = result.UnwrapOr(null!);
    Assert.That(actual, Is.EqualTo(expected));
  }

  [Test]
  public void UnwrapOr_Err_Works()
  {
    var (_, result) = Err();
    var expected = new object();
    var actual = result.UnwrapOr(expected);
    Assert.That(actual, Is.EqualTo(expected));
  }

  [Test]
  public void UnwrapOrDefault_Ok_Works()
  {
    var (expected, result) = Ok();
    var actual = result.UnwrapOrDefault();
    Assert.That(actual, Is.EqualTo(expected));
  }

  [Test]
  public void UnwrapOrDefault_Err_Works()
  {
    var (_, result) = Err();
    var actual = result.UnwrapOrDefault();
    Assert.That(actual, Is.EqualTo(null));
  }

  [Test]
  public void UnwrapOrElse_Ok_Works()
  {
    var (expected, result) = Ok();
    var actual = result.UnwrapOrElse((e) => new object());
    Assert.That(actual, Is.EqualTo(expected));
  }

  [Test]
  public void UnwrapOrElse_Err_Works()
  {
    var (_, result) = Err();
    var expected = new object();
    var actual = result.UnwrapOrElse((e) => expected);
    Assert.That(actual, Is.EqualTo(expected));
  }

  [Test]
  public void OkMethod_Ok_Works()
  {
    var (inner, result) = Ok();
    var actual = result.Ok();
    var expected = Option.Some(inner);
    Assert.That(actual, Is.EqualTo(expected));
  }

  [Test]
  public void OkMethod_Err_Works()
  {
    var (_, result) = Err();
    var actual = result.Ok();
    var expected = Option.None<object>();
    Assert.That(actual, Is.EqualTo(expected));
  }

  [Test]
  public void ErrMethod_Ok_Works()
  {
    var (_, result) = Ok();
    var actual = result.Err();
    var expected = Option.None<Exception>();
    Assert.That(actual, Is.EqualTo(expected));
  }

  [Test]
  public void ErrMethod_Err_Works()
  {
    var (inner, result) = Err();
    var actual = result.Err();
    var expected = Option.Some(inner);
    Assert.That(actual, Is.EqualTo(expected));
  }

  [Test]
  public void Map_Ok_Works()
  {
    var (_, result) = Ok();
    var expected = new object();
    var actual = result.Map((_) => expected);
    Assert.That(actual, Is.EqualTo(Result.Ok<object, Exception>(expected)));
  }

  [Test]
  public void Map_Err_Works()
  {
    var (expected, result) = Err();

    var actual = result.Map((_) => new object());
    Assert.That(actual, Is.EqualTo(Result.Err<object, Exception>(expected)));
  }

  [Test]
  public void MapErr_Ok_Works()
  {
    var (expected, result) = Ok();
    var actual = result.MapErr((_) => new Exception());
    Assert.That(actual, Is.EqualTo(Result.Ok<object, Exception>(expected)));
  }

  [Test]
  public void MapErr_Err_Works()
  {
    var (_, result) = Err();
    var expected = new Exception();
    var actual = result.MapErr((_) => expected);
    Assert.That(actual, Is.EqualTo(Result.Err<object, Exception>(expected)));
  }

  [Test]
  public void MapOr_Ok_Works()
  {
    var (_, result) = Ok();
    var expected = new object();
    var actual = result.MapOr((x) => expected, default);
    Assert.That(actual, Is.EqualTo(expected));
  }

  [Test]
  public void MapOr_Err_Works()
  {
    var (_, result) = Err();
    var expected = new object();
    var actual = result.MapOr((x) => new object(), expected);
    Assert.That(actual, Is.EqualTo(expected));
  }

  [Test]
  public void MapOrElse_Ok_Works()
  {
    var (_, result) = Ok();
    var expected = new object();
    var actual = result.MapOrElse<object, Exception>((_) => expected, (_) => null!);
    Assert.That(actual, Is.EqualTo(expected));
  }

  [Test]
  public void MapOrElse_Err_Works()
  {
    var (_, result) = Err();
    var expected = new Exception();
    var actual = result.MapOrElse<object, Exception>((_) => null!, (_) => expected);
    Assert.That(actual, Is.EqualTo(expected));
  }

  [Test]
  public void And_Ok_Ok_Works()
  {
    var (_, a) = Ok();
    var (_, b) = Ok();
    var actual = a.And(b);
    Assert.That(actual, Is.EqualTo(b));
  }

  [Test]
  public void And_Ok_Err_Works()
  {
    var (_, a) = Ok();
    var (_, b) = Err();
    var actual = a.And(b);
    Assert.That(actual, Is.EqualTo(b));
  }

  [Test]
  public void And_Err_Ok_Works()
  {
    var (_, a) = Err();
    var (_, b) = Ok();
    var actual = a.And(b);
    Assert.That(actual, Is.EqualTo(a));
  }

  [Test]
  public void And_Err_Err_Works()
  {
    var (_, a) = Err();
    var (_, b) = Err();
    var actual = a.And(b);
    Assert.That(actual, Is.EqualTo(a));
  }

  [Test]
  public void Or_Ok_Ok_Works()
  {
    var (_, a) = Ok();
    var (_, b) = Ok();
    var actual = a.Or(b);
    Assert.That(actual, Is.EqualTo(a));
  }

  [Test]
  public void Or_Ok_Err_Works()
  {
    var (_, a) = Ok();
    var (_, b) = Err();
    var actual = a.Or(b);
    Assert.That(actual, Is.EqualTo(a));
  }

  [Test]
  public void Or_Err_Ok_Works()
  {
    var (_, a) = Err();
    var (_, b) = Ok();
    var actual = a.Or(b);
    Assert.That(actual, Is.EqualTo(b));
  }

  [Test]
  public void Or_Err_Err_Works()
  {
    var (_, a) = Err();
    var (_, b) = Err();
    var actual = a.Or(b);
    Assert.That(actual, Is.EqualTo(b));
  }

  [Test]
  public void AndThen_Ok_ToOk_Works()
  {
    var (_, result) = Ok();
    var expected = new object();
    var actual = result.AndThen((_) => Result.Ok<object, Exception>(expected));
    Assert.That(actual, Is.EqualTo(Result.Ok<object, Exception>(expected)));
  }

  [Test]
  public void AndThen_Ok_ToErr_Works()
  {
    var (_, result) = Ok();
    var expected = new Exception();
    var actual = result.AndThen((_) => Result.Err<object, Exception>(expected));
    Assert.That(actual, Is.EqualTo(Result.Err<object, Exception>(expected)));
  }

  [Test]
  public void AndThen_Err_Works()
  {
    var (expected, result) = Err();
    var actual = result.AndThen((_) => Result.Ok<object, Exception>(expected));
    Assert.That(actual, Is.EqualTo(Result.Err<object, Exception>(expected)));
  }

  [Test]
  public void OrElse_Err_ToOk_Works()
  {
    var (_, result) = Err();
    var expected = new object();
    var actual = result.OrElse((_) => Result.Ok<object, Exception>(expected));
    Assert.That(actual, Is.EqualTo(Result.Ok<object, Exception>(expected)));
  }

  [Test]
  public void OrElse_Err_ToErr_Works()
  {
    var (_, result) = Err();
    var expected = new Exception();
    var actual = result.OrElse((_) => Result.Err<object, Exception>(expected));
    Assert.That(actual, Is.EqualTo(Result.Err<object, Exception>(expected)));
  }
}
