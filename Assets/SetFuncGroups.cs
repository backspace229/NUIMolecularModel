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
        if (0 == Time.frameCount % 60)
        {
            CreateFuncGroups();
        }
	}
    public void CreateFuncGroups()
    {
        List<GameObject> FuncGroup = new List<GameObject>();
        foreach (GameObject obj in UnityEngine.Object.FindObjectsOfType(typeof(GameObject)))
        {
            if (obj.tag == "Atoms")
                FuncGroup.Add(obj);
        }
        for (int i = 0; i < FuncGroup.Count; i++)
        {
            for (int j = 0; j < i; j++)
            {
                if (FuncGroup[i].transform.parent != OH.transform || FuncGroup[j].transform.parent != OH.transform)
                {
                    if (FuncGroup[i].name == "O" && FuncGroup[j].name == "H")
                    {
                        OH = new GameObject("OH");
                        FuncGroup[i].transform.parent = OH.transform;
                        FuncGroup[j].transform.parent = OH.transform;
                    }
                    else if (FuncGroup[j].name == "O" && FuncGroup[i].name == "H")
                    {
                        OH = new GameObject("OH");
                        FuncGroup[i].transform.parent = OH.transform;
                        FuncGroup[j].transform.parent = OH.transform;
                    }
                    DontDestroyOnLoad(OH);
                }
            }
        }
    }
}
