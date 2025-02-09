using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCam : MonoBehaviour
{
    [Header("References")]
    public ObjectOrientation orientation;
    public Character character;
    public CharacterGraphic characterGraphic;
    public Camera cam;
    public Rigidbody rb;
    public CombatLookAt combatLookAt;
    public CursorUI cursor;

    [Header("Camera Style")]
    public GameObject basicCamera;
    public GameObject combatCamera;

    private Vector2 moveInputValue;

    public float rotationSpeed = 7;
    private bool isGamePaused;

    public CameraStyle currentStyle;

    public void OnMove(InputValue value)
    {
        moveInputValue = value.Get<Vector2>();
    }

    public void OnPause(InputValue value)
    {
        isGamePaused = !isGamePaused;
        Cursor.lockState = isGamePaused ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !isGamePaused;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    private void Update()
    {
        // Rotate Orientation
        Vector3 viewDir = character.transform.position - new Vector3(cam.transform.position.x, character.transform.position.y, cam.transform.position.z);
        orientation.transform.forward = viewDir.normalized;

        if(currentStyle == CameraStyle.Basic)
        {
            Vector3 inputDir = orientation.transform.forward * moveInputValue.y + orientation.transform.right * moveInputValue.x;

            if (inputDir != Vector3.zero)
                characterGraphic.transform.forward = Vector3.Slerp(characterGraphic.transform.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
        }
        else if (currentStyle == CameraStyle.Combat)
        {
            Vector3 dirToCombatLookAt = combatLookAt.transform.position - new Vector3(cam.transform.position.x, combatLookAt.transform.position.y, cam.transform.position.z);
            orientation.transform.forward = dirToCombatLookAt.normalized;

            characterGraphic.transform.forward = dirToCombatLookAt.normalized;
        }
    }

    public void SwitchCameraStyle(CameraStyle style)
    {
        basicCamera.SetActive(style == CameraStyle.Basic);
        combatCamera.SetActive(style == CameraStyle.Combat);
        cursor.gameObject.SetActive(style == CameraStyle.Combat);

        currentStyle = style;
    }

    public GameObject CheckIfLookingAtALayer(LayerMask layer)
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 12.5f, layer))
        {
            return hit.collider.gameObject;
        }
        return null;
    }

    private void OnValidate()
    {
        SwitchCameraStyle(currentStyle);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(cam.transform.position, cam.transform.position + cam.transform.forward * 12.5f);
    }

    public enum CameraStyle
    {
        Basic,
        Combat
    }
}
