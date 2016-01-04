using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class SetFuncGroups : MonoBehaviour {

    GameObject OH;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("push F-key");
            CreateFuncGroups();
        }
	}
    public void CreateFuncGroups()
    {
        Debug.Log("createFuncGroup");
    }
}
