#pragma once

#include "Header.h"

float* zerosUpTo(float* input, int currentCount, int awaitCount) {
	float* output = new float[awaitCount];
	
	for (int i = 0; i < currentCount; i++) {
		*(output + i) = *(input + i);
	}

	for (int i = currentCount; i < awaitCount; i++) {
		*(output + i) = 0;
	}

	return output;
}

float* linearConvolution(float* first, int firstCount, 
	                      float* second, int secondCount,
	                      int* outputCountP) {
	
	int outputCount = firstCount + secondCount - 1;
	float* firstExt = zerosUpTo(first, firstCount, outputCount);
	float* secondExt = zerosUpTo(second, secondCount, outputCount);
	float* result = new float[outputCount];
	float sum;

	for (int i = 0; i < outputCount; i++) {
		sum = 0;

		for (int k = 0; k < firstCount; k++) {
			sum += *(firstExt + k) * *(secondExt + std::abs(i - k));
		}
		
		*(result + i) = sum;
	}
	
	*outputCountP = outputCount;

	delete[] firstExt;
	delete[] secondExt;

	return result;
}

// TODO: repair
float* fourierConvolution1(float* first, int firstCount,
	                        float* second, int secondCount,
	                        int* outputCountP) {

	int count = firstCount + secondCount - 1;
	float* firstExt = zerosUpTo(first, firstCount, count);
	float* secondExt = zerosUpTo(second, secondCount, count);
	float* output = new float[count];
	float* firstReal = new float[count];
	float* firstImag = new float[count];
	float* secondReal = new float[count];
	float* secondImag = new float[count];
	float* tempReal = new float[count];
	float* tempImag = new float[count];

	fourierStraight1Arr(firstExt, firstReal, firstImag, count);
	fourierStraight1Arr(secondExt, secondReal, secondImag, count);

	for (int i = 0; i < count; i++) {
		float fr = *(firstReal + i);
		float fi = *(firstImag + i);
		float sr = *(secondReal + i);
		float si = *(secondImag + i);

		*(tempReal + i) = fr * sr - fi * si;
		*(tempImag + i) = fr * si + fi * sr;
	}

	fourierReverse1Arr(tempReal, tempImag, output, count);

	*outputCountP = count;

	delete[] firstExt;
	delete[] secondExt;
	delete[] firstReal;
	delete[] firstImag;
	delete[] secondReal;
	delete[] secondImag;
	delete[] tempReal;
	delete[] tempImag;

	return output;
}
