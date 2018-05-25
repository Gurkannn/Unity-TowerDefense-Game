using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    
    private bool IsPanning { get; set; }
    public float PanSpeed;

	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(2))
        {
            IsPanning = true;
        }

        if (Input.GetMouseButtonUp(2))
        {
            IsPanning = false;
        }

        if (IsPanning)
        {
            Camera.main.transform.position -= new Vector3(Input.GetAxis("Mouse X") * PanSpeed * Time.deltaTime, Input.GetAxis("Mouse Y") * PanSpeed * Time.deltaTime) ;
        }
	}
}
