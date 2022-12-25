using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFortuneWheel : MonoBehaviour
{
    [System.Serializable]
    public struct RewardUI
    {
        public UICurrency uiRewardCurrency;
        public UIItem uiRewardItem;
    }

    public AnimationCurve curve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
    public RectTransform wheel;
    public int spinRound = 10;
    public float spinDuration = 2f;

    [Header("Test Tools")]
    public int testRewardIndex;

    private bool _isPlaying = false;
    private float _startAngle = 0f;
    private float _endAngle = 0f;
    private float _spinTime = 0f;

    private void Update()
    {
        if (!_isPlaying) return;

        _spinTime += Time.deltaTime;
        if (_spinTime > spinDuration)
        {
            _isPlaying = false;
            ShowReward();
        }

        float s = _spinTime / spinDuration;
        float angle = Mathf.Lerp(_startAngle, _endAngle, curve.Evaluate(s));
        wheel.eulerAngles = new Vector3(0, 0, angle);
    }

    public float GetRandomAngles(int rewardIndex)
    {
        return Random.Range(5 + (45 * rewardIndex), 40 + (45 * rewardIndex));
    }

    public void OnClickPlay()
    {
        if (_isPlaying)
            return;
    }

    public void Spin(int rewardIndex)
    {
        _spinTime = 0f;
        _startAngle = _endAngle % 360;
        _endAngle = -((spinRound * 360) + GetRandomAngles(rewardIndex));
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
