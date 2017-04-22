using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inhabitant : MonoBehaviour
{
    private float radiusSquared;
    public Transform ModelTransform;
    public float Gravity;

    // Use this for initialization
    public void Start()
    {
        radiusSquared = ModelTransform.localPosition.sqrMagnitude;
    }

    // Update is called once per frame
    public void Update()
    {
        if (ModelTransform.localPosition.sqrMagnitude > radiusSquared)
        {
            Rigidbody modelRigidbody = ModelTransform.GetComponent<Rigidbody>();
            modelRigidbody.velocity -= ModelTransform.localPosition.normalized * Gravity * Time.deltaTime;
        }

        if (ModelTransform.localPosition.sqrMagnitude < radiusSquared)
        {
            ModelTransform.localPosition = ModelTransform.localPosition.normalized * Mathf.Sqrt(radiusSquared);
        }
    }
}