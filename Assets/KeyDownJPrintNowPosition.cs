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
    string PATH;

    // Use this for initialization
    void Start()
    {
        exportXYZ = GetComponent<ExportXYZfromAtoms>();
        PATH = @".\Assets\Export_" + exportXYZ.nowtime + ".xyz";
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
        int n = 0;
        // typeで指定した型のすべてのオブジェクトを配列で取得し、その要素数分繰り返す
        foreach (GameObject obj in UnityEngine.Object.FindObjectsOfType(typeof(GameObject)))
        {
            // 化学結合を破壊
            if (obj.name == "ChemicalBond")
            {
                Destroy(this);
            }
            else
            {
                // シーン上に存在するオブジェクトならば処理
                if (obj.activeInHierarchy)
                {
                    n++;
                    ExportPosition(obj);
                }
            }

        }
        ControlFile(n);
    }

    void ExportPosition(GameObject obj)
    {
        // メモリストリーム作成
        FileInfo fi = new FileInfo(PATH);
        using (StreamWriter sw = fi.AppendText())
        {
            //if 入れる
            sw.WriteLine(obj.name + " "
                + obj.transform.position.x.ToString() + " "
                + obj.transform.position.y.ToString() + " "
                + obj.transform.position.z.ToString()
            );
            sw.Close();
        }
    }

    void ControlFile(int n)
    {
        string tmpFile = Path.GetTempFileName();
        using (StreamReader sr = new StreamReader(PATH))
        using (StreamWriter sw = new StreamWriter(tmpFile))
        {
            int i = 1;
            while (sr.Peek() > -1)
            {
                string line = sr.ReadLine();
                if (1 == i)
                {
                    sw.WriteLine(n - 3);
                }
                else if (2 == i)
                {
                    sw.WriteLine("Export_" + exportXYZ.nowtime + ".xyz");
                }
                else if (n == i)
                {
                    sw.Dispose();
                }
                else
                {
                    sw.WriteLine(line);
                }
                i++;
            }
            //閉じる
            sr.Close();
            sw.Close();
        }
        //入れ替え
        File.Copy(tmpFile, PATH, true);
        File.Delete(tmpFile);
    }
}
