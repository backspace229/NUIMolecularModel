using UnityEngine;
using System.Collections;
using System.IO;    //System.IO.StreamWriter
using System.Text;  //Encoding
using System;       //Exception


/**
 * Jを押すとオブジェクト名表示
 * これを現在の原子を保存するプログラムに書き換える
 */
public class PrintPosition : MonoBehaviour
{
    // 別ファイルの変数を使用
    ExportXYZ exportXYZ;
    string PATH;

    // Use this for initialization
    void Start() {
        exportXYZ = GetComponent<ExportXYZ>();
        PATH = @".\Assets\Export_" + exportXYZ.nowtime + ".xyz";
    }

    // Update is called once per frame
    void Update() {
        // J入力した瞬間
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("push P_Key");
            GetFindObjectsOfType();

        }
    }

    void GetFindObjectsOfType()
    {
        int n = 0;  // 分子数カウント
        // typeで指定した型のすべてのオブジェクトを配列で取得し、その要素数分繰り返す
        foreach (GameObject obj in UnityEngine.Object.FindObjectsOfType(typeof(GameObject)))
        {
            if (obj.name != "ChemicalBond"  // ChemicalBond以外で、
                && obj.activeInHierarchy)   // シーン上に存在するオブジェクトなら処理
            {
                n++;
                ExportPosition(obj);
            }
        }
        ControlFile(n);
    }

    // 1オブジェクトごとに書き出し(この時点ではカメラなども含まれてしまう)
    void ExportPosition(GameObject obj)
    {
        // メモリストリーム作成
        FileInfo fi = new FileInfo(PATH);
        using (StreamWriter sw = fi.AppendText())
        {
            //if 入れる
            sw.WriteLine(
                obj.name + " " +
                obj.transform.position.x.ToString() + " " +
                obj.transform.position.y.ToString() + " " +
                obj.transform.position.z.ToString()
            );
            sw.Close();
        }
    }

    // カメラなどの座標が書き込まれた行を力技で強引に編集
    void ControlFile(int n)
    {
        string tmpFile = Path.GetTempFileName();
        using (StreamReader sr = new StreamReader(PATH))
        using (StreamWriter sw = new StreamWriter(tmpFile))
        {
            int ReferLine = 1;  // 参照する行
            while (sr.Peek() > -1)
            {
                string line = sr.ReadLine();
                if (1 == ReferLine)
                {
                    sw.WriteLine(n - 3);
                }
                else if (2 == ReferLine)
                {
                    sw.WriteLine("Export_" + exportXYZ.nowtime + ".xyz");
                }
                else if (n == ReferLine)
                {
                    sw.Dispose();
                }
                else
                {
                    sw.WriteLine(line);
                }
                ReferLine++;
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
