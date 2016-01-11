using UnityEngine;
using System.Collections;
using System.IO;    //System.IO.StreamWriter
using System.Text;  //Encoding
using System;       //Exception


/**
 * Pを押すとオブジェクト名表示
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
        // P入力した瞬間
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
            if (obj.tag == "Atoms"  // タグがAtomsで、
            //if (obj.name != "ChemicalBond"
                && obj.activeInHierarchy)   // シーン上に存在するオブジェクトなら処理
            {
                n = ExportPosition(obj, n);
            }
        }
        ControlFile(n);
    }

    // 1オブジェクトごとに書き出し
    int ExportPosition(GameObject obj, int ReferLine)
    {
        // メモリストリーム作成
        FileInfo fi = new FileInfo(PATH);

        using (StreamWriter sw = fi.AppendText())
        //using (StreamWriter sw = new StreamWriter(PATH))
        {
            //
            if (0 == ReferLine)
            {
                sw.WriteLine("");
                sw.Write("Export_" + exportXYZ.nowtime + ".xyz");
            }
            //    //if 入れる
            sw.Write("\n" +
                obj.name + " " +
                obj.transform.position.x.ToString() + " " +
                obj.transform.position.y.ToString() + " " +
                obj.transform.position.z.ToString()
            );
            sw.Close();
        }
        return ++ReferLine;
    }

    // カメラなどの座標が書き込まれた行を力技で強引に編集
    void ControlFile(int n)
    {
        string tmpFile = Path.GetTempFileName();
        using (StreamReader sr = new StreamReader(PATH))
        using (StreamWriter sw = new StreamWriter(tmpFile))
        {
            int ReferLine = 0;  // 参照する行
            while (sr.Peek() > -1)
            {
                string line = sr.ReadLine();    // 読み込んだ一行
                ++ReferLine;
                if (1 == ReferLine)
                {
                    sw.Write(n);
                }
                else if (n + 3 == ReferLine)
                {
                    sw.Dispose();
                }
                else
                {
                    sw.Write("\n" + line);
                }
                //ReferLine++;
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
