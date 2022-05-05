using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;

public class Localization : MonoBehaviour
{
    public static Localization Instance { get; private set; }

    private Dictionary<string, string> currentLanguageTexts;

    private string defaultLanguage = "Russian";
    private string currentLanguage;

    public delegate void LanguageChangedEventHandler();
    public event LanguageChangedEventHandler languageChanged;

    private void Awake()
    {
        if (Instance != null) return;
        else Instance = this;

        Init();
    }

    private void Init()
    {
        if (PlayerPrefs.HasKey("LastLanguage"))
        {
            string newLanguage = PlayerPrefs.GetString("LastLanguage");
            try
            {
                SetNewLanguage(newLanguage);
            }
            catch (Exception e)
            {
                SetNewLanguage(defaultLanguage);
            }
        }
        else
        {
            SaveLanguage();
            SetNewLanguage(defaultLanguage);
        }
    }

    public string GetText(string _text)
    {
        if (!currentLanguageTexts.ContainsKey(_text))
            return null;

        return currentLanguageTexts[_text];
    }

    public void SetNewLanguage(string _language)
    {
        TextAsset textAsset = Resources.Load<TextAsset>("Interface/Localization/" + "Locale_" + _language);

        if (textAsset != null)
        {
            currentLanguageTexts = JsonConvert.DeserializeObject<Dictionary<string, string>>(textAsset.text);
            currentLanguage = _language;
            SaveLanguage();
            onLanguageChanged();
        }
        else
        {
            throw new Exception("Localization Error");
        }
    }

    private void SaveLanguage()
    {
        PlayerPrefs.SetString("LastLanguage", currentLanguage);
    }

    private void OnApplicationQuit()
    {
        SaveLanguage();
    }

    protected virtual void onLanguageChanged()
    {
        if (languageChanged != null)
            languageChanged();
    }

}
