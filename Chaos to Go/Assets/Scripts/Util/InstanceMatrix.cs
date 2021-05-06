using System;
using System.Collections;

public class InstanceMatrix<T> : IEnumerable
{
    private T[] buffer;
    private uint x, y;

    public InstanceMatrix(uint x, uint y){
        this.x = x;
        this.y = y;
        buffer = new T[x * y];
    }


    public void Set(uint i, uint j, T data)
    {
        buffer[index(i, j)] = data;
    }


    public T Get(uint i, uint j)
    {
        return buffer[index(i, j)];
    }


    private uint index(uint i, uint j)
    {
        return i * y + j;
    }


    public IEnumerator GetEnumerator()
    {
        return buffer.GetEnumerator();
    }
}