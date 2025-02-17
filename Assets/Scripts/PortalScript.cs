using UnityEngine;

public class PortalScript : MonoBehaviour
{
    private Transform exitTransform;
    
    void Start()
    {
        exitTransform = transform.Find("Exit");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.transform.position = exitTransform.position;
            GameState.TriggerGameEvent("Portal", new GameEvents.MessageEvent
            {
                message = "Ви телепортовані"
            });
        }
    }
}