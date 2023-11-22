using System.Numerics;

Func<Complex, Complex> round = c => ComplexRound(c, 8);

var input1 = new double[] { 1, 1, 1, 1 };
var f1i = Fourier.SlowStraight(input1);
var f1o = Fourier.SlowReverse(f1i);
var f2i = Fourier.FastStraight(input1);
var f2o = Fourier.ReverseUsingStraight(f2i);

WriteArray(input1,            "Input");
WriteArray(f1i.Select(round), "SlowStraight");
WriteArray(f1o.Select(round), "SlowReversed");
WriteArray(f2i.Select(round), "FastStraight");
WriteArray(f2o.Select(round), "FastReversed");

void WriteArray<T> (IEnumerable<T> source, string name) {
  Console.WriteLine($"{name}: [{string.Join(", ", source)}].");
}

Complex ComplexRound (Complex source, int count) {
  return new(double.Round(source.Real, count), double.Round(source.Imaginary, count));
}

public static class Fourier {

  public static Complex[] SlowStraight (double[] values) {
    return SlowStraight(values.Select(x => new Complex(x, 0d)).ToArray());
  }

  public static Complex[] SlowStraight (Complex[] values) {
    var result = new Complex[values.Length];

    for (int i = 0; i < values.Length; i++) {
      var coeff = double.Pi * i * 2 / values.Length;
      var sum = new Complex();

      for (int j = 0; j < values.Length; j++) {
        sum += new Complex(double.Cos(coeff * j), -double.Sin(coeff * j)) * values[j];
      }

      result[i] = sum;
    }

    return result;
  }

  public static Complex[] SlowReverse (Complex[] values) {
    var result = new Complex[values.Length];

    for (int i = 0; i < values.Length; i++) {
      var sum = new Complex();
      var coeff = double.Pi * i * 2 / values.Length;

      for (int j = 0; j < values.Length; j++) {
        sum += values[j] * new Complex(double.Cos(coeff * j), double.Sin(coeff * j));
      }

      result[i] = sum / values.Length;
    }

    return result;
  }

  public static Complex[] FastStraightArray (Complex[] values) {
    return FastStraight(values).ToArray();
  }

  public static IEnumerable<Complex> FastStraight (IEnumerable<double> values) {
    return FastStraight(values.Select(x => new Complex(x, 0)));
  }

  public static IEnumerable<Complex> FastStraight (IEnumerable<Complex> values) {
    var count = values.Count();

    if (count <= 1) {
      return values;
    }

    (var evens, var odds) = EvensOddsByIdxs(values);

    evens = FastStraight(evens);
    odds = FastStraight(odds);

    var coeff = new Complex(0, -2 * double.Pi / count);
    var factor = Enumerable
      .Range(0, count / 2)
      .Zip(odds, (k, o) => Complex.Exp(coeff * k) * o);

    return 
      Enumerable.Concat(
        evens.Zip(factor, (a, b) => a + b),
        evens.Zip(factor, (a, b) => a - b));
  }

  public static IEnumerable<Complex> ReverseUsingStraight (IEnumerable<Complex> values) {
    var count = values.Count();
    return FastStraight(values.Select(Complex.Conjugate)).Select(x => Complex.Conjugate(x) / count);
  }

  public static IEnumerable<Complex> NormalStraight (IEnumerable<Complex> values) {
    return null!;
  }

  private static (IEnumerable<T> Evens, IEnumerable<T> Odds) EvensOddsByIdxs<T> (IEnumerable<T> source) {
    bool selector = true;
    List<T> evens = new();
    List<T> odds = new();

    foreach (T elem in source) {
      (selector ? evens : odds).Add(elem);
      selector = !selector;
    }

    return (evens, odds);
  }
}

public static class Convolutions {

  public static Complex[] Linear (double[] input1, double[] input2) {
    return Linear(
      input1.Select(x => new Complex(x, 0d)).ToArray(), 
      input2.Select(x => new Complex(x, 0d)).ToArray());
  }

  public static Complex[] Linear (Complex[] input1, Complex[] input2) {
    var result = new Complex[input1.Length + input2.Length - 1];

    for (int i = 0; i < input1.Length; i++) {
      for (int j = 0; j < input2.Length; j++) {
        result[i + j] += input1[i] * input2[j];
      }
    }

    return result;
  }

  public static Complex[] Fourier (
    double[] a, double[] b, 
    Func<double[], Complex[]> straight, 
    Func<Complex[], Complex[]> reversed)
  {
    int n = a.Length + b.Length - 1;
    Array.Resize(ref a, n);
    Array.Resize(ref b, n);
    return reversed(Enumerable.Zip(straight(a), straight(b), (x, y) => x * y).ToArray());
  }
}
