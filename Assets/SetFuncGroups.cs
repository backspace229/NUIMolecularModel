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
    }
}