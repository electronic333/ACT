#pragma once

#include "Header.h"

template <typename T>
void swap(T* first, T* second) {
	T temp = *first;
	*first = *second;
	*second = temp;
}

template <typename T>
void reverse(T* arr, int count) {
	for (int i = 0; i < count % 2; i++) {
		swap(arr + i, arr + count - i - 1);
	}
}

void printArrayInt(int* arr, int count) {
	printf_s("[ ");
	for (int i = 0; i < count; i++) {
		if (i != 0) {
			printf_s(", ");
		}
		printf_s("%d", *(arr + i));
	}
	printf_s(" ]");
}

void printArrayFloat(float* arr, int count) {
	printf_s("[ ");
	for (int i = 0; i < count; i++) {
		if (i != 0) {
			printf_s(", ");
		}
		printf_s("%f", *(arr + i));
	}
	printf_s(" ]");
}

int* initRandArrInt(int count, int maxVal) {
	int* result = new int[count];

	for (int i = 0; i < count; i++) {
		*(result + i) = rand() % maxVal;
	}

	return result;
}

float* initRandArrFloat(int count, float compress) {
	float* result = new float[count];
	for (int i = 0; i < count; i++) {
		*(result + i) = (float)rand() / compress;
	}
	return result;
}

template <typename T>
T* copyArray(T* arr, int count) {
	T* result = new T[count];

	for (int i = 0; i < count; i++) {
		*(result + i) = *(arr + i);
	}

	return result;
}

void bubbleSort(int* arr, int count) {
	for (int i = 0; i < count; i++) {
		for (int j = i + 1; j < count; j++) {
			if (*(arr + j) < *(arr + i)) {
				swap(arr + j, arr + i);
			}
		}
	}
}

void selectionSort(int* arr, int count) {
	for (int i = 0; i < count; i++) {
		int minVal = *(arr + i);
		int minIdx = i;

		for (int j = i + 1; j < count; j++) {
			int curVal = *(arr + j);
			if (curVal < minVal) {
				minVal = curVal;
				minIdx = j;
			}
		}
		swap(arr + i, arr + minIdx);
	}
}

void mergeSort(int* arr, int count) {
	int* temp = copyArray(arr, count);

	for (int blockSize = 1; blockSize < count; blockSize *= 2) {
		for (int start = 0; start < count - blockSize; start += 2 * blockSize) {
			int mid = start + blockSize - 1;
			int end = std::min(start + 2 * blockSize - 1, count - 1);

			int left = start;
			int right = mid + 1;
			int index = start;

			while (left <= mid && right <= end) {
				if (*(arr + left) <= *(arr + right)) {
					*(temp + index) = *(arr + left);
					++left;
				}
				else {
					*(temp + index) = *(arr + right);
					++right;
				}
				++index;
			}

			while (left <= mid) {
				*(temp + index) = *(arr + left);
				++left;
				++index;
			}

			while (right <= end) {
				*(temp + index) = *(arr + right);
				++right;
				++index;
			}

			for (index = start; index <= end; ++index) {
				*(arr + index) = *(temp + index);
			}
		}
	}

	delete[] temp;
}

