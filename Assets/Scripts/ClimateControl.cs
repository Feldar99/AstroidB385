using System.Collections;
using System.Collections.Generic;
using NewtonVR;
using UnityEngine;

public class ClimateControl : MonoBehaviour
{
    private const float MIN_NOTICIBLE_DECAY_INTERVAL = 0.1f;

    public GameObject paintObject, cursor;
    public Color barrenColor, vibrantColor;
    public int brushSize = 20;
    public float decayRate = 1;
    public float decayInterval = 0.2f;


    private Color[] climate;
    private NVRHand cursorHand;
    private float timeElapsedSinceLastDecay;
    private int interlaceFactor;
    private int interlaceIndex;
    private AudioSource rainSound;
    private ParticleSystem rainParticles;

    void Start()
    {
        cursorHand = cursor.GetComponent<NVRHand>();
        interlaceFactor = (int) Mathf.Ceil(decayInterval / MIN_NOTICIBLE_DECAY_INTERVAL);
        rainSound = cursor.GetComponentInChildren<AudioSource>();
        rainParticles = cursor.GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsedSinceLastDecay += Time.deltaTime;
        if (timeElapsedSinceLastDecay >= decayInterval)
        {
            Texture2D tex = paintObject.GetComponent<Renderer>().material.mainTexture as Texture2D;
            climate = tex.GetPixels();
            for (int i = interlaceIndex; i < climate.Length; i += interlaceFactor)
            {
                climate[i] = Color.Lerp(climate[i], barrenColor, timeElapsedSinceLastDecay * decayRate);
            }
            tex.SetPixels(climate);
            tex.Apply();

            timeElapsedSinceLastDecay = 0;
            interlaceIndex = (interlaceIndex + 1) % interlaceFactor;
        }
        if (cursorHand.UseButtonPressed)
        {
            Paint(cursor.transform.position);
            if (!rainParticles.isPlaying)
            {
                rainParticles.Play();
            }
        }
        else
        {
            rainParticles.Stop();
        }
        rainSound.mute = !cursorHand.UseButtonPressed;
    }

    void Paint(Vector3 cursorPos)
    {
        Texture2D tex = paintObject.GetComponent<Renderer>().material.mainTexture as Texture2D;
        Vector3 uvWorldPosition = Vector3.zero;
        RaycastHit hit;
        Ray cursorRay = new Ray(cursorPos, paintObject.transform.position - cursorPos);
        if (Physics.Raycast(cursorRay, out hit, 200))
        {
            Vector2 pixelUV = hit.textureCoord;
            rainSound.transform.position = hit.point;
            pixelUV.x *= tex.width;
            pixelUV.y *= tex.height;
            Color[] colors = new Color[brushSize * brushSize];
            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = vibrantColor;
            }
            //Debug.Log(pixelUV);
            tex.SetPixels((int) pixelUV.x - brushSize / 2, (int) pixelUV.y - brushSize / 2, brushSize, brushSize, colors);

            tex.Apply();
        }
    }

    public Color GetColor(Vector3 position)
    {
        Texture2D tex = paintObject.GetComponent<Renderer>().material.mainTexture as Texture2D;
        RaycastHit hit;
        Ray cursorRay = new Ray(position, paintObject.transform.position - position);
        if (Physics.Raycast(cursorRay, out hit, 200))
        {
            Vector2 pixelUV = hit.textureCoord;
            rainSound.transform.position = hit.point;
            return tex.GetPixelBilinear(pixelUV.x, pixelUV.y);
        }

        return barrenColor;
    }

    public float GetTerrainLife(Vector3 position)
    {
        Color groundColor = GetColor(position);
        //Debug.Log("Ground Color: " + groundColor);

        Color difference = groundColor - barrenColor;

        if (difference == Color.black)
        {
            return 0;
        }

        if (Mathf.Abs(difference.r) > Mathf.Epsilon)
        {
            return difference.r / (vibrantColor.r - barrenColor.r);
        }
        if (Mathf.Abs(difference.g) > Mathf.Epsilon)
        {
            return difference.g / (vibrantColor.g - barrenColor.g);
        }
        if (Mathf.Abs(difference.b) > Mathf.Epsilon)
        {
            return difference.b / (vibrantColor.b - barrenColor.b);
        }
        return difference.a / (vibrantColor.a - barrenColor.a);
    }
}