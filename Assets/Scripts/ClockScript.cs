using UnityEngine;

public class ClockScript : MonoBehaviour
{
    private TMPro.TextMeshProUGUI clock;
    private float gameTime;
    
    void Start()
    {
        clock = GetComponent<TMPro.TextMeshProUGUI>();
        gameTime = 0f;
    }
    
    void Update()
    {
        gameTime += Time.deltaTime;

        int hours = (int)gameTime / 3600;
        int minutes = (int)gameTime / 60 % 60;
        int seconds = (int)gameTime % 60;

        clock.text = $"{hours:D2}:{minutes:D2}:{seconds:D2}";
    }
}