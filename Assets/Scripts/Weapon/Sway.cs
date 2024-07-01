using UnityEngine;

public class Sway : MonoBehaviour
{
    [SerializeField] private float swayClamp = 0.09f;
    [SerializeField] private float smoothing = 3f;

    private Vector3 origin, target;
    private Vector2 input;

    private void Start()
    {
        origin = transform.localPosition;
    }

    private void Update()
    {
        input = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        input.x = Mathf.Clamp(input.x, -swayClamp, swayClamp);
        input.y = Mathf.Clamp(input.y, -swayClamp, swayClamp);

        target = new Vector3(-input.x, -input.y, 0);
        transform.localPosition = Vector3.Lerp(transform.localPosition, target + origin, Time.deltaTime * smoothing);
    }
}
