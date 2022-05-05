using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Matryoshka : MonoBehaviour
{
    [SerializeField]
    public Sizes size;

    [SerializeField]
    public PlayerController Owner;

    public Matryoshka() { }

    public Matryoshka(Sizes _size)
    {
        this.size = _size;
    }

    public void SetOwner(PlayerController _onwer)
    {
        Owner = _onwer;
    }

    public Sizes GetSize()
    {
        return size;
    }

    public void DeleteThisTile()
    {
        Destroy(gameObject);
    }

}