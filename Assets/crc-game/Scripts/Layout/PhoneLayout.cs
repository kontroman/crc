using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PhoneLayout : PortraitLayout
{
    public PhoneLayout(
        GameObject background,
        GameObject gameField,
        bool isSingleGame = true,
        bool isTablet = false
        ) : base (background, gameField, isSingleGame, isTablet)
    {
        //settings
    }
}