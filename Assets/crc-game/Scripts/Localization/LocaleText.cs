using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class LocaleText : MonoBehaviour
{
    [SerializeField]
    private string textKey;

    [SerializeField]
    private bool autoUpdate = true;

    private Text textComponent;
    private Localization _manager;

    private void Awake()
    {
        if (TryGetComponent(out Text text))
        {
            textComponent = text;
        }
        _manager = GameObject.Find("Localization").GetComponent<Localization>();

        if (autoUpdate)
            _manager.languageChanged += UpdateLocale;
    }

    private void Start()
    {
        UpdateLocale();
    }

    private void UpdateLocale()
    {
        try
        {
            string responce = _manager.GetText(textKey);
            if (responce != null && textComponent != null)
                textComponent.text = responce;
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }
}
