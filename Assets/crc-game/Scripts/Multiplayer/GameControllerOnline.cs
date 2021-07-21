using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun.UtilityScripts;

public class GameControllerOnline : MonoBehaviour
{
    public static GameControllerOnline Instance { get; private set; }

    public bool IsOnline = true;

    [SerializeField]
    private PlayerController player1;
    public PlayerController Player1 { get { return player1; } }

    [SerializeField]
    private PlayerController player2;
    public PlayerController Player2 { get { return player2; } }

    private PlayerController winner;

    private bool gameInProgress = true;
    public bool GameInProgress { get { return gameInProgress; } }

    public List<PlayerController> players = new List<PlayerController>();

    [PunRPC]
    private void Awake()
    {
        if (Instance != null) return;
        else Instance = this;
    }

    public void AddPlayer(PlayerController player)
    {
        players.Add(player);
        Debug.Log("Added new player");

        //TODO: RPC
        if (players.Count == 2) FindPlayers();
    }

    [PunRPC]
    public void FindPlayers()
    {
        Debug.Log("Setuping players");

        player1 = players.Find(p => p.GetComponent<PhotonView>().Owner == PhotonNetwork.CurrentRoom.Players[1]);
        player2 = players.Find(p => p.GetComponent<PhotonView>().Owner == PhotonNetwork.CurrentRoom.Players[2]);

        SetupPlayers();
    }

    [PunRPC]
    public void SetupPlayers()
    {
        this.enabled = true;

        foreach (Matryoshka sq in player1.playerArm)
            sq.SetOwner(player1);

        foreach (Matryoshka sq in player2.playerArm)
            sq.SetOwner(player2);
        
        TurnControllerOnline.Instance.ChangeTurn(player1);
    }

    [PunRPC]
    public void SetWinner(PlayerController _winner)
    {
        winner = _winner;

        GameObject.Find("AudioWinner").GetComponent<AudioSource>().Play();

        EndGame();
    }

    [PunRPC]
    private void EndGame()
    {
        gameInProgress = false;
    }
}
