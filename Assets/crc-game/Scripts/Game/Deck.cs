using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class DeckLine
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

public class Deck : MonoBehaviour
{
    public static Deck Instance { get; private set; }

    public List<DeckLine> deck = new List<DeckLine>(3);

    public List<PlayerController> owners = new List<PlayerController>(9);

    private void Awake()
    {
        if (Instance != null) return;
        else Instance = this;

        Init();
    }

    private void Init()
    {
       
    }

    public bool AvailableTile(Dropzone zone)
    {
        GameObject tile = deck.Find(t => t.HasTile(zone.gameObject.name)).FindTile(zone.gameObject.name);

        return tile.GetComponent<Square>().GetOwner() != TurnController.Instance.PlayerToMove;
    }
    
    public void AddMarkToTile(Dropzone zone)
    {
        GameObject tile = deck.Find(t => t.HasTile(zone.gameObject.name)).FindTile(zone.gameObject.name);

        PlayerController player = zone._ref.GetComponent<Matryoshka>().Owner;

        SetTileOwner(tile.transform.GetSiblingIndex(), player);
    }

    private void SetTileOwner(int childIndex, PlayerController _newOwner)
    {
        owners[childIndex] = _newOwner;

        WinCondition.Instance.CheckForWin(owners);
    }
}
