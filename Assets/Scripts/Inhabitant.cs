using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inhabitant : MonoBehaviour
{
    private float radiusSquared;

    public Transform ModelTransform;
    public float Gravity;
    public float HappinessChangeRate;
    public float MaxHappiness;
    public GameObject HappinessIndicator;
    public Color UnhappyColor = Color.red;
    public Color NeutralColor = Color.yellow;
    public Color HappyColor = Color.green;
    public float PickupPenalty = 0.25f;

    private Vector3 velocity;
    private bool held;
    private ClimateControl climateControl;
    private float happiness;
    private HappinessManager happinessManager;

    public float Radius
    {
        get { return Mathf.Sqrt(radiusSquared); }
    }

    public float Happiness
    {
        get { return happiness; }
    }

    public bool Neutral
    {
        get { return happiness > HappinessManager.UnhappyThreshold && happiness < HappinessManager.HappyThreshold; }
    }

    public bool Unhappy
    {
        get { return happiness <= HappinessManager.UnhappyThreshold && !Angry; }
    }

    public bool Happy
    {
        get { return happiness >= HappinessManager.HappyThreshold && !Joyous; }
    }

    public bool Angry
    {
        get { return happiness <= -MaxHappiness + Mathf.Epsilon; }
    }

    public bool Joyous
    {
        get { return happiness >= MaxHappiness - Mathf.Epsilon; }
    }

    // Use this for initialization
    public void Start()
    {
        radiusSquared = ModelTransform.localPosition.sqrMagnitude;
        climateControl = GetComponentInParent<ClimateControl>();
        happinessManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<HappinessManager>();
        happinessManager.RegisterInhabitant(this);
    }

    // Update is called once per frame
    public void FixedUpdate()
    {
        if (!held && ModelTransform.localPosition.sqrMagnitude > radiusSquared + Mathf.Epsilon)
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

        float life = climateControl.GetTerrainLife(ModelTransform.position);
        if (held)
        {
            life = -PickupPenalty;
        }
        //Debug.Log("Life: " + life);

        happiness += (life - 0.5f) * HappinessChangeRate * Time.deltaTime * 2;
        happiness = Mathf.Clamp(happiness, -MaxHappiness, MaxHappiness);
        //Debug.Log("Happiness: " + happiness);

        float scaledHappiness = happiness / MaxHappiness;
        //Debug.Log("Scaled Happiness: " + scaledHappiness);
        Color happinessColor;
        if (scaledHappiness < 0)
        {
            happinessColor = Color.Lerp(NeutralColor, UnhappyColor, -scaledHappiness);
        }
        else
        {
            happinessColor = Color.Lerp(NeutralColor, HappyColor, scaledHappiness);
        }

        HappinessIndicator.GetComponent<Renderer>().material.SetColor("_EmissionColor", happinessColor);

        happinessManager.ReportHappiness(this);
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

    public void SetHeld(bool held)
    {
        this.held = held;
    }
}