using System.Collections.Generic;
using UnityEngine;

public class ToastScript : MonoBehaviour
{
    private static ToastScript instance;
    private TMPro.TextMeshProUGUI toastTMP;
    private readonly float timeout = 5.0f;
    private float leftTime;
    private GameObject content;
    private readonly Queue<ToastMessage> messages = new Queue<ToastMessage>();

    public static void ShowToast(string message, string author = null, float? timeout = null)
    {
        foreach (ToastMessage m in instance.messages)
        {
            if (m.author == author && m.message == message)
            {
                return;
            }
        }

        instance.messages.Enqueue(new ToastMessage
        { 
            author = author,
            message = message, 
            timeout = timeout ?? instance.timeout 
        });
    }

    void Start()
    {
        instance = this;
        content = transform.Find("Content").gameObject;
        toastTMP = transform.Find("Content/ToastTMP").GetComponent<TMPro.TextMeshProUGUI>();
        content.SetActive(false);
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
                toastTMP.text = !string.IsNullOrEmpty(m.author) ?
                    $"{m.author}: {m.message}" : m.message;
                leftTime = m.timeout;
                content.SetActive(true);
            }
        }
    }

    private class ToastMessage
    {
        public string author { get; set; }
        public string message { get; set; }
        public float timeout { get; set; }
    }
}