using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerController : MonoBehaviour
{
    public List<Matryoshka> playerArm = new List<Matryoshka>();

    public void Start()
    {
        Init();
    }

    public void Init()
    {
        PlayerController[] players = new PlayerController[2];

        while (players[0] == null && players[1] == null)
        {
            players = FindObjectsOfType<PlayerController>();
        }

        FindObjectOfType<GameControllerOnline>().AddPlayer(this);

        foreach (PlayerController go in players)
        {
            if (!go.gameObject.GetComponent<PhotonView>().IsMine)
            {
                go.gameObject.GetComponent<PlayerDeck>().bigSprite = Resources.Load<Sprite>("Sprites/Interface/Game/red_big");
                go.gameObject.GetComponent<PlayerDeck>().middleSprite = Resources.Load<Sprite>("Sprites/Interface/Game/red_middle");
                go.gameObject.GetComponent<PlayerDeck>().smallSprite = Resources.Load<Sprite>("Sprites/Interface/Game/red_small");
                go.gameObject.transform.SetParent(GameObject.Find("Canvas").transform, false);
                go.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(0, 600, 0);
                go.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector3(1, 1, 1);
            }
            else
            {
                go.gameObject.GetComponent<PlayerDeck>().bigSprite = Resources.Load<Sprite>("Sprites/Interface/Game/yellow_big");
                go.gameObject.GetComponent<PlayerDeck>().middleSprite = Resources.Load<Sprite>("Sprites/Interface/Game/yellow_middle");
                go.gameObject.GetComponent<PlayerDeck>().smallSprite = Resources.Load<Sprite>("Sprites/Interface/Game/yellow_small");
                go.gameObject.transform.SetParent(GameObject.Find("Canvas").transform, false);
                go.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(0, -600, 0);
                go.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector3(1, 1, 1);
            }
        }
        FillArmWithMatryoshkas();
    }

    private void FillArmWithMatryoshkas()
    {
        AddMatryoshka(Sizes.Big);
        AddMatryoshka(Sizes.Big);

        AddMatryoshka(Sizes.Medium);
        AddMatryoshka(Sizes.Medium);

        AddMatryoshka(Sizes.Small);
        AddMatryoshka(Sizes.Small);
    }

    private void AddMatryoshka(Sizes _size)
    {
        playerArm.Add(new Matryoshka(_size));
    }
}
