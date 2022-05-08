using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnController : MonoBehaviour
{
    public static TurnController Instance { get; private set; }

    private PlayerController playerToMove;
    public PlayerController PlayerToMove { get { return playerToMove; } }

    private void Start()
    {
        if (Instance != null) return;
        else Instance = this;

        Init();
    }

    private void Init()
    {
        ChangeTurn(GameController.Instance.Player1);
    }

    public void ChangeTurn(PlayerController player)
    {
        playerToMove = player;
        TieConditions.Instance.CheckTie();
    }
}
