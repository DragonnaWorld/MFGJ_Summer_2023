using System;
using System.Collections;
using System.Collections.Generic;

public class IDGenerator
{
    readonly Queue<int> list;
    readonly bool[] isInUse;

    public IDGenerator(int size)
    {
        list = new();
        isInUse = new bool[size];
        for (int i = 0; i < size; ++i)
            list.Enqueue(i);
    }

    public int Generate()
    {
        if (list.Count == 0)
            throw new ArgumentException("There is no more ID to generate");
        var res = list.Dequeue();
        isInUse[res] = true;
        return res;
    }

    public void Retrieve(int id)
    {
        if (!isInUse[id])
            throw new ArgumentException("ID is not used yet");
        isInUse[id] = false;
        list.Enqueue(id);
    }
}