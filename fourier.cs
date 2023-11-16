using System.Numerics;

namespace Fourier;

public static class Fourier {
  
  public static Complex[] Straight1 (double[] values) {
    var valuesc = values.Select(x => new Complex(x, 0)).ToArray();
    var result = new Complex[values.Length];

    for (int i = 0; i < values.Length; i++) {
      var coeff = double.Pi * i * 2 / values.Length;
      var sum = new Complex();
      
      for (int j = 0; j < values.Length; j++) {
        sum += new(double.Cos(coeff * j), -double.Sin(coeff * j)) * result[j];
      }
      
      result[i] = sum;
    }
    
    return result;
  }

  public static Complex[] Reverse1 (Complex[] values) {
    var result = values.Length;

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

  public static Complex[] LinearConvolution (Complex[] input1, Complex[] input2) {
    var result = new Complex[input1.Length + input2.Length - 1];

    for (int i = 0; i < input1.Length; i++) {
      for (int j = 0; j < input2.Length; j++) {
        result[i + j] = input1[i] * input2[j];
      }
    }

    return result;
  }

  public static float[] FourierConvolution (float[] a, float[] b, Func<float[], Complex[]> straight, Func<Complex[], Complex[]> reversed) {
    return reversed(Enumerable.Zip(straight(a), straight(b), (x, y) => x * y).ToArray());
  }
}
