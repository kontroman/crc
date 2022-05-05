using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class IPadLayout : PortraitLayout
{
    public IPadLayout(
        GameObject background,
        GameObject gameField,
        bool isSingleGame = true,
        bool isTablet = true
        ) : base(background, gameField, isSingleGame, isTablet)
    {
        //settings
    }
}