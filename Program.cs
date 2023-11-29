using System.Diagnostics;
using System.Numerics;

var size = 4;

var first = new int[size, size];
var second = new int[size, size];

int[,] Slow () {
  var result = new int[size, size];

  for (var i = 0; i < size; i++) {
    for (var j = 0; j < size; j++) {
      for (var k = 0; k < size; k++) {
        result[i, j] += first[i, k] * second[k, j];
      }
    }
  }

  return result;
}

int[,] Fast () {
  var result = new int[size, size];

  

  return result;
}

bool IsEqual<T> (T[,] first, T[,] second) where T: IEqualityOperators<T, T, bool> {
  for (var i = 0; i < size; i++) {
    for (var j = 0; j < size; j++) {
      if (first[i, j] != second[i, j]) {
        return false;
      }
    }
  }

  return true;
}

void Print (int[,] array) {
  for (var i = 0; i < size; i++) {
    for (var j = 0; j < size; j++) {
      Console.Write($"{array[i, j]} ");
    }
    Console.WriteLine();
  }
}

for (var i = 0; i < size; i++) {
  for (var j = 0; j < size; j++) {
    first[i, j] = (i + j) % 2 == 1 ? -1 : 1;
    second[i, j] = i + j;
  }
}

Print(first);
Print(second);
Print(Slow());
Print(Fast());

var sizes = new (int, int)[] { (2, 2), (2, 100), (100, 3), (3, 4) };
Console.WriteLine($"For [{string.Join(", ", sizes)}] sizes minimum actions is {MatrixChain(sizes)}.");

//var stopwatch = new Stopwatch();

//stopwatch.Start();

//var slow = Slow(first, second, size);
//var slowTime = stopwatch.Elapsed;

//stopwatch.Restart();

//var fast = Fast(first, second, size);
//var fastTime = stopwatch.Elapsed;

//stopwatch.Stop();

//Console.WriteLine($"Is matrixs equals: {IsEqual(slow, fast)}");
//Console.WriteLine($"Slow elapsed: {slowTime}");
//Console.WriteLine($"Fast elapsed: {fastTime}");

int MatrixChain (int[] dims) {
  var n = dims.Length;
  var c = new int[n + 1, n + 1];

  for (var len = 2; len <= n; len++) {
    for (var i = 1; i <= n - len + 1; i++) {
      var j = i + len - 1;
      c[i, j] = int.MaxValue;

      for (int k = i; j < n && k <= j - 1; k++) {
        c[i, j] = int.Min(c[i, j], c[i, k] + c[k + 1, j] + dims[i - 1] * dims[k] * dims[j]);
      }
    }
  }
  return c[1, n - 1];
}

