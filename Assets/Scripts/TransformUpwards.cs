using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformUpwards : MonoBehaviour {

    Camera cam;

    private void Start()
    {
        cam = Camera.current;
    }

    void Update () {
        transform.LookAt(-cam.transform.position);
        transform.Translate(0, 0.1f * Time.deltaTime, 0);
	}
}
