using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Inhabitant : MonoBehaviour
{
    private float radiusSquared;
    private Rigidbody modelRigidbody;

    public Transform ModelTransform;
    public float Gravity;

    private Vector3 velocity;
    private bool gravityEnabled = true;

    // Use this for initialization
    public void Start()
    {
        radiusSquared = ModelTransform.localPosition.sqrMagnitude;
        modelRigidbody = ModelTransform.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    public void FixedUpdate()
    {
        if (gravityEnabled && ModelTransform.localPosition.sqrMagnitude > radiusSquared + Mathf.Epsilon)
        {
            Vector3 toCenter = transform.position - ModelTransform.position;
            toCenter.Normalize();
            velocity += toCenter * Gravity * Time.deltaTime;
            Debug.Log("Velocity: " + velocity);
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
        Debug.Log("Euler Angles:" + rotation.eulerAngles + "Distance: " + distance);
        //Debug.Log(distance);
        transform.rotation = rotation;
        Debug.Log(transform.rotation.eulerAngles);
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