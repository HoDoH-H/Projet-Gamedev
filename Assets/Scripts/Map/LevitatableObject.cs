using UnityEngine;

public class LevitatableObject : MonoBehaviour
{
    [Header("References")]
    public Rigidbody rb;
    public Transform graphic;

    [Header("Settings")]
    public bool isGlowing = false;
    public bool isLevitating = false;
    private Vector3 centerOffset;

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

    public void SetGlowing(float intensity = 0)
    {
        graphic.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.white * intensity);
    }
}
