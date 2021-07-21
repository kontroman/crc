using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LayoutFactory
{
    public static ILayout Create(
        GameObject background,
        GameObject gameField,
        bool isSingleGame = true,
        bool isTablet = false
        )
    {
        if (Mathf.Abs(Camera.main.aspect - 0.75f) < 0.0001f)
            return new IPadLayout(
                background,
                gameField,
                isSingleGame,
                _ = true
                );
        else
            return new PhoneLayout(
                background,
                gameField,
                isSingleGame,
                _ = false
                );
    }
}
