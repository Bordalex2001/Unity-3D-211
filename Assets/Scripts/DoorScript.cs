using UnityEngine;

public class DoorScript : MonoBehaviour
{
    [SerializeField]
    private string requiredKey = "1";
    private float openingTime = 3.0f;
    private float timeout = 0f;
    private float openedPart = 0.5f; //частина, при відкритті якої перемикається кімната (room)
    private bool isClosed = true;
    private AudioSource closedSound;
    private AudioSource openSound;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && isClosed)
        {
            if (GameState.collectedItems.ContainsKey("Key" + requiredKey))
            {
                bool isOnTime = (float)GameState.collectedItems["Key" + requiredKey] > 0;
                GameState.TriggerGameEvent(
                    "Door:", 
                    new GameEvents.MessageEvent { 
                        message = "Двері відчиняються " + (isOnTime ? "швидко" : "повільно"),
                        data = requiredKey
                    }
                );

                if (!isOnTime)
                {
                    openingTime *= 3;
                }

                timeout = 1.0f;
                isClosed = false;
                openSound.Play();
            }
            else
            {
                GameState.TriggerGameEvent(
                    "Door1:",
                    new GameEvents.MessageEvent { 
                        message = "Для відчинення дверей вам треба підібрати ключ " + requiredKey,
                        data = requiredKey
                    }
                );

                closedSound.Play();
            }
        }
    }

    void Start()
    {
        AudioSource[] audioSources = GetComponents<AudioSource>();
        closedSound = audioSources[0];
        openSound = audioSources[1];

        closedSound.volume = GameState.effectsVolume;
        openSound.volume = GameState.effectsVolume;
        GameState.Subscribe(OnSoundsVolumeTrigger, "EffectsVolume");
    }

    void Update()
    {
        if (timeout > 0f)
        {
            float t = Time.deltaTime / openingTime;
            transform.Translate(t, 0, 0);

            if (timeout >= openedPart && timeout - t < openedPart)
            {
                GameState.room++;
            }
            timeout -= t;
        }
    }

    private void OnSoundsVolumeTrigger(string eventName, object data)
    {
        if (eventName == "EffectsVolume")
        {
            closedSound.volume = (float)data;
            openSound.volume = (float)data;
        }
    }

    private void OnDestroy()
    {
        GameState.Unsubscribe(OnSoundsVolumeTrigger, "EffectsVolume");
    }
}