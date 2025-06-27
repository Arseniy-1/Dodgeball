using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System;
using System.Threading.Tasks;

public class RankViewCanvas : InteractiveCanvas
{
    [SerializeField] private Image _chestFillImage;
    [SerializeField] private TMP_Text _percentageText;

    private const int MaxAmount = 100;
    private RankHolder _rankHolder;
    private Coroutine _animRoutine;

    public event Action OnRewardViewClosed;
    
    public void Initialize(RankHolder rankHolder)
    {
        _rankHolder = rankHolder;
    }

    public void ShowResults()
    {
        if (_animRoutine != null)
            StopCoroutine(_animRoutine);

        _animRoutine = StartCoroutine(AnimateProgress(_rankHolder.PreviousAmount, _rankHolder.CurrentAmount));
    }

    private IEnumerator AnimateProgress(int from, int to)
    {
        float duration = 2.5f;
        float time = 0f;

        float startFill = from / (float)MaxAmount;
        float endFill = to / (float)MaxAmount;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            float currentFill = Mathf.Lerp(startFill, endFill, t);
            _chestFillImage.fillAmount = currentFill;

            int percent = Mathf.RoundToInt(currentFill * 100f);
            _percentageText.text = percent + "%";

            yield return null;
        }

        _chestFillImage.fillAmount = endFill;
        _percentageText.text = (to * 100 / MaxAmount) + "%";
    }
    
    protected override void HandleButtonClick()
    {
        OnRewardViewClosed?.Invoke();
    }
}