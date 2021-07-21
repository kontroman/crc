using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneTransitionImage : MonoBehaviour
{
    private Image image;
    private float fadeTime = 1f;

    private void Awake()
    {
        image = GetComponent<Image>();

        StartCoroutine(changeColor());
    }

    IEnumerator changeColor()
    {
        while (image.color.a > 0)
        {
            image.color = Color.Lerp(image.color, new Color(255,255,255,0), fadeTime * Time.deltaTime);
            yield return null;
        }
    }
}
