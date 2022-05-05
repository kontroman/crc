using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PortraitLayout : ILayout
{
    private bool isTablet;
    private bool isSingleGame;
    private GameObject background;
    private GameObject gameField;

    public PortraitLayout(
        GameObject _background,
        GameObject _gameField,
        bool _isSingleGame,
        bool _isTablet
        )
    {
        this.isSingleGame = _isSingleGame;
        this.gameField = _gameField;
        this.isTablet = _isTablet;
        this.background = _background;
    }

    public void Setup()
    {
        SetupBackgroud();
        SetBackgroundImage();
        SetupGameField();
    }

    private void SetupGameField()
    {
        if (gameField == null) return;

        var fieldRect = gameField.GetComponent<RectTransform>();

        fieldRect.anchorMin = new Vector2(0.5f, 0.5f);
        fieldRect.anchorMax = new Vector2(0.5f, 0.5f);

        fieldRect.pivot = new Vector2(0.5f, 0.5f);

        fieldRect.sizeDelta = new Vector2(940, 940);

        var grid = gameField.GetComponent<GridLayoutGroup>();

        grid.cellSize = new Vector2(300, 300);
        grid.spacing = new Vector2(20, 20);

    }

    private void SetupBackgroud()
    {
        var backRect = background.GetComponent<RectTransform>();

        backRect.anchorMin = new Vector2(0, 0);
        backRect.anchorMax = new Vector2(1, 1);

        backRect.pivot = new Vector2(0.5f, 0.5f);
    }

    private void SetBackgroundImage()
    {
        if (isTablet)
            background.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Interface/Game/back_ipad");
        else
            background.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Interface/Game/back");
    }
}