using UnityEngine;
using UnityEngine.InputSystem;

public class CameraScript : MonoBehaviour
{
    private InputAction lookAction;
    private Vector3 cameraAngles, cameraAngles0;
    private Transform character;
    private Vector3 r;
    private float minFpvDistance = 0.9f;
    private float maxFpvDistance = 9.0f;
    private float maxDistanceToCharacter = 20.0f;
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
            float currentMaxDistance = isPos3 ? maxFpvDistance : maxDistanceToCharacter;

            if (r.magnitude >= currentMaxDistance)
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
                        GameState.isFpv = true;
                    }
                    else
                    {
                        r *= 1 - wheel.y / 10;
                    }
                }
                else
                {
                    if (wheel.y < 0)
                    {
                        r *= 100f;
                        GameState.isFpv = false;
                    }
                }
            }
        }
        if (!isPos3)
        {
            Vector2 lookValue = lookAction.ReadValue<Vector2>();
            if (lookValue != Vector2.zero)
            {
                cameraAngles.x += lookValue.y * Time.deltaTime * GameState.lookSensitivityY;
                cameraAngles.y += lookValue.x * Time.deltaTime * GameState.lookSensitivityX;

                //Обмеження зміни кута камери за вертикаллю у залежності від режиму її роботи
                if (isPos3) //режим FPV
                {
                    cameraAngles.x = Mathf.Clamp(cameraAngles.x, -10f, 40f);
                }
                else //Звичайний режим
                {
                    cameraAngles.x = Mathf.Clamp(cameraAngles.x, 35f, 75f);
                }

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