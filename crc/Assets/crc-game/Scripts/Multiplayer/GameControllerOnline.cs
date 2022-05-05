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
    private GameObject window;

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

        if (players.Count >= 2)
        SetupPlayers();

        GameObject.Find("GameManager").GetComponent<GameManager>().DeleteWaintingPanel();
    }

    [PunRPC]
    public void SetupPlayers()
    {
        this.enabled = true;

        foreach (Matryoshka sq in player1.playerArm)
            sq.SetOwner(player1);

        foreach (Matryoshka sq in player2.playerArm)
            sq.SetOwner(player2);
    }

    [PunRPC]
    public void SetWinner(int viewID)
    {
        if (winner != null) return;

        winner = GetByViewID.Instance.GetById(viewID).gameObject.GetComponent<PlayerController>();

        GameObject.Find("AudioWinner").GetComponent<AudioSource>().Play();

        if (window != null) return;

        if (PhotonNetwork.IsMasterClient)
            PhotonView.Get(this).RPC("CreateRematchPopup", RpcTarget.MasterClient);

            //PhotonView.Get(this).RPC("SetWinner", RpcTarget.MasterClient, viewID);

        EndGame();
    }

    [PunRPC]
    private void CreateRematchPopup()
    {
        window = GameObject.Find("GameOverWindow(Clone)");

        if (window != null)
        {
            window.transform.SetParent(GameObject.Find("Canvas").transform);
            window.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            return;
        }

        window = PhotonNetwork.Instantiate(
            "GameOverWindow",
            new Vector3(0, 0, 0),
            Quaternion.identity
            );

        PhotonView.Get(this).RPC("SetupWindow", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void ResetGameData()
    {
        GameObject gameField = GameObject.Find("GameField");
        DeckOnline deck = gameField.GetComponent<DeckOnline>();

        deck.ResetOwners();

        Destroy(players[0].gameObject);
        Destroy(players[1].gameObject);

        players.Clear();
        players = new List<PlayerController>(2);

        //нужно ли ждать пока штуки создадутся? 
        GameObject player = PhotonNetwork.Instantiate("Player", new Vector3(0, 0, 0), Quaternion.identity);

        //player.GetComponent<PlayerController>().Init();
        
        FindPlayers();

        TurnControllerOnline.Instance.ResetTurnController();

        foreach(Transform child in gameField.transform)
        {
            child.GetComponent<DropzoneOnline>()._ref = null;
            child.GetComponent<Square>().ChangeTileSize(Sizes.Default);
            child.GetComponent<Square>().SetOwner(null);
            if (child.childCount != 0)
                Destroy(child.GetChild(0).gameObject);
        }

        winner = null;

        gameInProgress = true;

        Destroy(window);
    }

    private bool AmIWinner()
    {
        //Check if winner pc = my pc;
        return winner;
    }

    [PunRPC]
    private void EndGame()
    {
        gameInProgress = false;
    }

    [PunRPC]
    public void SetupWindow()
    {
        window = GameObject.Find("GameOverWindow(Clone)");

        window.transform.SetParent(GameObject.Find("Canvas").transform);
        window.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
    }
}
