using UnityEngine;

public class Door1Script : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "Character")
        {
            ToastScript.ShowToast(
                message: "Для відчинення дверей вам треба підібрати ключ", 
                author: "Двері 1"
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
