using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MyDictionary<TKey, TValue>
{
    public TKey key;
    public TValue value;

    public MyDictionary(TKey key, TValue value)
    {
        this.key = key;
        this.value = value;
    }
}