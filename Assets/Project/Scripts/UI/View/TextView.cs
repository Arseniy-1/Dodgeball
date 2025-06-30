using TMPro;
using UnityEngine;

public abstract class TextView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textView;

    protected void OnValueChanged(int current, int max)
    {
        _textView.text = current.ToString() + "/" + max.ToString();
    }
}