using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private BoxCollider box;
    [SerializeField] private MeshCollider col;
    [SerializeField] private float maxRotationToTheRight = 90f;
    [SerializeField] private float maxRotationToTheLeft = -90f;
    [SerializeField] private float maxRotationSpeedToAutoClose = 0.6f;

    [Header("Debug")]
    [SerializeField] private bool doBreak = false;

    private void Update()
    {
        ControlRotation();
        Break(doBreak);
    }

    void ControlRotation()
    {
        if (transform.localEulerAngles.y > maxRotationToTheRight && transform.localEulerAngles.y < 300 + maxRotationToTheLeft)
        {
            rb.angularVelocity = Vector3.zero;
            transform.Rotate(new Vector3(0, -1, 0), Space.Self);
        }
        else if(transform.localEulerAngles.y < 360 + maxRotationToTheLeft && transform.localEulerAngles.y > maxRotationToTheRight + 30)
        {
            rb.angularVelocity = Vector3.zero;
            transform.Rotate(new Vector3(0, 1, 0), Space.Self);
        }
        else if(transform.localEulerAngles.y < 2 && Mathf.Abs(transform.GetComponent<Rigidbody>().angularVelocity.y) < maxRotationSpeedToAutoClose || transform.localEulerAngles.y > 358 && Mathf.Abs(transform.GetComponent<Rigidbody>().angularVelocity.y) < maxRotationSpeedToAutoClose)
        {
            transform.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
    }

    public void Break(bool doBreak = true)
    {
        if(doBreak && transform.parent != null)
        {
            rb.constraints = RigidbodyConstraints.None;
            transform.parent = null;
            rb.automaticCenterOfMass = true;
            col.excludeLayers = 0;
        }
    }

    private void OnValidate()
    {
        ControlRotation();
    }
}
