using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Sizes
{
    Default = 0,

    Small = 1,
    Medium = 2,
    Big = 3
}

public class Square : MonoBehaviour
{
    private PlayerController owner;

    private Sizes currentTileSize = Sizes.Default;
    public Sizes CurrentSize { get { return currentTileSize; } }

    public PlayerController GetOwner()
    {
        return owner;
    }

    public void SetOwner(PlayerController player)
    {
        owner = player;
    }

    public void ChangeTileSize(Sizes _size)
    {
        currentTileSize = _size;
    }
}

