using UnityEngine;
using UnityEngine.UI;

public class SettingsScript : MonoBehaviour
{
    private GameObject content;
    private Slider effectsSlider;
    private Slider ambientSlider;
    private Toggle soundOffToggle;
    
    void Start()
    {
        Transform contentTransform = transform.Find("Content");
        content = contentTransform.gameObject;
        if (content.activeInHierarchy)
        {
            Time.timeScale = 0.0f;
        }
        effectsSlider = contentTransform
            .Find("Sound/EffectsSlider")
            .GetComponent<Slider>();
        OnEffectsSliderChanged(effectsSlider.value);

        ambientSlider = contentTransform
            .Find("Sound/AmbientSlider")
            .GetComponent<Slider>();
        OnAmbientSliderChanged(ambientSlider.value);

        soundOffToggle = contentTransform
            .Find("Sound/Toggle")
            .GetComponent<Toggle>();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Time.timeScale = content.activeInHierarchy ? 1.0f : 0.0f;
            content.SetActive(!content.activeInHierarchy);
        }

        OnSoundToggleChanged(soundOffToggle.isOn);
    }

    public void OnEffectsSliderChanged(float value)
    {
        GameState.TriggerGameEvent("EffectsVolume", GameState.effectsVolume = value);
    }

    public void OnAmbientSliderChanged(float value)
    {
        GameState.TriggerGameEvent("AmbientVolume", GameState.ambientVolume = value);
    }

    public void OnSoundToggleChanged(bool value)
    {
        if (value)
        {
            OnEffectsSliderChanged(0);
            OnAmbientSliderChanged(0);
        }
        else
        {
            OnEffectsSliderChanged(effectsSlider.value);
            OnAmbientSliderChanged(ambientSlider.value);
        }
    }
}