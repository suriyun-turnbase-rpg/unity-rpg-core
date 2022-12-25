using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFortuneWheelManager : MonoBehaviour
{
    public AnimationCurve curve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
    public FortuneWheel fortuneWheel;
    public RectTransform wheel;
    public UICurrency uiRewardCurrency;
    public UIItem uiRewardItem;
    public Transform uiRewardContainer;
    public int spinRound = 10;
    public float spinDuration = 2f;

    [Header("Test Tools")]
    public int testRewardIndex;

    private bool _isPlaying = false;
    private float _startAngle = 0f;
    private float _endAngle = 0f;
    private float _spinTime = 0f;
    private float _iconAngle;
    private float _halfIconAngle;
    private float _halfIconAngleWithPaddings;

    private void Update()
    {
        if (!_isPlaying) return;

        _spinTime += Time.deltaTime;
        if (_spinTime > spinDuration)
        {
            _isPlaying = false;
            ShowReward();
        }

        float time = _spinTime / spinDuration;
        float angle = Mathf.Lerp(_startAngle, _endAngle, curve.Evaluate(time));
        wheel.eulerAngles = new Vector3(0, 0, angle);
    }

    public void SetupWheel()
    {
        uiRewardContainer.RemoveAllChildren();
        _iconAngle = 360f / fortuneWheel.rewards.Length;
        _halfIconAngle = _iconAngle * 0.5f;
        _halfIconAngleWithPaddings = _halfIconAngle - (_halfIconAngle * 0.25f);
        for (int i = 0; i < fortuneWheel.rewards.Length; ++i)
        {
            Transform uiTransform;
            var reward = fortuneWheel.rewards[i];
            if (reward.rewardItems != null && reward.rewardItems.Length > 0 && reward.rewardItems[0].item != null && reward.rewardItems[0].amount > 0)
            {
                var ui = Instantiate(uiRewardItem, uiRewardContainer);
                ui.data = new PlayerItem()
                {
                    DataId = reward.rewardItems[0].item.Id,
                    Amount = reward.rewardItems[0].amount,
                };
                uiTransform = ui.transform;
            }
            else if (reward.rewardHardCurrency > 0)
            {
                var ui = Instantiate(uiRewardCurrency, uiRewardContainer);
                ui.data = new PlayerCurrency()
                {
                    DataId = GameInstance.GameDatabase.hardCurrency.Id,
                    Amount = reward.rewardHardCurrency,
                };
                uiTransform = ui.transform;
            }
            else
            {
                var ui = Instantiate(uiRewardCurrency, uiRewardContainer);
                ui.data = new PlayerCurrency()
                {
                    DataId = GameInstance.GameDatabase.softCurrency.Id,
                    Amount = reward.rewardSoftCurrency,
                };
                uiTransform = ui.transform;
            }
            uiTransform.RotateAround(uiRewardContainer.position, Vector3.back, _iconAngle * i);
        }
    }

    public float GetRandomAngles(int rewardIndex)
    {
        float angle = -(_iconAngle * rewardIndex);
        float minAngle = (angle - _halfIconAngleWithPaddings) % 360f;
        float maxAngle = (angle + _halfIconAngleWithPaddings) % 360f;
        return Random.Range(minAngle, maxAngle);
    }

    public void OnClickPlay()
    {
        if (_isPlaying)
            return;
    }

    public void Spin(int rewardIndex)
    {
        _spinTime = 0f;
        _startAngle = _endAngle % 360f;
        _endAngle = -((spinRound * 360f) + GetRandomAngles(rewardIndex));
        _isPlaying = true;
    }

    public void ShowReward()
    {

    }

    [ContextMenu("Test Spin")]
    public void TestSpin()
    {
        Spin(testRewardIndex);
    }
}
