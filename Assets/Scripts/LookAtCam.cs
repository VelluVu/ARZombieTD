using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCam : MonoBehaviour {

    public Camera cam;
	// Update is called once per frame
	void Update () {
        gameObject.transform.LookAt(cam.transform);
	}
}
