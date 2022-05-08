using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TieConditions : MonoBehaviour
{
    public static TieConditions Instance { get; private set; }

    public GameObject[] pawns;

    public GameObject player1;
    public GameObject player2;

    private void Awake()
    {
        if (Instance != null) return;
        else Instance = this;
       pawns = GameObject.FindGameObjectsWithTag("ShakePanel");
    }

    public void CheckTie()
    {
        if (CheckTieCondition())
        {
            GameController.Instance.SetTie();
            Debug.Log("yes");
        }
        Debug.Log(CheckTieCondition());
            Debug.Log("no");
    }

    public bool CheckTieCondition()
    {
        bool trigger = true;
        for (int i = 0; i < pawns.Length; i++ )
        {
            if (pawns[i].gameObject.transform.childCount != 0)
            {
                Debug.Log(pawns[i].gameObject.transform.childCount);
                trigger = false;
            }
        }
        return trigger;
    }
}
