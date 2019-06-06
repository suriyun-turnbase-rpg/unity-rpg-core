using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnimItemEvolve : MonoBehaviour
{
    public UIItem uiLevelUpItem;
    public UIItem[] uiMaterials;
    public UILevel uiLevel;

    public abstract void Play(PlayerItem oldItem, PlayerItem newItem);
}
