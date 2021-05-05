using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseGameData : ScriptableObject
{
    [Tooltip("Default title")]
    public string title;
    [Tooltip("Titles by language keys")]
    public LanguageData[] titles;
    public string Title
    {
        get { return Language.GetText(titles, title); }
    }
    [Multiline]
    public string description;
    [Tooltip("Descriptions by language keys")]
    public LanguageData[] descriptions;
    public string Description
    {
        get { return Language.GetText(descriptions, description); }
    }

    public string tag;

    public virtual string Id { get { return name; } }
    protected virtual void OnValidate() { }
}
