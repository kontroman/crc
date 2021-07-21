using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDeck : MonoBehaviour
{
    [SerializeField]
    private List<Transform> points = new List<Transform>();

    [SerializeField]
    public Sprite bigSprite;
    private float sizeBig = 200f;

    [SerializeField]
    public Sprite middleSprite;
    private float sizeMiddle = 150f;

    [SerializeField]
    public Sprite smallSprite;
    private float sizeSmall = 100f;

    private void Start()
    {
        DisplayAvailableTiles();   
    }

    private void DisplayAvailableTiles()
    {
        points[0].gameObject.GetComponent<Image>().sprite = bigSprite;
        points[0].gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(sizeBig, sizeBig);

        points[1].gameObject.GetComponent<Image>().sprite = bigSprite;
        points[1].gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(sizeBig, sizeBig);

        points[2].gameObject.GetComponent<Image>().sprite = middleSprite;
        points[2].gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(sizeMiddle, sizeMiddle);

        points[3].gameObject.GetComponent<Image>().sprite = middleSprite;
        points[3].gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(sizeMiddle, sizeMiddle);

        points[4].gameObject.GetComponent<Image>().sprite = smallSprite;
        points[4].gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(sizeSmall, sizeSmall);

        points[5].gameObject.GetComponent<Image>().sprite = smallSprite;
        points[5].gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(sizeSmall, sizeSmall);

    }
}
