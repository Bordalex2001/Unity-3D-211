using UnityEngine;
using UnityEngine.InputSystem;

public class NewMonoBehaviourScript : MonoBehaviour
{
    private Rigidbody rb;
    private InputAction moveAction;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        moveAction = InputSystem.actions.FindAction("Move");
    }

    void Update()
    {
        // Projections:
        Vector3 f = Camera.main.transform.forward;
        f.y = 0.0f;
        if (f == Vector3.zero)
        {
            f = Camera.main.transform.up;
            f.y = 0.0f;
        }
        f.Normalize();
        Vector3 r = Camera.main.transform.right;
        r.y = 0.0f;
        r.Normalize();

        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        rb.AddForce(250 * Time.deltaTime * (r * moveValue.x + f * moveValue.y));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameObject.FindWithTag("Battery"))
        {
            FlashLightState.charge += 1.0f;
            Destroy(other.gameObject);
        }
    }
}