using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    public bool IsOnline = false;

    [SerializeField]
    private PlayerController player1;
    public PlayerController Player1 { get { return player1; } }

    [SerializeField]
    private PlayerController player2;
    public PlayerController Player2 { get { return player2; } }

    private PlayerController winner;
    private bool gameInProgress = true;
    public bool GameInProgress { get { return gameInProgress; } }

    private void Awake()
    {
        if (Instance != null) return;
        else Instance = this;

        SetupPlayers();
    }

    private void Start()
    {
        if (IsOnline)
            PhotonView.Get(this).RPC("FindPlayers", RpcTarget.All);

        SetupPlayers();
    }

    [PunRPC]
    private void FindPlayers()
    {

        PlayerController[] players = new PlayerController[2];
        players = FindObjectsOfType<PlayerController>();

        foreach (PlayerController pc in players)
        {
            Debug.LogError(pc.name);
            if (pc.GetComponent<PhotonView>().IsMine)
            {
                if (PhotonNetwork.IsMasterClient)
                    player1 = pc;
                else
                    player2 = pc;
            }
            else
            {
                if (!pc.GetComponent<PhotonView>().IsMine)
                    player1 = pc;
                else
                    player2 = pc;
            }
        }

    }

    public void SetupPlayers()
    {
        foreach (Matryoshka sq in player1.playerArm)
            sq.SetOwner(player1);

        foreach (Matryoshka sq in player2.playerArm)
            sq.SetOwner(player2);
    }

    public void SetWinner(PlayerController _winner)
    {
        winner = _winner;

        GameObject.Find("AudioWinner").GetComponent<AudioSource>().Play();

        GameObject window = Instantiate(Resources.Load<GameObject>("Prefabs/GameOverWindowSingle"), new Vector3(0, 0, 0), Quaternion.identity);
        window.transform.SetParent(GameObject.Find("Canvas").transform);
        window.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

        EndGame();
    }

    private void EndGame()
    {
        gameInProgress = false;
    }
}
