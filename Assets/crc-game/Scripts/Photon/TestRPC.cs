using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRPC : MonoBehaviour, IPunObservable
{
    public static TestRPC Instance { get; private set; }

    [SerializeField]
    private int Opp = 5;

    float timer = 2f;

    private void Awake()
    {
        if (Instance != null) return;
        else Instance = this;
    }

    [PunRPC]
    public void AddOpp()
    {
        Opp += 5;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
            stream.SendNext(Opp);
        if (stream.IsReading)
            Opp = (int)stream.ReceiveNext();
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            Debug.LogError(Opp);
            timer = 2f;
        }
    }
}
