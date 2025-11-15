using Microsoft.VisualBasic;

namespace CriusNyx.Results.Tests;

public class ObjectTests
{
  public (object o, Option<object> option) Some()
  {
    var o = new object();
    var option = Option.Some(o);
    return (o, option);
  }

  public Option<object> None()
  {
    return Option.None<object>();
  }

  [Test]
  public void CanCreate_Some()
  {
    var (_, option) = Some();
    Assert.That(option.IsSome());
  }

  [Test]
  public void CanCreate_None()
  {
    var option = None();
    Assert.That(option.IsNone());
  }

  [Test]
  public void IsSomeAnd_True_Works()
  {
    var (_, option) = Some();
    var actual = option.IsSomeAnd((_) => true);
    Assert.That(actual);
  }

  [Test]
  public void IsSomeAnd_False_Works()
  {
    var (_, option) = Some();
    var actual = option.IsSomeAnd((_) => false);
    Assert.That(!actual);
  }

  [Test]
  public void IsSomeAnd_None_Works()
  {
    var option = None();
    var actual = option.IsSomeAnd((_) => true);
    Assert.That(!actual);
  }

  [Test]
  public void IsNoneOr_True_Works()
  {
    var (_, option) = Some();
    var actual = option.IsNoneOr((_) => true);
    Assert.That(actual);
  }

  [Test]
  public void IsNoneOr_False_Works()
  {
    var (_, option) = Some();
    var actual = option.IsNoneOr((_) => false);
    Assert.That(!actual);
  }

  [Test]
  public void IsNoneOr_None_Works()
  {
    var option = None();
    var actual = option.IsNoneOr((_) => false);
    Assert.That(actual);
  }

  [Test]
  public void Inspect_Some_Works()
  {
    var (expected, option) = Some();
    object? actual = null;
    option.Inspect((value) => actual = value);
    Assert.That(actual, Is.EqualTo(expected));
  }

  [Test]
  public void Inspect_None_Works()
  {
    var option = None();
    object? actual = null;
    option.Inspect((value) => actual = value);
    Assert.That(actual, Is.EqualTo(null));
  }

  [Test]
  public void Expect_Some_Works()
  {
    var (expected, option) = Some();
    var actual = option.Expect("Shouldn't throw");
    Assert.That(actual, Is.EqualTo(expected));
  }

  [Test]
  public void Expect_None_Works()
  {
    var option = None();
    Assert.Throws<InvalidOperationException>(() => option.Expect("Should Throw"));
  }

  [Test]
  public void Unwrap_Some_Works()
  {
    var (expected, option) = Some();
    var actual = option.Unwrap();
    Assert.That(actual, Is.EqualTo(expected));
  }

  [Test]
  public void Unwrap_None_Works()
  {
    var option = None();
    Assert.Throws<InvalidOperationException>(() => option.Unwrap());
  }

  [Test]
  public void UnwrapOr_Some_Works()
  {
    var (expected, option) = Some();
    var actual = option.UnwrapOr(new object());
    Assert.That(actual, Is.EqualTo(expected));
  }

  [Test]
  public void UnwrapOr_None_Works()
  {
    var option = None();
    var expected = new object();
    var actual = option.UnwrapOr(expected);
    Assert.That(actual, Is.EqualTo(expected));
  }

  [Test]
  public void UnwrapOrDefault_Some_Works()
  {
    var (expected, option) = Some();
    var actual = option.UnwrapOrDefault();
    Assert.That(actual, Is.EqualTo(expected));
  }

  [Test]
  public void UnwrapOrDefault_None_Works()
  {
    var option = None();

    var actual = option.UnwrapOrDefault();
    Assert.That(actual, Is.EqualTo(null));
  }

  [Test]
  public void UnwrapOrElse_Some_Works()
  {
    var (expected, option) = Some();

    var actual = option.UnwrapOrElse(() => new object());
    Assert.That(actual, Is.EqualTo(expected));
  }

  [Test]
  public void UnwrapOrElse_None_Works()
  {
    var option = None();
    var expected = new object();
    var actual = option.UnwrapOrElse(() => expected);
    Assert.That(actual, Is.EqualTo(expected));
  }

  [Test]
  public void OkOr_Some_Works()
  {
    var (expected, option) = Some();
    var actual = option.OkOr(new Exception());
    Assert.That(actual, Is.EqualTo(Result.Ok<object, Exception>(expected)));
  }

  [Test]
  public void OkOr_None_Works()
  {
    var option = None();
    var expected = new Exception();
    var actual = option.OkOr(expected);
    Assert.That(actual, Is.EqualTo(Result.Err<object, Exception>(expected)));
  }

