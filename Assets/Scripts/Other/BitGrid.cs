using System;
using System.Collections.Specialized;

public class BitGrid
{
    BitVector32[] data;
    private int width;
    private int height;
    public BitGrid(int _width, int _height)
    {
        width = _width;
        height = _height;
        var bits = width * height;
        int dataSize = (int)MathF.Ceiling(bits / 32f);
        data = new BitVector32[dataSize];
    }
    public void Set(int x, int y, bool value)
    {
        int bitNumber = (x % width) + (y * height);
        int index = (bitNumber / 32);
        data[index][bitNumber % 32] = value;
    }
    public bool Get(int x, int y)
    {
        int bitNumber = (x % width) + (y * height);
        int index = (bitNumber / 32);
        return data[index][bitNumber % 32];
    }
}