using System.Numerics;

namespace Fourier;

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
        sum += new(double.Cos(coeff * j), -double.Sin(coeff * j)) * values[j];
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
}
