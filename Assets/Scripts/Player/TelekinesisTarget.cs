using System;
using UnityEngine;

public class TelekinesisTarget : MonoBehaviour
{
    public Action levitatingObjectGottenTooFar;
    public LevitatableObject currentObject;

    [SerializeField] private float maxDistance = 10f;

    private void Update()
    {
        if (currentObject != null)
        {
            if (Vector3.Distance(transform.position, currentObject.transform.position) > maxDistance)
            {
                levitatingObjectGottenTooFar?.Invoke();
            }
        }
    }
}
