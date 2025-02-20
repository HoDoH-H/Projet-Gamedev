using System.Collections;
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


    public static Telekinesis instance;

    private void Awake()
    {
        instance = this;
    }

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
                // Check if looking at a levitatable object
                GameObject obj = cam.CheckIfLookingAtALayer(levitatableLayer);
                if (obj == null)
                    return;

                // Take Item
                PlayerMovement.instance.characterAnimator.SetTrigger("Take");
                StartCoroutine(Take(obj.GetComponentInParent<LevitatableObject>()));
                StartCoroutine(PlayerMovement.instance.DeactivateMovementFor(0.6f));
            }
            else
            {
                // Drop Item

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

            PlayerMovement.instance.characterAnimator.SetTrigger("Throw");
            StartCoroutine(Throw());
            StartCoroutine(PlayerMovement.instance.DeactivateMovementFor(1.2f));
        }
    }

    IEnumerator Throw()
    {
        yield return new WaitForSeconds(0.5f);

        doMoveToTarget = !doMoveToTarget;
        currentObject.rb.useGravity = true;
        currentObject.rb.AddForce(cam.cam.transform.forward * throwForce, ForceMode.Impulse);
        currentObject.isLevitating = false;
        SetCurrentObject(null);
    }

    IEnumerator Take(LevitatableObject obj)
    {
        if (obj == null)
            yield break;

        yield return new WaitForSeconds(0.5f);

        SetCurrentObject(obj);

        // If object is door, break it
        Door isDoor = currentObject.GetComponent<Door>();
        if (isDoor != null)
            StartCoroutine(isDoor.Break());

        doMoveToTarget = true;
        currentObject.rb.useGravity = false;
        currentObject.isLevitating = true;
    }

    void ClearCurrentObject()
    {
        if(currentObject == null)
            return;

        currentObject.isLevitating = false;
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
