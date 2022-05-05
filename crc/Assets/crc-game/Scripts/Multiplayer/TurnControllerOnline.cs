using Photon.Pun;
using Photon;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnControllerOnline : MonoBehaviourPun, IPunObservable
{
    public static TurnControllerOnline Instance { get; private set; }

    //photon view ID of actor
    public int currentPlayer;

    public List<PlayerController> players = new List<PlayerController>();

    private void Start()
    {
        if (Instance != null) return;
        else Instance = this;
    }
    
    public PlayerController GetCurrentPlayer()
    {
        return players[currentPlayer];
    }

    void Update() //WTF
    {
        if (players.Count != 2  && GameControllerOnline.Instance.players.Count == 2)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (GameControllerOnline.Instance.players[0].GetComponent<PhotonView>().IsMine)
                {
                    players.Add(GameControllerOnline.Instance.players[0]);
                    players.Add(GameControllerOnline.Instance.players[1]);
                }
                else
                {
                    players.Add(GameControllerOnline.Instance.players[1]);
                    players.Add(GameControllerOnline.Instance.players[0]);
                }
            }
            else
            {
                if (GameControllerOnline.Instance.players[0].GetComponent<PhotonView>().IsMine)
                {
                    players.Add(GameControllerOnline.Instance.players[1]);
                    players.Add(GameControllerOnline.Instance.players[0]);
                }
                else
                {
                    players.Add(GameControllerOnline.Instance.players[0]);
                    players.Add(GameControllerOnline.Instance.players[1]);
                }
            }
        }
    }

    //non-master call this to change move in master client and send it in observable
    //���� ������� ��� ����� � ienumerator - ��� ��������. ������?
    [PunRPC]
    public void ChangeMove()
    {
        currentPlayer = currentPlayer == 0 ? 1 : 0; //��� ��������� ������ �������� ����� �������� ����
        if (!PhotonNetwork.IsMasterClient)//���� ��� ��� �� ������ - �� �������� ������ � �������
            PhotonView.Get(this).RPC("ChangeMove", RpcTarget.MasterClient);
    }
    
    public void ResetTurnController()
    {
        players.Clear();

        ChangeMove();
    }

    //photonView`s actorID as parametr
    public bool IsMyTurn(int player)
    {
        return currentPlayer == player;
    }

    [PunRPC]
    public void SetNewPlayerToMove(int newPlayer)
    {
        currentPlayer = newPlayer;
    }


    //������ ���� ����������. �������� - ���������
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting) //��� ����� ������ ������
        {
            stream.SendNext(currentPlayer);
            Debug.Log("Send: new player " + currentPlayer);
        }
        if (stream.IsReading) //��� ����� ������ ������ ������
        {
            currentPlayer = (int)stream.ReceiveNext();
            Debug.Log("Recieved: new player " + currentPlayer);
        }
    }
}
