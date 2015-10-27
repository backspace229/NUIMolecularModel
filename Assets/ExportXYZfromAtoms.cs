using UnityEngine;
using System.Collections;
using System.IO;    //System.IO.FileInfo
using System;

public class ExportXYZfromAtoms : MonoBehaviour {

    // Use this for initialization
	void Start () {
        Debug.Log("Start: ExportXYZformAtoms");

        // 現在時刻を取得
        DateTime now = DateTime.Now;
        string nowtime = now.ToString("yyyyMMddHHmmss");
        // (Export + 現在時刻)の.xyz ファイルを作成
        FileStream stream = File.Create(@".\Assets\Export_" + nowtime + ".xyz");
        // ファイルを閉じる
        stream.Close();
        Debug.Log("作成完了");

        Debug.Log("End: ExportXYZformAtoms");
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