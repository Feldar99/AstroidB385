using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity3D : MonoBehaviour
{
    private float radiusSquared;

    public Transform ModelTransform;
    public float Gravity;

    private Vector3 velocity;
    private bool gravityEnabled = true;

    public float Radius
    {
        get { return Mathf.Sqrt(radiusSquared); }
    }

    // Use this for initialization
    public void Start()
    {
        radiusSquared = ModelTransform.localPosition.sqrMagnitude;
    }

    // Update is called once per frame
    public void FixedUpdate()
    {
        if (gravityEnabled && ModelTransform.localPosition.sqrMagnitude > radiusSquared + Mathf.Epsilon)
        {
            Vector3 toCenter = transform.position - ModelTransform.position;
            toCenter.Normalize();
            velocity += toCenter * Gravity * Time.deltaTime;
            SetPosition(ModelTransform.position + velocity * Time.deltaTime);
        }
        else
        {
            SetPosition(ModelTransform.position);
        }

        if (ModelTransform.localPosition.sqrMagnitude < radiusSquared - Mathf.Epsilon)
        {
            ModelTransform.localPosition = ModelTransform.localPosition.normalized * Mathf.Sqrt(radiusSquared);
            //modelRigidbody.velocity = Vector3.zero;
            velocity = Vector3.zero;
        }
    }

    public void SetPosition(Quaternion rotation,
        float distance)
    {
        transform.rotation = rotation;
        ModelTransform.position = transform.position + (transform.rotation * Vector3.up) * distance;
    }

    public void SetPosition(Vector3 position)
    {
        Vector3 toModel = position - transform.position;
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, toModel.normalized);
        SetPosition(rotation, toModel.magnitude);
    }

    public void SetGravityEnabled(bool gravityEnabled)
    {
        this.gravityEnabled = gravityEnabled;
    }
}