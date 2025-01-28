using UnityEngine;
using UnityEngine.UI;

public class BatteryIndicatorScript : MonoBehaviour
{
    private Image image;
    private FlashLightScript flashLight;
    
    void Start()
    {
        image = GetComponent<Image>();
        flashLight = GameObject
            .Find("FlashLight")
            .GetComponent<FlashLightScript>();
    }

    void Update()
    {
        image.fillAmount = flashLight.chargeLevel;
        if (image.fillAmount > 0.8f)
        {
            image.color = Color.green;
        }
        else if (image.fillAmount > 0.3f)
        {
            image.color = Color.yellow;
        }
        else
        {
            image.color = Color.red;
        }
    }
}