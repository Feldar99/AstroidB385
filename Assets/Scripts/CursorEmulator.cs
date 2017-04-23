using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorEmulator : MonoBehaviour {
    public float distanceFromCamera = 1;
    public Camera sceneCamera;
    public GameObject paintObject;
    public Transform FollowTransform;
	
	// Update is called once per frame
	void Update () {
        //Vector3 cursorPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distanceFromCamera);
        //transform.position = sceneCamera.ScreenToWorldPoint(cursorPos);
        transform.LookAt(paintObject.transform);
	    if (FollowTransform != null)
	    {
	        transform.position = FollowTransform.position;
	    }
    }
}
