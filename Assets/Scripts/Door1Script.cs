using UnityEngine;

public class Door1Script : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "Character")
        {
            ToastScript.ShowToast("Для відчинення дверей вам необхідно знайти ключ №1" 
            //+ Random.value
            );
        }
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
