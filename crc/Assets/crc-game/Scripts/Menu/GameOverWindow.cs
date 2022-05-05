using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverWindow : MonoBehaviourPunCallbacks
{
    public void BackToMainMenu()
    {
        PhotonNetwork.Disconnect();
    }

    public void BackToMainMenuSingle()
    {
        SceneManager.LoadScene(0);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        SceneManager.LoadScene(0);

        base.OnDisconnected(cause);
    }
}
