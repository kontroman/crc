using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrcGame : MonoBehaviour
{
    public GameObject background;
    public GameObject gameField;

    public bool isSingleGame = true;

    private ILayout _layout;

    void Awake()
    {
        _layout = LayoutFactory.Create(
            background,
            gameField,
            isSingleGame
            );

        _layout.Setup();
    }
}
