using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintOutControls : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey("Melee") || Input.GetKey("Horizontal") || Input.GetKey("Vertical") || Input.GetKey("Block") || Input.GetKey("Dash/Roll") || Input.GetKey("camHorizontal") || Input.GetKey("camVertical") || Input.GetKey("Aim") || Input.GetKey("Fire") || Input.GetKey("Recall Shield")){
	Debug.Log("Key");
	}
    }
}
