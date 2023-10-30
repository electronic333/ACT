#pragma once

#include "Header.h"

const int intBitsCount = 32;

// Умножение столбиком
unsigned mult(unsigned a, unsigned b) {
	unsigned acc = 0;

	for (int i = 0; i < intBitsCount; i++) {
		if (((1 << i) & b) > 0) {
			acc += a << i;
		}
	}
	
	return acc;
}


