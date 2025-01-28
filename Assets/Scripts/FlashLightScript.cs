using UnityEngine;

public class FlashLightScript : MonoBehaviour
{
    private Transform parentTransform;
    private Light flashLight;
    public bool isFlashLightOn;

    public float chargeLevel => Mathf.Clamp01(FlashLightState.charge);

    void Start()
    {
        parentTransform = transform.parent;
        if (parentTransform == null)
        {
            Debug.LogError("FlashLightScript: parentTransform not found");
        }
        flashLight = GetComponent<Light>();
        FlashLightState.charge = 2.0f;
        GameState.Subscribe(OnBatteryEvent, "Battery");
    }

    
    void Update()
    {
        if (parentTransform == null) return;

        if (FlashLightState.charge > 0 && !GameState.isDay)
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

    private void OnDestroy()
    {
        GameState.Unsubscribe(OnBatteryEvent, "Battery");
    }
}