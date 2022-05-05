using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRPC : MonoBehaviour
{
    public static TestRPC Instance { get; private set; }

    [SerializeField]
    public static int Opp = 5;

    float timer = 2f;

    private void Awake()
    {
        if (Instance != null) return;
        else Instance = this;
    }

    [PunRPC]
    public void AddOpp(int add)
    {
        Opp += add;
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
