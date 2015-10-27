using UnityEngine;
using System.Collections;

public class ExportXYZfromAtoms : MonoBehaviour {

	// Update is called once per frame
	void Update () {
        Debug.Log("test");
	}
}


// XYZフォーマットの構造体を定義してみる
public struct XYZformat
{
    public string name;
    public Vector3[] location;

    public XYZformat(string name, Vector3[] location)
    {
        this.name = name;
        this.location = location;
    }
}