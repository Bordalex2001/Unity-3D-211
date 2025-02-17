using UnityEngine;
using UnityEngine.UI;

public class SettingsScript : MonoBehaviour
{
    private GameObject content;
    private Slider effectsSlider;
    private Slider ambientSlider;
    private Toggle soundOffToggle;
    private Slider sensitivityXSlider;
    private Slider sensitivityYSlider;
    private Slider fpvSlider;
    private Toggle linkToggle;
    
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
            .Find("Sound/SoundOffToggle")
            .GetComponent<Toggle>();
        sensitivityXSlider = contentTransform
            .Find("Controls/SensitivityXSlider")
            .GetComponent<Slider>();
        sensitivityYSlider = contentTransform
            .Find("Controls/SensitivityYSlider")
            .GetComponent<Slider>();
        linkToggle = contentTransform
            .Find("Controls/LinkToggle")
            .GetComponent<Toggle>();
        OnSensitivityXSliderChanged(sensitivityXSlider.value);
        if (!linkToggle.isOn) OnSensitivityYSliderChanged(sensitivityYSlider.value);

        fpvSlider = contentTransform
            .Find("Controls/FpvSlider")
            .GetComponent<Slider>();
        OnFpvSliderChanged(fpvSlider.value);

        LoadSettings();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            OnCloseButtonClick();
        }

        OnSoundToggleChanged(soundOffToggle.isOn);
    }

    private void LoadSettings()
    {
        if (PlayerPrefs.HasKey(nameof(effectsSlider)))
        {
            OnEffectsSliderChanged(
                PlayerPrefs.GetFloat(nameof(effectsSlider))
            );
        }
        if (PlayerPrefs.HasKey(nameof(ambientSlider)))
        {
            OnAmbientSliderChanged(
                PlayerPrefs.GetFloat(nameof(ambientSlider))
            );
        }
        if (PlayerPrefs.HasKey(nameof(soundOffToggle)))
        {
            soundOffToggle.isOn = PlayerPrefs.GetInt(nameof(soundOffToggle)) > 0;
        }
        if (PlayerPrefs.HasKey(nameof(sensitivityXSlider)))
        {
            OnSensitivityXSliderChanged(
                PlayerPrefs.GetFloat(nameof(sensitivityXSlider))
            );
        }
        if (PlayerPrefs.HasKey(nameof(sensitivityYSlider)))
        {
            OnSensitivityYSliderChanged(
                PlayerPrefs.GetFloat(nameof(sensitivityYSlider))
            );
        }
        if (PlayerPrefs.HasKey(nameof(linkToggle)))
        {
            linkToggle.isOn = PlayerPrefs.GetInt(nameof(linkToggle)) > 0;
        }
        if (PlayerPrefs.HasKey(nameof(fpvSlider)))
        {
            OnFpvSliderChanged(
                PlayerPrefs.GetFloat(nameof(fpvSlider))
            );
        }
    }

    public void OnSaveButtonClick()
    {
        PlayerPrefs.SetFloat(nameof(effectsSlider), effectsSlider.value);
        PlayerPrefs.SetFloat(nameof(ambientSlider), ambientSlider.value);
        PlayerPrefs.SetInt(nameof(soundOffToggle), soundOffToggle.isOn ? 1 : 0);
        PlayerPrefs.SetFloat(nameof(sensitivityXSlider), sensitivityXSlider.value);
        PlayerPrefs.SetFloat(nameof(sensitivityYSlider), sensitivityYSlider.value);
        PlayerPrefs.SetInt(nameof(linkToggle), linkToggle.isOn ? 1 : 0);
        PlayerPrefs.SetFloat(nameof(fpvSlider), fpvSlider.value);
        PlayerPrefs.Save();
    }

    public void OnDefaultsButtonClick()
    {
        PlayerPrefs.DeleteAll();
    }

    public void OnCloseButtonClick()
    {
        Time.timeScale = content.activeInHierarchy ? 1.0f : 0.0f;
        content.SetActive(!content.activeInHierarchy);
    }

    public void OnEffectsSliderChanged(float value)
    {
        effectsSlider.value = value;
        GameState.TriggerGameEvent("EffectsVolume", GameState.effectsVolume = value);
    }

    public void OnAmbientSliderChanged(float value)
    {
        ambientSlider.value = value;
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

    public void OnSensitivityXSliderChanged(float value)
    {
        float sens = Mathf.Lerp(1, 10, value);
        sensitivityXSlider.value = value;
        GameState.lookSensitivityX = sens;
        if (linkToggle.isOn)
        {
            sensitivityYSlider.value = value;
            GameState.lookSensitivityY = -sens;
        }
    }

    public void OnSensitivityYSliderChanged(float value)
    {
        float sens = Mathf.Lerp(1, 10, value);
        sensitivityYSlider.value = value;
        GameState.lookSensitivityY = -sens;
        if (linkToggle.isOn)
        {
            sensitivityXSlider.value = value;
            GameState.lookSensitivityX = sens;
        }
    }

    public void OnFpvSliderChanged(float value)
    {
        fpvSlider.value = value;
        GameState.minFpvDistance = Mathf.Lerp(0.5f, 1.5f, value);
    }
}