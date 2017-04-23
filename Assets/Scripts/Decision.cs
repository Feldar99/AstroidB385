using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Decision : MonoBehaviour
{

    //if true this decision will be taken
    public abstract bool Condition();
    //called every frame that a decision is being taken.  Returns true when the decision is finished
    public abstract bool UpdateDecision(); 
}
