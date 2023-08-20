using UnityEngine;

[System.Serializable]
public class Array2D<T>
{
    [System.Serializable]
    public class Array1D
    {
        public T[] array;
    }

    public Array1D[] array;

    public T[] this[int index]
    {
        get { return array[index].array; }
        set { array[index].array = value; }
    }
}