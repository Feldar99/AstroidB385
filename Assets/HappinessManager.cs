using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HappinessManager : MonoBehaviour
{
    public List<Inhabitant> inhabitants;
    //private int inhabitantCount;
    public float globalHappiness;
    private float musicTimer;

    public float HappinessChangeRate = 10;
    public float MusicDelay = 120;

    public AudioSource UnhappyMusic;
    public AudioSource NeutralMusic;
    public AudioSource HappyMusic;

    public static float UnhappyThreshold = -50;
    public static float HappyThreshold = 50;
    public static float MaxUnhappy = -100;
    public static float MaxHappy = 100;

	// Use this for initialization
	void Start ()
	{
	    musicTimer = MusicDelay / 2;

	}
	
	// Update is called once per frame
	void Update ()
	{
	    if (!HappyMusic.isPlaying && !UnhappyMusic.isPlaying && !NeutralMusic.isPlaying)
	    {
	        musicTimer -= Time.deltaTime;
	    }

	    if (UnhappyMusic.isPlaying && globalHappiness >= 0)
	    {
	        UnhappyMusic.Stop();
	    }
        else if (HappyMusic.isPlaying && globalHappiness <= 0)
	    {
	        HappyMusic.Stop();
	    }

	    if (musicTimer <= 0)
	    {

            if (globalHappiness <= UnhappyThreshold)
            {
                UnhappyMusic.Play();
            }
            else if (globalHappiness >= HappyThreshold)
            {
                HappyMusic.Play();
            }
            else
            {
                NeutralMusic.Play();
            }

	        musicTimer = MusicDelay;
	    }

        /*
	    int joyCount = 0;
	    foreach (Inhabitant inhabitant in inhabitants)
	    {
	        if (inhabitant.Joyous)
	            joyCount++;
	    }
	    if (joyCount == inhabitants.Count)
	    {
	        
	    }
        */
	}

    public void RegisterInhabitant(Inhabitant inhabitant)
    {
        inhabitants.Add(inhabitant);
        //inhabitantCount++;
    }

    public void ReportHappiness(Inhabitant inhabitant)
    {
        globalHappiness += inhabitant.Happiness / inhabitants.Count * Time.deltaTime * HappinessChangeRate;
    }
}
