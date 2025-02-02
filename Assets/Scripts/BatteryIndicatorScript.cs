using UnityEngine;
using UnityEngine.UI;

public class BatteryIndicatorScript : MonoBehaviour
{
    private Image image;
    private FlashLightScript flashLight;
    private AudioSource lowBatterySound;
    private AudioSource criticalLowBatterySound;
    private bool hasLowBattery = false;
    private bool hasCriticalLowBattery = false;
    
    void Start()
    {
        image = GetComponent<Image>();
        flashLight = GameObject
            .Find("FlashLight")
            .GetComponent<FlashLightScript>();

        AudioSource[] audioSources = GetComponents<AudioSource>();
        lowBatterySound = audioSources[0];
        criticalLowBatterySound = audioSources[1];

        lowBatterySound.volume = GameState.effectsVolume;
        criticalLowBatterySound.volume = GameState.effectsVolume;
        GameState.Subscribe(OnSoundsVolumeTrigger, "EffectsVolume");
    }

    void Update()
    {
        float chargeLevel = flashLight.chargeLevel;
        image.fillAmount = chargeLevel;

        if (chargeLevel > 0.8f)
        {
            image.color = Color.green;
        }
        else if (chargeLevel > 0.3f)
        {
            image.color = Color.yellow;
        }
        else
        {
            image.color = Color.red;
        }

        if (chargeLevel <= 0.3f && !hasLowBattery)
        {
            lowBatterySound.Play();
            hasLowBattery = true;
        }
        else if (chargeLevel <= 0.1f && !hasCriticalLowBattery)
        {
            criticalLowBatterySound.Play();
            hasCriticalLowBattery = true;
        }
    }

    private void OnSoundsVolumeTrigger(string eventName, object data)
    {
        if (eventName == "EffectsVolume")
        {
            lowBatterySound.volume = (float)data;
            criticalLowBatterySound.volume = (float)data;
        }
    }

    private void OnDestroy()
    {
        GameState.Unsubscribe(OnSoundsVolumeTrigger, "EffectsVolume");
    }
}