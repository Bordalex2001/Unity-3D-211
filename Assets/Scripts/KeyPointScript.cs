using UnityEngine;

public class KeyPointScript : MonoBehaviour
{
    [SerializeField]
    private string keyPointName = "1";
    [SerializeField]
    private float timeout = 5.0f;
    [SerializeField]
    private int room = -1;
    private float leftTime;
    private AudioSource successSound;
    private AudioSource failureSound;

    public float part { get; private set; }

    void Start()
    {
        leftTime = timeout;
        part = 1.0f;

        AudioSource[] audioSources = GetComponents<AudioSource>();
        successSound = audioSources[0];
        failureSound = audioSources[1];

        successSound.volume = GameState.effectsVolume;
        failureSound.volume = GameState.effectsVolume;
        GameState.Subscribe(OnSoundsVolumeTrigger, "EffectsVolume");
    }

    void Update()
    {
        if (leftTime > 0 && GameState.room == room)
        {
            leftTime -= Time.deltaTime;
            if (leftTime < 0) leftTime = 0;
            part = leftTime / timeout;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            bool isSuccess = leftTime > 0;

            if (isSuccess)
            {
                successSound.Play();
            }
            else
            {
                failureSound.Play();
            }
            
            GameState.collectedItems.Add("Key" + keyPointName, part);
            GameState.TriggerGameEvent(
                "KeyPoint",
                new GameEvents.MessageEvent
                {
                    message = "Підібрано ключ " + keyPointName,
                    data = part
                }
            );

            Destroy(gameObject, .6f);
        }
    }

    private void OnSoundsVolumeTrigger(string eventName, object data)
    {
        if (eventName == "EffectsVolume")
        {
            successSound.volume = (float)data;
            failureSound.volume = (float)data;
        }
    }

    private void OnDestroy()
    {
        GameState.Unsubscribe(OnSoundsVolumeTrigger, "EffectsVolume");
    }
}