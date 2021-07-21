using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Matryoshka : MonoBehaviour
{
    public Sizes size;

    [SerializeField]
    private PlayerController owner;
    public PlayerController Owner { get { return owner; } }

    public Matryoshka(Sizes _size)
    {
        this.size = _size;
    }

    public void SetOwner(PlayerController _onwer)
    {
        owner = _onwer;
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