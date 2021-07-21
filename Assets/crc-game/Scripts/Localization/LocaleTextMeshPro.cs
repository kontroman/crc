using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class LocaleTextMeshPro : MonoBehaviour
{
    [SerializeField]
    private string textKey;

    [SerializeField]
    private bool autoUpdate = true;

    private TextMeshProUGUI textComponent;
    private Localization _manager;

    private void Awake()
    {
        textComponent = GetComponent<TextMeshProUGUI>();

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
