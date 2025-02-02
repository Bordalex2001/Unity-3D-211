using UnityEngine;
using UnityEngine.UI;

public class TimerImageScript : MonoBehaviour
{
    private Image image;
    private KeyPointScript keyPointScript;

    void Start()
    {
        image = GetComponent<Image>();
        Transform t = transform;
        while (t != null && 
            (keyPointScript = t.gameObject.GetComponent<KeyPointScript>()) == null)
        {
            t = t.parent;
        }

        if (keyPointScript == null)
        {
            Debug.LogError("TimerImageScript: KeyPointScript not found in parent");
        }
    }

    void Update()
    {
        if (keyPointScript != null)
        {
            image.fillAmount = keyPointScript.part;
            image.color = new Color(
                1 - image.fillAmount,
                image.fillAmount,
                0.2f
            );
        }
    }
}