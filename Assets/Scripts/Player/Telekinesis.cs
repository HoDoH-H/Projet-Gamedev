using UnityEngine;
using UnityEngine.InputSystem;

public class Telekinesis : MonoBehaviour
{
    [Header("References")]
    public ThirdPersonCam cam;
    public TelekinesisTarget target;

    [Header("Settings")]
    public LayerMask levitatableLayer;
    public float throwForce = 10f;

    [Header("Debug")]
    [SerializeField] private LevitatableObject currentObject;
    private bool doMoveToTarget = true;

    private void Start()
    {
        target.levitatingObjectGottenTooFar += ClearCurrentObject;
    }

    public void OnInteract(InputValue value)
    {
        if (value.isPressed)
        {
            if (currentObject == null)
            {
                GameObject obj = cam.CheckIfLookingAtALayer(levitatableLayer);
                if (obj == null)
                    return;

                SetCurrentObject(obj.GetComponentInParent<LevitatableObject>());

                Door isDoor = currentObject.GetComponent<Door>();
                if (isDoor != null)
                    StartCoroutine(isDoor.Break());

                doMoveToTarget = true;
                currentObject.rb.useGravity = false;
            }
            else
            {
                ClearCurrentObject();
            }
        }
    }

    public void OnAttack(InputValue value)
    {
        if (value.isPressed)
        {
            if (currentObject == null)
                return;

            doMoveToTarget = !doMoveToTarget;
            currentObject.rb.useGravity = true;
            currentObject.rb.AddForce(cam.cam.transform.forward * throwForce, ForceMode.Impulse);
            SetCurrentObject(null);
        }
    }

    void ClearCurrentObject()
    {
        if(currentObject == null)
            return;

        currentObject.rb.useGravity = true;
        SetCurrentObject(null);
    }

    void SetCurrentObject(LevitatableObject obj)
    {
        currentObject = obj;
        target.currentObject = obj;
    }

    private void Update()
    {
        if (doMoveToTarget)
            currentObject?.MoveObjectTowardTarget(target.transform);
    }
}
