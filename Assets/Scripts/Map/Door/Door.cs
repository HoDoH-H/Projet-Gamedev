using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Transform door;
    [SerializeField] private float maxRotationToTheRight = 90f;
    [SerializeField] private float maxRotationToTheLeft = -90f;
    [SerializeField] private Vector3 currentRotation;

    private void Update()
    {
        currentRotation = Quaternion.Euler(door.localEulerAngles).eulerAngles;
        ControlRotation();
    }

    void ControlRotation()
    {
        if (door.localEulerAngles.y > 0 + maxRotationToTheRight && door.localEulerAngles.y < 0 + maxRotationToTheRight + 5)
        {
            Debug.Log("Max angle reached");
            door.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            door.Rotate(new Vector3(0, -1, 0), Space.Self);
        }
        else if (door.localEulerAngles.y > 360 - maxRotationToTheLeft)
        {
            Debug.Log("Min angle reached");
            door.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            door.Rotate(new Vector3(0, 1, 0), Space.Self);
        }
    }

    private void OnValidate()
    {
        ControlRotation();
    }
}
