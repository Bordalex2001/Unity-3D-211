using UnityEngine;
using UnityEngine.InputSystem;

public class CameraScript : MonoBehaviour
{
    private InputAction lookAction;
    private Vector3 cameraAngles0;
    private Vector3 cameraAngles;
    private Transform character;
    private Vector3 r;
    private float sensitivityH = 5.0f;
    private float sensitivityW = -3.0f;
    private float minFpvDistance = 0.9f;
    private float maxFpvDistance = 9.0f;
    private bool isPos3;

    void Start()
    {
        lookAction = InputSystem.actions.FindAction("Look");
        cameraAngles0 = cameraAngles = transform.eulerAngles;
        character = GameObject.Find("Character").transform;
        r = transform.position - character.position;
        isPos3 = false;
    }

    void Update()
    {
        Vector2 wheel = Input.mouseScrollDelta;
        if(wheel.y != 0)
        {
            if (r.magnitude >= minFpvDistance)
            {
                isPos3 = true;
                if (wheel.y > 0)
                {
                    r *= 1 - wheel.y / 10;
                }
            }
            else
            {
                isPos3 = false;
                if (r.magnitude >= minFpvDistance)
                {

                }
            }
        }
        Vector2 lookValue = lookAction.ReadValue<Vector2>();
        if (lookValue != Vector2.zero)
        {
            cameraAngles.x += lookValue.y * Time.deltaTime * sensitivityH;
            cameraAngles.y += lookValue.x * Time.deltaTime * sensitivityW;
            transform.eulerAngles = cameraAngles;
        }
        transform.position = character.position + Quaternion.Euler(0, cameraAngles.y, 0) * r;
    }
}
