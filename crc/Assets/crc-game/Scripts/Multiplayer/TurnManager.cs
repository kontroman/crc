using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using Photon.Pun;

public class TurnManager : MonoBehaviourPun, IPunTurnManagerCallbacks
{
    public void OnPlayerFinished(Player player, int turn, object move)
    {
        throw new System.NotImplementedException();
    }

    public void OnPlayerMove(Player player, int turn, object move)
    {
        Debug.Log("Player moved");
    }

    public void OnTurnBegins(int turn)
    {
        throw new System.NotImplementedException();
    }

    public void OnTurnCompleted(int turn)
    {
        throw new System.NotImplementedException();
    }

    public void OnTurnTimeEnds(int turn)
    {
        throw new System.NotImplementedException();
    }
}