  [Test]
  public void OkOrElse_Some_Works()
  {
    var (expected, option) = Some();
    var actual = option.OkOrElse(() => new Exception());
    Assert.That(actual, Is.EqualTo(Result.Ok<object, Exception>(expected)));
  }

  [Test]
  public void OkOrElse_None_Works()
  {
    var option = None();
    var expected = new Exception();
    var actual = option.OkOrElse(() => expected);
    Assert.That(actual, Is.EqualTo(Result.Err<object, Exception>(expected)));
  }

  [Test]
  public void Map_Some_Works()
  {
    var (_, option) = Some();
    var expected = new object();
    var actual = option.Map((_) => expected);
    Assert.That(actual, Is.EqualTo(Option.Some(expected)));
  }

  [Test]
  public void Map_None_Works()
  {
    var option = None();
    var actual = option.Map((_) => new object());
    Assert.That(actual, Is.EqualTo(Option.None<object>()));
  }

  [Test]
  public void MapOr_Some_Works()
  {
    var (_, option) = Some();
    var expected = new object();
    var actual = option.MapOr((_) => expected, new object());
    Assert.That(actual, Is.EqualTo(Option.Some(expected)));
  }

  [Test]
  public void MapOr_None_Works()
  {
    var option = None();
    var expected = new object();
    var actual = option.MapOr((_) => new object(), expected);
    Assert.That(actual, Is.EqualTo(Option.Some(expected)));
  }

  [Test]
  public void MapOrElse_Some_Works()
  {
    var (_, option) = Some();
    var expected = new object();
    var actual = option.MapOrElse((_) => expected, () => new object());
    Assert.That(actual, Is.EqualTo(Option.Some(expected)));
  }

  [Test]
  public void MapOrElse_None_Works()
  {
    var option = None();
    var expected = new object();
    var actual = option.MapOrElse((_) => new object(), () => expected);
    Assert.That(actual, Is.EqualTo(Option.Some(expected)));
  }

  [Test]
  public void And_Some_Some_Works()
  {
    var (_, a) = Some();
    var (_, b) = Some();
    var actual = a.And(b);
    Assert.That(actual, Is.EqualTo(b));
  }

  [Test]
  public void And_Some_None_Works()
  {
    var (_, a) = Some();
    var b = None();
    var actual = a.And(b);
    Assert.That(actual, Is.EqualTo(b));
  }

  [Test]
  public void And_None_Some_Works()
  {
    var a = None();
    var (_, b) = Some();
    var actual = a.And(b);
    Assert.That(actual, Is.EqualTo(a));
  }

  [Test]
  public void And_None_None_Works()
  {
    var a = None();
    var b = None();
    var actual = a.And(b);
    Assert.That(actual, Is.EqualTo(a));
  }

  [Test]
  public void Or_Some_Some_Works()
  {
    var (_, a) = Some();
    var (_, b) = Some();
    var actual = a.Or(b);
    Assert.That(actual, Is.EqualTo(a));
  }

  [Test]
  public void Or_Some_None_Works()
  {
    var (_, a) = Some();
    var b = None();
    var actual = a.Or(b);
    Assert.That(actual, Is.EqualTo(a));
  }

  [Test]
  public void Or_None_Some_Works()
  {
    var a = None();
    var (_, b) = Some();
    var actual = a.Or(b);
    Assert.That(actual, Is.EqualTo(b));
  }

  [Test]
  public void Or_None_None_Works()
  {
    var a = None();
    var b = None();
    var actual = a.Or(b);
    Assert.That(actual, Is.EqualTo(b));
  }

  [Test]
  public void Xor_Some_Some_Works()
  {
    var (_, a) = Some();
    var (_, b) = Some();
    var actual = a.Xor(b);
    Assert.That(actual, Is.EqualTo(Option.None<object>()));
  }

  [Test]
  public void Xor_Some_None_Works()
  {
    var (_, a) = Some();
    var b = None();
    var actual = a.Xor(b);
    Assert.That(actual, Is.EqualTo(a));
  }

  [Test]
  public void Xor_None_Some_Works()
  {
    var a = None();
    var (_, b) = Some();
    var actual = a.Xor(b);
    Assert.That(actual, Is.EqualTo(b));
  }

  [Test]
  public void Xor_None_None_Works()
  {
    var a = None();
    var b = None();
    var actual = a.Xor(b);
    Assert.That(actual, Is.EqualTo(Option.None<object>()));
  }
}
