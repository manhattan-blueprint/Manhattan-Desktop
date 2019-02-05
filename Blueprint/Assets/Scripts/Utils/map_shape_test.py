import math

size = 30
root = math.sqrt(3)

def cos(inp):
    return math.cos(math.radians(inp))

def sin(inp):
    return math.sin(math.radians(inp))

def testFunction(inpFunc):
    for i in [[inpFunc(x, y) for x in range(size)] for y in range(size)]:
        print(i)

topL = lambda x, y: x * root - 1.15 * size <= y <= x * root + size/2
botR = lambda x, y: -x * root + size/2 <= y <= -x * root + 4.3 * size/2

def inHex(x, y):
    if topL(x, y) and botR(x, y):
        return 1
    return 0

testFunction(inHex)
# testFunction(botR)
