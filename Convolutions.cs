public static class Convolutions {
  public static Complex[] Linear (Complex[] input1, Complex[] input2) {
    var result = new Complex[input1.Length + input2.Length - 1];

    for (int i = 0; i < input1.Length; i++) {
      for (int j = 0; j < input2.Length; j++) {
        result[i + j] = input1[i] * input2[j];
      }
    }

    return result;
  }

  public static Complex[] Fourier (float[] a, float[] b, Func<float[], Complex[]> straight, Func<Complex[], Complex[]> reversed) {
    return reversed(Enumerable.Zip(straight(a), straight(b), (x, y) => x * y).ToArray());
  }
}
