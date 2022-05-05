using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCondition : MonoBehaviour
{
    public static WinCondition Instance { get; private set; }

    private List<PlayerController> owners;

    private void Awake()
    {
        if (Instance != null) return;
        else Instance = this;
    }

    public void CheckForWin(List<PlayerController> _owners)
    {
        owners = _owners;

        if (CheckLine(0, 1, 2))
            GameController.Instance.SetWinner(owners[0]);
        if (CheckLine(3, 4, 5))
            GameController.Instance.SetWinner(owners[3]);
        if (CheckLine(6, 7, 8))
            GameController.Instance.SetWinner(owners[6]);

        if (CheckLine(0, 3, 6))
            GameController.Instance.SetWinner(owners[0]);
        if (CheckLine(1, 4, 7))
            GameController.Instance.SetWinner(owners[1]);
        if (CheckLine(2, 5, 8))
            GameController.Instance.SetWinner(owners[2]);

        if (CheckLine(0, 4, 8))
            GameController.Instance.SetWinner(owners[0]);
        if (CheckLine(2, 4, 6))
            GameController.Instance.SetWinner(owners[2]);
    }

    private bool CheckLine(int x, int y, int z)
    {
         return owners[x] == owners[y] && owners[y] == owners[z] && owners[x] != null;
    }

}
