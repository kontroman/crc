using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public static MenuButtons Instance { get; private set; }

    public GameObject transition;
    private Manager manager;

    private void Awake()
    {
        if (Instance != null) return;
        else Instance = this;

        manager = new Manager();
    }

    [Photon.Pun.PunRPC]
    public void LoadNewScene(int buildIndex) => StartCoroutine(PlayMultiplayerGame(buildIndex));
    public void LoadNewSoloScene(int buildIndex) => StartCoroutine(PlaySoloGame(buildIndex));

    IEnumerator PlaySoloGame(int buildIndex)
    {
        transition.SetActive(true);
        yield return new WaitForSeconds(1.9f);
        SceneManager.LoadScene(buildIndex);
    }
    IEnumerator PlayMultiplayerGame(int buildIndex)
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount != 2) yield break;

        transition.SetActive(true);
        yield return new WaitForSeconds(1.9f);
        PhotonNetwork.AutomaticallySyncScene = true;
        
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(buildIndex);
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            StartCoroutine(PlaySoloGame(SceneManager.GetActiveScene().buildIndex));
    }

    public void NewRandomGame()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable) return;
        
        manager.Connect();
    }

    public void OpenStatistic()
    {
        throw new NotImplementedException();
    }

    public void OpenRules()
    {
        throw new NotImplementedException();
    }
}