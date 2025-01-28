using UnityEngine;

public class BatteryScript : MonoBehaviour
{
    [SerializeField]
    private float batteryCharge = 0.5f;
    [SerializeField]
    private bool isRandomCharge = false;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isRandomCharge) batteryCharge = Random.Range(0.3f, 1.0f);
        if (other.gameObject.CompareTag("Player"))
        {
            GameState.TriggerGameEvent(
                "Battery",
                new GameEvents.MessageEvent
                {
                    message = $"Заряд ліхтарика збільшено на {batteryCharge:F1}",
                    data = batteryCharge
                }
            );
            FlashLightState.charge += batteryCharge;
            Destroy(gameObject);
        }
    }
}