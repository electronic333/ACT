using System.Diagnostics;
using System.Numerics;

var size = 256;

var first = new int[size, size];
var second = new int[size, size];

for (var i = 0; i < size; i++) {
  for (var j = 0; j < size; j++) {
    first[i, j] = (i + j) % 2 == 1 ? -1 : 1;
    second[i, j] = i + j;
  }
}

var methods = new (Func<int[,], int[,], int[,]> Function, string Name)[] {
  (Slow, "Slow"), (SlowSwapped, "SlowSwapped"), (FastMult, "Fast"),
  (SlowSwappedParallelFor, "SlowSwappedParallelFor"),
};

var stopwatch = new Stopwatch();

Console.WriteLine($"Multiplicate two matrices {size}x{size}. Method list:");

foreach (var method in methods) {
  stopwatch.Restart();
  _ = method.Function(first, second);
  stopwatch.Stop();
  Console.WriteLine($"Method: \"{method.Name}\", elapsed time: {stopwatch.Elapsed};");
}

static T[,] Slow<T> (T[,] first, T[,] second)
  where T: IAdditionOperators<T, T, T>, IMultiplyOperators<T, T, T> {

  if (!IsValidMatrixForMult(first, second)) {
    throw new("Wrong sizes of matrices.");
  }

  var result = new T[first.GetLength(0), second.GetLength(1)];

  for (var i = 0; i < first.GetLength(0); i++) {
    for (var j = 0; j < second.GetLength(1); j++) {
      for (var k = 0; k < second.GetLength(0); k++) {
        result[i, j] += first[i, k] * second[k, j];
      }
    }
  }

  return result;
}

static T[,] SlowSwapped<T> (T[,] first, T[,] second) 
  where T : IAdditionOperators<T, T, T>, IMultiplyOperators<T, T, T> {

  if (!IsValidMatrixForMult(first, second)) {
    throw new("Wrong sizes of matrices.");
  }

  var result = new T[first.GetLength(0), second.GetLength(1)];

  for (var i = 0; i < first.GetLength(0); i++) {
    for (var k = 0; k < second.GetLength(0); k++) {
      for (var j = 0; j < second.GetLength(1); j++) {
        result[i, j] += first[i, k] * second[k, j];
      }
    }
  }

  return result;
}

static T[,] SlowSwappedParallelFor<T> (T[,] first, T[,] second)
  where T : IAdditionOperators<T, T, T>, IMultiplyOperators<T, T, T> {

  if (!IsValidMatrixForMult(first, second)) {
    throw new("Wrong sizes of matrices.");
  }

  var result = new T[first.GetLength(0), second.GetLength(1)];
  
  Parallel.For(0, first.GetLength(0), new() {  }, i => {
    for (var k = 0; k < second.GetLength(0); k++) {
      for (var j = 0; j < second.GetLength(1); j++) {
        result[i, j] += first[i, k] * second[k, j];
      }
    }
  });
    
  return result;
}

static T[,] FastMult<T> (T[,] first, T[,] second)
  where T : IAdditionOperators<T, T, T>
          , IMultiplyOperators<T, T, T>
          , ISubtractionOperators<T, T, T> {

  var n = first.GetLength(0);
  var result = new T[n, n];

  if (n == 1) {
    result[0, 0] = first[0, 0] * second[0, 0];
  }
  else {
    var A = new T[n / 2, n / 2];
    var B = new T[n / 2, n / 2];
    var C = new T[n / 2, n / 2];
    var D = new T[n / 2, n / 2];
    var E = new T[n / 2, n / 2];
    var F = new T[n / 2, n / 2];
    var G = new T[n / 2, n / 2];
    var H = new T[n / 2, n / 2];

    // Разбиваем исходные матрицы для рекурсивного умножения
    for (var i = 0; i < n / 2; i++) {
      for (var j = 0; j < n / 2; j++) {
        A[i, j] = first[i, j];
        B[i, j] = first[i, j + n / 2];
        C[i, j] = first[i + n / 2, j];
        D[i, j] = first[i + n / 2, j + n / 2];
        E[i, j] = second[i, j];
        F[i, j] = second[i, j + n / 2];
        G[i, j] = second[i + n / 2, j];
        H[i, j] = second[i + n / 2, j + n / 2];
      }
    }

    // Расчет промежуточных матриц
    var P1 = FastMult(A, SubMatrix(F, H));
    var P2 = FastMult(AddMatrix(A, B), H);
    var P3 = FastMult(AddMatrix(C, D), E);
    var P4 = FastMult(D, SubMatrix(G, E));
    var P5 = FastMult(AddMatrix(A, D), AddMatrix(E, H));
    var P6 = FastMult(SubMatrix(B, D), AddMatrix(G, H));
    var P7 = FastMult(SubMatrix(A, C), AddMatrix(E, F));

    // Вычисление результирующей матрицы
    var C11 = AddMatrix(SubMatrix(AddMatrix(P5, P4), P2), P6);
    var C12 = AddMatrix(P1, P2);
    var C21 = AddMatrix(P3, P4);
    var C22 = SubMatrix(SubMatrix(AddMatrix(P1, P5), P3), P7);

    for (var i = 0; i < n / 2; i++) {
      for (var j = 0; j < n / 2; j++) {
        result[i, j] = C11[i, j];
        result[i, j + n / 2] = C12[i, j];
        result[i + n / 2, j] = C21[i, j];
        result[i + n / 2, j + n / 2] = C22[i, j];
      }
    }
  }

  return result;
}

static T[,] AddMatrix<T> (T[,] matrix1, T[,] matrix2)
  where T: IAdditionOperators<T, T, T> {
  
  var n = matrix1.GetLength(0);
  var result = new T[n, n];

  for (var i = 0; i < n; i++) {
    for (var j = 0; j < n; j++) {
      result[i, j] = matrix1[i, j] + matrix2[i, j];
    }
  }

  return result;
}

static T[,] SubMatrix<T> (T[,] matrix1, T[,] matrix2)
  where T: ISubtractionOperators<T, T, T> {
  
  var n = matrix1.GetLength(0);
  var result = new T[n, n];

  for (var i = 0; i < n; i++) {
    for (var j = 0; j < n; j++) {
      result[i, j] = matrix1[i, j] - matrix2[i, j];
    }
  }

  return result;
}

static bool IsEqual<T> (T[,] first, T[,] second) where T : IEqualityOperators<T, T, bool> {
  if (first.GetLength(0) != second.GetLength(0) ||
      first.GetLength(1) != second.GetLength(1)) {
    return false;
  }

  for (var i = 0; i < first.GetLength(0); i++) {
    for (var j = 0; j < first.GetLength(1); j++) {
      if (first[i, j] != second[i, j]) {
        return false;
      }
    }
  }

  return true;
}

static bool IsValidMatrixForMult<T> (T[,] first, T[,] second) {
  return first.GetLength(1) == second.GetLength(0);
}

static bool IsSquare<T> (T[,] matrix) {
  return matrix.GetLength(0) == matrix.GetLength(1);
}
