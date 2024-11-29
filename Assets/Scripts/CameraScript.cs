using UnityEngine;
using UnityEngine.InputSystem;

public class CameraScript : MonoBehaviour
{
    private InputAction lookAction;
    private Vector3 cameraAngles, cameraAngles0;
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
        if (wheel.y != 0)
        {
            if (r.magnitude >= maxFpvDistance)
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
                    float rr = r.magnitude * (1 - wheel.y / 10);
                    if (rr <= minFpvDistance)
                    {
                        r *= 0.01f;
                    }
                    else
                    {
                        r *= (1 - wheel.y / 10);
                    }
                }
                else
                {
                    if (wheel.y < 0)
                    {
                        r *= 100f;
                    }
                }
            }
        }
        if (!isPos3)
        {
            Vector2 lookValue = lookAction.ReadValue<Vector2>();
            if (lookValue != Vector2.zero)
            {
                cameraAngles.x += lookValue.y * Time.deltaTime * sensitivityW;
                cameraAngles.y += lookValue.x * Time.deltaTime * sensitivityH;

                //Обмеження зміни кута камери за вертикаллю за рухом миші                           
                cameraAngles.x = Mathf.Clamp(cameraAngles.x, 35f, 75f);

                transform.eulerAngles = cameraAngles;
            }
            transform.position = character.position +
                Quaternion.Euler(
                    cameraAngles.x - cameraAngles0.x,
                    cameraAngles.y - cameraAngles0.y,
                    0
                ) * r;
        }
    }
}