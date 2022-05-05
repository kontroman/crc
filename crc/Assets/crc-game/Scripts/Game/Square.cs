using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;

[Serializable]
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

    [PunRPC]
    public void SetOwnerMultiplayer(int viewID)
    {
        PlayerController player = GetByViewID.Instance.GetById(viewID).gameObject.GetComponent<PlayerController>();

        while(player == null)
        {
            GetByViewID.Instance.GetById(viewID).gameObject.GetComponent<PlayerController>();
        }

        owner = player;
    }

    public void ChangeTileSize(Sizes _size)
    {
        currentTileSize = _size;
    }

    [PunRPC]
    public void ChangeTileSizeMultiplayer(int sizeID)
    {
        currentTileSize = (Sizes)sizeID;
    }
}

