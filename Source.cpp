#include "Header.h"

void taskSorting() {
	int maxVal = 100;
	int length = 10;

	int* arr1 = initRandArrInt(length, maxVal);
	int* arr2 = copyArray(arr1, length);
	int* arr3 = copyArray(arr1, length);

	printf_s("Source: ");
	printArrayInt(arr1, length);

	bubbleSort(arr1, length);
	selectionSort(arr2, length);
	mergeSort(arr3, length);

	printf_s("\nBubbleSort: ");
	printArrayInt(arr1, length);

	printf_s("\nSelectionSort: ");
	printArrayInt(arr2, length);

	printf_s("\nMergeSort: ");
	printArrayInt(arr3, length);

	delete[] arr1;
	delete[] arr2;
	delete[] arr3;
}

void taskFourier1() {
	int length = 10;
	float* arrInit1 = initRandArrFloat(length, 100);
	float* arrInit2 = new float[length];
	float* arrReal = new float[length];
	float* arrImag = new float[length];

	fourierStraight1Arr(arrInit1, arrReal, arrImag, length);
	fourierReverse1Arr(arrReal, arrImag, arrInit2, length);

	printf_s("Source: ");
	printArrayFloat(arrInit1, length);
	printf_s("\nProcessed: ");
	printArrayFloat(arrInit2, length);

	delete[] arrInit1;
	delete[] arrInit2;
	delete[] arrReal;
	delete[] arrImag;
}

void taskMult() {
	int a = 259;
	int b = 301;

	printf_s("a = %d, b = %d, res = %d\n", a, b, mult(a, b));
}

void taskConvolution() {
	int firstCount = 6;
	int secondCount = 7;
	int linearCount;
	int fourierCount;

	float* first = initRandArrFloat(firstCount, 10000);
	float* second = initRandArrFloat(secondCount, 10000);
	float* linear = linearConvolution(first, firstCount, second, secondCount, &linearCount);
	float* fourier = fourierConvolution1(first, firstCount, second, secondCount, &fourierCount);

	printf_s("First: ");
	printArrayFloat(first, firstCount);

	printf_s("\nSecond: ");
	printArrayFloat(second, secondCount);
	
	printf_s("\nLinearConvolution: ");
	printArrayFloat(linear, linearCount);
	
	printf_s("\nFourierConvolution1: ");
	printArrayFloat(fourier, fourierCount);

	delete[] first;
	delete[] second;
	delete[] linear;
	delete[] fourier;
}

int main() {
	srand(time(0));
	taskConvolution();
}

