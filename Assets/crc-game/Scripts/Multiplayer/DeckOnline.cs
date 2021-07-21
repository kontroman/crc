using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;

[Serializable]
public class DeckLineOnline
{
    public List<GameObject> lines = new List<GameObject>();

    public GameObject FindTile(string _name)
    {
        return lines.Find(tile => tile.name == _name).gameObject ?? null;
    }

    public bool HasTile(string _name)
    {
        return lines.Find(tile => tile.name == _name);
    }
}

public class DeckOnline : MonoBehaviour
{
    public static DeckOnline Instance { get; private set; }

    public List<DeckLineOnline> deck = new List<DeckLineOnline>(3);

    public List<PlayerController> owners = new List<PlayerController>(9);

    private void Awake()
    {
        if (Instance != null) return;
        else Instance = this;
    }

    public bool AvailableTile(DropzoneOnline zone)
    {
        GameObject tile = deck.Find(t => t.HasTile(zone.gameObject.name)).FindTile(zone.gameObject.name);

        return tile.GetComponent<Square>().GetOwner() != TurnControllerOnline.Instance.PlayerToMove;
    }

    [PunRPC]
    public void AddMarkToTile(DropzoneOnline zone)
    {
        GameObject tile = deck.Find(t => t.HasTile(zone.gameObject.name)).FindTile(zone.gameObject.name);

        PlayerController player = zone._ref.GetComponent<Matryoshka>().Owner;

        SetTileOwner(tile.transform.GetSiblingIndex(), player);
    }

    [PunRPC]
    private void SetTileOwner(int childIndex, PlayerController _newOwner)
    {
        owners[childIndex] = _newOwner;

        WinConditionOnline.Instance.CheckForWin(owners);
    }
}
