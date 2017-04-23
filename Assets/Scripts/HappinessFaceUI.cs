using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HappinessFaceUI : MonoBehaviour {
    public Sprite happyFace, angryFace, neutralFace, rageFace, joyFace;

    public Inhabitant WatchedInhabitant;

    private HappinessManager happinessManager;

    private void Start() {
        happinessManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<HappinessManager>();
    }

    public void Update()
    {
        //float joyThreshold = HappinessManager.MaxHappy * .8f;
        //if (happinessManager.globalHappiness > joyThreshold)
        //{
        //    SwitchToJoyFace();
        //    return;
        //}


        if (WatchedInhabitant)
        {
            //if (WatchedInhabitant.Angry)
            //{
            //    SwitchToRageFace();
            //}
            if (WatchedInhabitant.Unhappy || WatchedInhabitant.Angry)
            {
                SwitchToAngryFace();
            }
            else if (WatchedInhabitant.Happy)
            {
                SwitchToHappyFace();
            }
            else if (WatchedInhabitant.Joyous)
            {
                SwitchToJoyFace();
            }
            else
            {
                SwitchToNeutralFace();
            }
        }
    }

    public void SwitchToAngryFace()
    {
        gameObject.GetComponent<Image>().sprite = angryFace;
    }

    public void SwitchToHappyFace()
    {
        gameObject.GetComponent<Image>().sprite = happyFace;
    }

    public void SwitchToNeutralFace()
    {
        gameObject.GetComponent<Image>().sprite = neutralFace;
    }

    public void SwitchToRageFace()
    {
        gameObject.GetComponent<Image>().sprite = rageFace;
    }

    public void SwitchToJoyFace()
    {
        gameObject.GetComponent<Image>().sprite = joyFace;
    }
}
