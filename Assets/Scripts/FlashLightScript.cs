using UnityEngine;

public class FlashLightScript : MonoBehaviour
{
    private Transform parentTransform;
    private Light flashLight;
    public bool isFlashLightOn;
    public AudioSource switchFlashLightSound;

    public float chargeLevel => FlashLightState.charge;

    void Start()
    {
        parentTransform = transform.parent;
        if (parentTransform == null)
        {
            Debug.LogError("FlashLightScript: parentTransform not found");
        }
        flashLight = GetComponent<Light>();
        FlashLightState.charge = 2.0f;
        isFlashLightOn = false;
        
        switchFlashLightSound = GetComponent<AudioSource>();
        switchFlashLightSound.volume = GameState.effectsVolume;

        GameState.Subscribe(OnBatteryEvent, "Battery");
        GameState.Subscribe(OnSoundsVolumeTrigger, "EffectsVolume");
    }

    
    void Update()
    {
        if (parentTransform == null) return;

        if (FlashLightState.charge > 0 && !GameState.isDay && isFlashLightOn)
        {
            flashLight.intensity = chargeLevel;
            FlashLightState.charge -= Time.deltaTime / FlashLightState.runTime;
        }

        if (GameState.isFpv)
        {
            transform.forward = Camera.main.transform.forward;
        }
        else
        {
            Vector3 f = Camera.main.transform.forward;
            f.y = 0.0f;
            if (f == Vector3.zero) f = Camera.main.transform.up;
            transform.forward = f.normalized;
        }

        if (Input.GetKeyUp(KeyCode.F))
        {
            switchFlashLightSound.Play();
            if (flashLight.enabled == true)
            {
                flashLight.enabled = false;
                isFlashLightOn = false;
            }
            else
            {
                flashLight.enabled = true;
                isFlashLightOn = true;
            }
        }
    }

    private void OnBatteryEvent(string eventName, object data)
    {
        if (data is GameEvents.MessageEvent e)
        {
            FlashLightState.charge += (float)e.data;
        }
    }

    private void OnSoundsVolumeTrigger(string eventName, object data)
    {
        if (eventName == "EffectsVolume")
        {
            switchFlashLightSound.volume = (float)data;
        }
    }

    private void OnDestroy()
    {
        GameState.Unsubscribe(OnBatteryEvent, "Battery");
        GameState.Unsubscribe(OnSoundsVolumeTrigger, "EffectsVolume");
    }
}