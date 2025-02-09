using UnityEngine;

public class Object : MonoBehaviour
{
    [Header("References")]
    public Rigidbody rb;
    public Transform graphic;

    [Header("Settings")]
    public float groundDrag = 10f;
    public float groundDistance = 0.4f;
    public LayerMask whatIsGround;

    public bool isGrounded;

    private void Update()
    {
        isGrounded = Physics.Raycast(rb.transform.position -(Vector3.up * 0.5f), Vector3.down, groundDistance, whatIsGround);
        rb.linearDamping = isGrounded ? groundDrag : 0f;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(graphic.position, Vector3.up * 0.2f + graphic.position + groundDistance * Vector3.down);
    }
}
