using UnityEngine;
using System.Collections;
using System.IO;    //System.IO.StreamWriter
using System.Text;  //Encoding
using System;       //Exception


/**
 * Jを押すとオブジェクト名表示
 * これを現在の原子を保存するプログラムに書き換える
 */
public class KeyDownJPrintNowPosition : MonoBehaviour
{
    // 別ファイルの変数を使用
    ExportXYZfromAtoms exportXYZ;

    // Use this for initialization
    void Start()
    {
        exportXYZ = GetComponent<ExportXYZfromAtoms>();
    }

    // Update is called once per frame
    void Update()
    {
        // J入力した瞬間
        if (Input.GetKeyDown(KeyCode.J))
        {
            Debug.Log("push J_Key");
            GetFindObjectsOfType();

        }
    }

    void GetFindObjectsOfType()
    {
        // typeで指定した型のすべてのオブジェクトを配列で取得し、その要素数分繰り返す
        foreach (GameObject obj in UnityEngine.Object.FindObjectsOfType(typeof(GameObject)))
        {
            // シーン上に存在するオブジェクトならば処理
            if (obj.activeInHierarchy)
            {
                ReadFile(obj);
                ExportPosition(obj);
            }
        }
    }

    void ReadFile(GameObject obj)
    {
        FileInfo fi = new FileInfo(@".\Assets\Export_" + exportXYZ.nowtime + ".xyz");
        try
        {
            // 一行目読み込み
            using (StreamReader sr = new StreamReader(fi.OpenRead(), Encoding.UTF8))
            {
                if (null == sr.ReadLine())
                {
                    ExportPosition(obj);
                }
                else
                {
                    ExportPosition(obj);
                }
            }
        }
        catch (Exception e)
        {
            //guitxt += SetDefaultText();
            Debug.Log(e);
        }
    }
    // 改行処理?
    string SetDefaultText()
    {
        return "C#あ\n";
    }

    void ExportPosition(GameObject obj)
    {
        // GameObjectの名前を表示
        //Debug.Log(obj.name);
        //Debug.Log(obj.transform.position.x);

        // メモリストリーム作成
        FileInfo fi = new FileInfo(@".\Assets\Export_" + exportXYZ.nowtime + ".xyz");
        //StreamWriter sw = new StreamWriter(fi.OpenWrite());
        using (StreamWriter sw = fi.AppendText())
        {
            //if 入れる
            sw.WriteLine(obj.name + "\t"
                + obj.transform.position.x.ToString() + "\t"
                + obj.transform.position.y.ToString() + "\t"
                + obj.transform.position.z.ToString()
            );
            sw.Close();
        }
        //sw.WriteLine();
        //sw.WriteLine(obj.name + "\t"
        //    + obj.transform.position.x.ToString() + "\t"
        //    + obj.transform.position.y.ToString() + "\t"
        //    + obj.transform.position.z.ToString()
        //);
    }
}
