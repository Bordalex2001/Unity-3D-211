using UnityEngine;

public class BatteryScript : MonoBehaviour
{
    [SerializeField]
    private float batteryCharge = 0.5f;
    [SerializeField]
    private bool isRandomCharge = false;
    private AudioSource collectSound;
    private float destroyTimeout;

    void Start()
    {
        collectSound = GetComponent<AudioSource>();
        destroyTimeout = 0f;
    }

    void Update()
    {
        if (destroyTimeout > 0f)
        {
            destroyTimeout -= Time.deltaTime;
            if (destroyTimeout <= 0f)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isRandomCharge) batteryCharge = Random.Range(0.3f, 1.0f);
        if (other.gameObject.CompareTag("Player"))
        {
            collectSound.Play();
            GameState.TriggerGameEvent(
                "Battery",
                new GameEvents.MessageEvent
                {
                    message = $"Заряд ліхтарика збільшено на {batteryCharge:F1}",
                    data = batteryCharge
                }
            );

            FlashLightState.charge += batteryCharge;
            destroyTimeout = .6f;
        }
    }
}