using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacterEntity : MonoBehaviour
{
    [Tooltip("The transform where we're going to spawn uis")]
    public Transform uiContainer;
    [Tooltip("The transform where we're going to spawn body effects")]
    public Transform bodyEffectContainer;
    [Tooltip("The transform where we're going to spawn floor effects")]
    public Transform floorEffectContainer;
    [Tooltip("The transform where we're going to spawn damage")]
    public Transform damageContainer;
}
