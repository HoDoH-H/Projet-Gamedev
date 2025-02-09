using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private BoxCollider box;
    [SerializeField] private float maxRotationToTheRight = 90f;
    [SerializeField] private float maxRotationToTheLeft = -90f;
    [SerializeField] private float maxRotationSpeedToAutoClose = 0.6f;

    [Header("Debug")]
    [SerializeField] private bool doBreak = false;

    private void Update()
    {
        ControlRotation();
        StartCoroutine(Break(doBreak));
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

    public IEnumerator Break(bool doBreak = true)
    {
        if(doBreak && transform.parent != null)
        {
            gameObject.layer = LayerMask.NameToLayer("levitatableObjects");
            rb.constraints = RigidbodyConstraints.None;
            rb.automaticInertiaTensor = true;
            transform.parent = null;
            rb.automaticCenterOfMass = true;
            box.excludeLayers = 0;

            Collider[] col = Physics.OverlapBox(transform.position, box.size / 2, transform.rotation, LayerMask.GetMask("whatIsWall"));
            
            if(col.Length != 0)
            {
                box.excludeLayers = LayerMask.GetMask("whatIsWall");
                while (col.Length != 0)
                {
                    col = Physics.OverlapBox(transform.position, box.size / 2, transform.rotation, LayerMask.GetMask("whatIsWall"));
                    yield return new WaitForSeconds(0.05f);
                }
                box.excludeLayers = 0;
            }
            
            this.enabled = false;
        }
    }

    private void OnValidate()
    {
        ControlRotation();
    }
}
