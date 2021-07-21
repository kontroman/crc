using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnControllerOnline : MonoBehaviourPun
{
    public static TurnControllerOnline Instance { get; private set; }

    private PlayerController playerToMove;
    public PlayerController PlayerToMove { get { return playerToMove; } }

    private void Start()
    {
        if (Instance != null) return;
        else Instance = this;
    }

    public bool IsMyTurn(PlayerController player)
    {
        return PlayerToMove == player;
    }

    [PunRPC]
    public void ChangeTurn(PlayerController player)
    {
        Debug.Log("New player to move: " + player);
        playerToMove = player;
    }
}
