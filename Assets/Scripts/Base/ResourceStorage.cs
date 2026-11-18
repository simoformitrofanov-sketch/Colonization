using System;
using UnityEngine;

public class ResourceStorage : MonoBehaviour
{
    public event Action<int> CountChanged;

    public int Count { get; private set; }

    public void Add()
    {
        Count++;
        CountChanged?.Invoke(Count);
    }
}