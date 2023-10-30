import numpy as np
import matplotlib.pyplot as plt
from functools import reduce

def fourier1One (input, k):
    count = len(input)
    realSum = 0
    imagSum = 0
    for i in range(count):
        elem = input[i]
        



def fourier1List (input):
    for i in range(len(input)):
        yield fourier1One(input, i)
