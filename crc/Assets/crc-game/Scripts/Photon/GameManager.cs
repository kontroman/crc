using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{

    public GameObject playerPrefab;
    public GameObject _canvas;
    public GameObject _waitingPanel;

    private void Awake()
    {
        if (PhotonNetwork.IsMasterClient)
            PhotonView.Get(this.gameObject.GetComponent<PhotonView>()).RPC("CreatePlayers", RpcTarget.AllBuffered);
    }

    public void DeleteWaintingPanel()
    {
        if(_waitingPanel != null)
        Destroy(_waitingPanel);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        PhotonView.Get(this.gameObject.GetComponent<PhotonView>()).RPC("CreatePlayers", RpcTarget.AllBuffered);
    }

    [Photon.Pun.PunRPC]
    private void CreatePlayers()
    {
        GameObject Player = PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0, -600, 0), Quaternion.identity);

        SetPlayerTransform(Player);
    }

    [Photon.Pun.PunRPC]
    private void SetPlayerTransform(GameObject player)
    {
        player.transform.SetParent(_canvas.transform, false);
        player.transform.localPosition = new Vector3(0, -600, 0);
        player.GetComponent<RectTransform>().sizeDelta = new Vector3(1, 1, 1);
    }
}
