using System.Linq;
using UnityEngine;

public class LightScript : MonoBehaviour
{
    private Light[] dayLights;
    private Light[] nightLights;
    private AudioSource dayAmbientSound;
    private AudioSource nightAmbientSound;
    private AudioSource switchLightSound;

    void Start()
    {
        dayLights = GameObject.FindGameObjectsWithTag("DayLight").Select(g => g.GetComponent<Light>()).ToArray();
        nightLights = GameObject.FindGameObjectsWithTag("NightLight").Select(g => g.GetComponent<Light>()).ToArray();

        AudioSource[] audioSources = GetComponents<AudioSource>();
        switchLightSound = audioSources[0];
        dayAmbientSound = audioSources[1];
        nightAmbientSound = audioSources[2];

        SwitchLight();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.N))
        {
            SwitchLight();
        }
    }

    private void SwitchLight()
    {
        switchLightSound.Play();
        GameState.isDay = !GameState.isDay;
        foreach (Light light in dayLights)
        {
            light.enabled = GameState.isDay;
        }
        foreach (Light light in nightLights)
        {
            light.enabled = !GameState.isDay;
        }

        if (GameState.isDay)
        {
            dayAmbientSound.Play();
            nightAmbientSound.Stop();
        }
        else
        {
            nightAmbientSound.Play();
            dayAmbientSound.Stop();
        }
    }
}