using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RematchPopup : MonoBehaviourPunCallbacks
{
    private int PlayersAgreedRematch = 0;
    private bool inProcess = false;

    [PunRPC]
    public void AcceptRematch()
    {
        if (PhotonNetwork.IsMasterClient)
            PlayersAgreedRematch++;
        else PhotonView.Get(this).RPC("AcceptRematch", RpcTarget.MasterClient, null);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting) //Эту штуку делает мастер
        {
            stream.SendNext(PlayersAgreedRematch);
            Debug.Log("Send: PlayersAgreedRematch " + PlayersAgreedRematch);
        }
        if (stream.IsReading) //Эту штуку делает другой клиент
        {
            PlayersAgreedRematch = (int)stream.ReceiveNext();
            Debug.Log("Recieved: PlayersAgreedRematch " + PlayersAgreedRematch);
        }
    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (PlayersAgreedRematch >= 2 && !inProcess)
            {
                inProcess = true;
                PhotonView.Get(GameControllerOnline.Instance.gameObject).RPC(
                    "ResetGameData",
                    RpcTarget.All
                    );
                //StartCoroutine("StartRematch");
            }
        }
    }

    IEnumerator StartRematch()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount != 2) yield break;

        PhotonNetwork.AutomaticallySyncScene = true;

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(3);
        }

        yield return null;
    }

    //Две кнопки - "рематч" и "выход"
    public void DeclineRematch()
    {
        PhotonNetwork.Disconnect();
        PhotonNetwork.AutomaticallySyncScene = false;
        SceneManager.LoadScene(0);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        PhotonNetwork.AutomaticallySyncScene = false;
        SceneManager.LoadScene(0);

        base.OnDisconnected(cause);
    }
}
