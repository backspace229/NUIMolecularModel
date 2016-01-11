using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CutBonds : MonoBehaviour {

	// Use this for initialization
	void Start () {
        List<GameObject> BondsList = new List<GameObject>();
        foreach (GameObject obj in UnityEngine.Object.FindObjectsOfType(typeof(GameObject)))
        {
            if (obj.tag == "ChemicalBond")
                BondsList.Add(obj);
        }
	}
	
	// Update is called once per frame
	void Update () {
        //OnTriggerEnter();
	}
    void OnTriggerEnter(Collider other)
    {
        Destroy(this);
    }
}
