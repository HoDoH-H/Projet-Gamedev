using UnityEngine;

public class LevitatableObject : MonoBehaviour
{
    [Header("References")]
    public Rigidbody rb;
    public Transform graphic;

    [Header("Settings")]
    public float groundDrag = 10f;
    public float groundDistance = 0.4f;
    public bool isGlowing = false;
    public bool isLevitating = false;
    public LayerMask whatIsGround;
    private Vector3 centerOffset;

    public bool isGrounded;

    private void Start()
    {
        Renderer renderer = graphic.GetComponent<Renderer>();
        if (renderer != null)
        {
            centerOffset = renderer.bounds.center - graphic.transform.position;
        }
    }

    private void Update()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundDistance, whatIsGround);
        rb.linearDamping = isGrounded ? groundDrag : 0.5f;

        SetGlowing(isGlowing ? 4 : 1);
    }

    public void MoveObjectTowardTarget(Transform target)
    {
        //Vector3 dirToTarget = target.position - transform.position;
        Vector3 dirToTarget = target.position - (transform.position + transform.TransformVector(centerOffset));

        float stiffness = 100f;
        float damping = 5f;

        Vector3 force = (dirToTarget * stiffness) - (rb.linearVelocity * damping);
        rb.AddForce(force, ForceMode.Force);
        rb.angularVelocity = rb.angularVelocity * 0.9f;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + groundDistance * Vector3.down);
    }

    public void SetGlowing(float intensity = 0)
    {
        graphic.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.white * intensity);
    }
}
