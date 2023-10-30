#pragma once

#include "Header.h"

const float pi = 3.1415;

void fourierStraight1One(float* inputArr, float* real, float* imag, int count, int k) {
	float sumReal = 0;
	float sumImag = 0;
	float coeff = pi * k * 2 / count;
	float val;

	for (int i = 0; i < count; i++) {
		val = *(inputArr + i);
		sumReal += val * std::cos(coeff * i);
		sumImag += val * std::sin(coeff * i);
	}

	*real = sumReal;
	*imag = -sumImag;
}

// re ((a1 + ib1) * (a2 + ib2)) =
// = re (a1a2 + ia1b2 + ia2b1 - b1b2) =
// = a1a2 - b1b2
void fourierReverse1One(float* realArr, float* imagArr, float* output, int count, int k) {
	float realSum = 0;
	float fcount = (float)count;
	float coeff = pi * k * 2 / fcount;
	float fstReal;
	float fstImag;
	float sndReal;
	float sndImag;

	for (int i = 0; i < count; i++) {
		fstReal = *(realArr + i);
		fstImag = *(imagArr + i);
		sndReal = std::cos(coeff * i);
		sndImag = std::sin(coeff * i);
		realSum += fstReal * sndReal - fstImag * sndImag;
	}

	*output = realSum / fcount;
}

void fourierStraight1Arr(float* inputArr, float* realArr, float* imagArr, int count) {
	for (int i = 0; i < count; i++) {
		fourierStraight1One(inputArr, realArr + i, imagArr + i, count, i);
	}
}

void fourierReverse1Arr(float* realArr, float* imagArr, float* outputArr, int count) {
	for (int i = 0; i < count; i++) {
		fourierReverse1One(realArr, imagArr, outputArr + i, count, i);
	}
}
