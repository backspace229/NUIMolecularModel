using UnityEngine;
using System.Collections;
using System.IO;    //System.IO.FileInfo, System.IO.StreamReader
using System;   //Exception, System.Split
using System.Text;  //Encoding

public class ImportXYZ : MonoBehaviour {

    SetParents setParent;

	// Use this for initialization
	void Start () {
        Debug.Log("Start: ImportXYZandSetAtoms !");

        setParent = GetComponent<SetParents>();
        setParent.CreateParents("exampleChemical", "Molecule");

        Debug.Log("End: ImportXYZandSetAtoms !");
        Application.LoadLevel("Edit");
	}

	// Update is called once per frame
	void Update () {
	
	}
}
