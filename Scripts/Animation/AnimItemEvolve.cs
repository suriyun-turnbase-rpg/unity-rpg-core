using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimItemEvolve : MonoBehaviour
{
    public UIItem uiEvolveItem;
    public UIItem[] uiMaterials;
    public UILevel uiLevel;
    public Animator animAnimator;
    [Tooltip("Total duration = `animationDurationEachMaterials` * {materials amount} + `extraAnimationDuration`")]
    public float animationDurationEachMaterials = 0.8f;
    [Tooltip("Total duration = `animationDurationEachMaterials` * {materials amount} + `extraAnimationDuration`")]
    public float extraAnimationDuration = 0f;
    public UnityEvent onEvolve;
    public UnityEvent onStart;
    public UnityEvent onEnd;

    private PlayerItem oldItem;
    private PlayerItem newItem;

    public void Play(PlayerItem oldItem, PlayerItem newItem, List<PlayerItem> materials)
    {
        onStart.Invoke();

        this.oldItem = oldItem;
        this.newItem = newItem;
        uiEvolveItem.SetData(oldItem);
        for (var i = 0; i < uiMaterials.Length; ++i)
        {
            var uiMaterial = uiMaterials[i];
            if (i < materials.Count)
            {
                uiMaterial.SetData(materials[i]);
                uiMaterial.Show();
            }
            else
            {
                uiMaterial.Hide();
            }
        }

        uiLevel.gameObject.SetActive(oldItem.ActorItemData != null);
        uiLevel.level = oldItem.Level;
        uiLevel.maxLevel = oldItem.MaxLevel;
        uiLevel.collectExp = oldItem.CollectExp;
        uiLevel.nextExp = oldItem.NextExp;

        gameObject.SetActive(true);

        if (animAnimator != null)
            animAnimator.SetInteger("MaterialsAmount", materials.Count);

        StartCoroutine(DelayBeforePlay(animationDurationEachMaterials * materials.Count + extraAnimationDuration));
    }

    IEnumerator DelayBeforePlay(float delay)
    {
        yield return new WaitForSeconds(delay);
        uiEvolveItem.SetData(newItem);
        uiLevel.level = newItem.Level;
        uiLevel.maxLevel = newItem.MaxLevel;
        uiLevel.collectExp = newItem.CollectExp;
        uiLevel.nextExp = newItem.NextExp;
        onEvolve.Invoke();
        yield return null;
        onEnd.Invoke();
    }
}
