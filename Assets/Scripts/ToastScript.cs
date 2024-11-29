using System.Collections.Generic;
using UnityEngine;

public class ToastScript : MonoBehaviour
{
    private static ToastScript instance;
    private TMPro.TextMeshProUGUI toastTMP;
    private float timeout = 5.0f;
    private float leftTime;
    private GameObject content;
    private readonly Queue<ToastMessage> messages = new Queue<ToastMessage>();

    public static void ShowToast(string message, float? timeout = null)
    {
        if (instance.messages.Count > 0 &&
            instance.messages.Peek().message == message)
        {
            return;
        }
        instance.messages.Enqueue(new ToastMessage
        {
            message = message,
            timeout = timeout ?? instance.timeout
        });
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (leftTime > 0)
        {
            leftTime -= Time.deltaTime;
            if (leftTime <= 0)
            {
                messages.Dequeue();
                content.SetActive(false);
            }
        }
        else
        {
            if (messages.Count > 0)
            {
                ToastMessage m = messages.Peek();
                toastTMP.text = m.message;
                leftTime = m.timeout;
                content.SetActive(true);
            }
        }
    }

    private class ToastMessage
    {
        public string message { get; set; }
        public float timeout { get; set; }
    }
}