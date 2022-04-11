using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Tuple<T1, T2>
{
    public readonly T1 Item1;
    public readonly T2 Item2;

    public Tuple(T1 item1, T2 item2)
    {
        Item1 = item1;
        Item2 = item2;
    }

}
[System.Serializable]
public struct TriComponent<T1, T2, T3>
{
    public readonly T1 Item1;
    public readonly T2 Item2;
    public readonly T3 Item3;

    public TriComponent(T1 item1, T2 item2, T3 item3)
    {
        Item1 = item1;
        Item2 = item2;
        Item3 = item3;
    }
}