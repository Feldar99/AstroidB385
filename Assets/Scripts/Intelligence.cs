using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intelligence : MonoBehaviour
{

    public Decision[] PossibleDecisions;
    private Decision lastDecision;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	    if (lastDecision == null)
	    {
	        EvaluateDecisions();
	    }

	    if (lastDecision != null)
	    {
	        bool finished = lastDecision.UpdateDecision();
	        if (finished)
	        {
	            lastDecision = null;
	        }
	    }
	}

    private void EvaluateDecisions()
    {
        foreach (Decision decision in PossibleDecisions)
        {
            if (decision.Condition())
            {
                lastDecision = decision;
                break;
            }
        }
    }
}
