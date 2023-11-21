using System.Numerics;

var input1 = new double[] { 1, 1 };
var input2 = new double[] { 1, 1 };
var output1 = Convolutions.Fourier(input1, input2, Fourier.Straight1, Fourier.Reverse1);
var output2 = Convolutions.Linear(input1, input2);

WriteArray(input1, nameof(input1));
WriteArray(input2, nameof(input2));
WriteArray(output1.Select(ComplexRound), nameof(output1));
WriteArray(output2, nameof(output2));

void WriteArray<T> (IEnumerable<T> source, string name) {
  Console.WriteLine($"{name}: [{string.Join(", ", source)}].");
}

Complex ComplexRound (Complex source) {
  return new(double.Round(source.Real, 8), double.Round(source.Imaginary, 8));
}

public static class Fourier {

  public static Complex[] Straight1 (double[] values) {
    return Straight1(values.Select(x => new Complex(x, 0d)).ToArray());
  }

  public static Complex[] Straight1 (Complex[] values) {
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

  public static Complex[] Reverse1 (Complex[] values) {
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

  public static Complex[] Straight3 (Complex[] values) {
    
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
