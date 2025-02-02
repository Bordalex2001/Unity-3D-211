using UnityEngine;

public class SpinScript : MonoBehaviour
{
    [SerializeField]
    private float period = 2.0f;
    [SerializeField]
    private bool x = false;
    [SerializeField]
    private bool y = true;
    [SerializeField]
    private bool z = false;

    void Update()
    {
        transform.Rotate(
            x ? Time.deltaTime / period * 360 : 0,
            y ? Time.deltaTime / period * 360 : 0,
            z ? Time.deltaTime / period * 360 : 0,
            Space.World
        );
    }
}