using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinConditionOnline : MonoBehaviour
{
    public static WinConditionOnline Instance { get; private set; }

    private List<PlayerController> owners;

    private void Awake()
    {
        if (Instance != null) return;
        else Instance = this;
    }
    
    [PunRPC]
    public void CheckForWin()
    {
        owners = DeckOnline.Instance.owners;

        if (CheckLine(0, 1, 2))
            PhotonView.Get(this).RPC("SetWinner", RpcTarget.All, PhotonView.Get(owners[0]).ViewID);
            //GameControllerOnline.Instance.SetWinner(owners[0]);
        if (CheckLine(3, 4, 5))
            PhotonView.Get(this).RPC("SetWinner", RpcTarget.All, PhotonView.Get(owners[3]).ViewID);
        if (CheckLine(6, 7, 8))
            PhotonView.Get(this).RPC("SetWinner", RpcTarget.All, PhotonView.Get(owners[6]).ViewID);

        if (CheckLine(0, 3, 6))
            PhotonView.Get(this).RPC("SetWinner", RpcTarget.All, PhotonView.Get(owners[0]).ViewID);
        if (CheckLine(1, 4, 7))
            PhotonView.Get(this).RPC("SetWinner", RpcTarget.All, PhotonView.Get(owners[1]).ViewID);
        if (CheckLine(2, 5, 8))
            PhotonView.Get(this).RPC("SetWinner", RpcTarget.All, PhotonView.Get(owners[2]).ViewID);

        if (CheckLine(0, 4, 8))
            PhotonView.Get(this).RPC("SetWinner", RpcTarget.All, PhotonView.Get(owners[0]).ViewID);
        if (CheckLine(2, 4, 6))
            PhotonView.Get(this).RPC("SetWinner", RpcTarget.All, PhotonView.Get(owners[2]).ViewID);
    }

    private bool CheckLine(int x, int y, int z)
    {
        return owners[x] == owners[y] && owners[y] == owners[z] && owners[x] != null;
    }
}
