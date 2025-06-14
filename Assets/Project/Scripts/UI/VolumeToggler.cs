using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using YG;

public class VolumeToggler : MonoBehaviour
{
    [SerializeField] private Toggle _button;
    [SerializeField] private AudioMixerGroup _audioMixer;

    [SerializeField] private bool _isEnabled = true;
    
    private float _minlVolume = -80;

    public void OnEnable()
    {
        _button.onValueChanged.AddListener(ToggleMusic);
    }
    
    private void OnDisable()
    {
        _button.onValueChanged.RemoveListener(ToggleMusic);
    }
    
    private void ToggleMusic(bool isMuted)
    {
        if (isMuted)
            _audioMixer.audioMixer.SetFloat(_audioMixer.name, _minlVolume);

        _isEnabled = !isMuted;
    }
    
    private void SetCurrentVolume(float volume)
    {
        _audioMixer.audioMixer.SetFloat(_audioMixer.name, Mathf.Log10(volume) * 40);
    }
}