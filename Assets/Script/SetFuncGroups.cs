using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class SetFuncGroups : MonoBehaviour
{
    SetParents setParent;
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {

            setParent = GetComponent<SetParents>();
            setParent.CreateParents("OH");
            setParent.CreateMolecule("OH");
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            setParent = GetComponent<SetParents>();
            setParent.CreateParents("Methane");
            setParent.CreateMolecule("Methane");
        }        
        if (Input.GetKeyDown(KeyCode.A))
        {
            setParent = GetComponent<SetParents>();
            setParent.CreateParents("Export_20160115191441");
            setParent.CreateMolecule("Export_20160115191441");
        }
    }
}