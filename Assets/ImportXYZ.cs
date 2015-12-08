using UnityEngine;
using System.Collections;
using System.IO;    //System.IO.FileInfo, System.IO.StreamReader
using System;   //Exception, System.Split
using System.Text;  //Encoding

public class ImportXYZ : MonoBehaviour {

    public static readonly string IMPORT_FILE = "exampleChemical";
    //public const double BOND_JUDGMENT = 1.1;

    int AtomsNum = 0, tmpCount = 0;
    private string guitxt = "";
    private string[] line;
    public FixedJoint joint;
    Vector3[] locations;
    GameObject Parent;

	// Use this for initialization
	void Start () {
        Debug.Log("Start: ImportXYZandSetAtoms !");
        Parent = new GameObject("Parent");
        //Debug.Log(System.IO.Directory.GetCurrentDirectory());   // カレントディレクトリを調べる

        // ファイルが存在するか調べる //そのうちディレクトリを選べるように(?)
        string fileName = @".\Assets\" + IMPORT_FILE + ".xyz";
        // ファイルがある
        if (System.IO.File.Exists(fileName))
        {
            guitxt = "Files exist!";
            OnGUI();
            Debug.Log("import_" + IMPORT_FILE);
            // ファイル読み込み()
            ReadFile();
        }
        // ファイルが無い
        else
        {
            // エラーメッセージ
            guitxt = "Files not exist...";
            OnGUI();
        }

        SetBonds SetBonds = GetComponent<SetBonds>();
        SetBonds.CreateBonds();

        foreach (GameObject obj in UnityEngine.Object.FindObjectsOfType(typeof(GameObject)))
        {
            if (obj.tag == "Atoms" || obj.tag == "ChemicalBond")
            {
                //obj.transform.parent = Parent.transform;
                joint = obj.AddComponent<FixedJoint>();
            }
        }

        Debug.Log("End: ImportXYZandSetAtoms !");
        //Application.LoadLevel("Edit");
	}

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, Screen.width, Screen.height), guitxt);
    }

    //ファイル読み込み
    void ReadFile()
    {
        FileInfo fi = new FileInfo(Application.dataPath + "/" + IMPORT_FILE + ".xyz");
        try
        {
            // 一行目読み込み
            using (StreamReader sr = new StreamReader(fi.OpenRead(), Encoding.UTF8))
            {
                // 連続したデータを扱うときに使えるかも
                AtomsNum = int.Parse(sr.ReadLine());
                //Debug.Log(atomsNum);
            }
        }
        catch (Exception e)
        {
            guitxt += SetDefaultText();
            Debug.Log(e);
        }
        locations = new Vector3[AtomsNum];
        StreamReader sr_1 = new StreamReader(fi.OpenRead(), Encoding.UTF8);
        // 2行目は飛ばす
        // 3行目以降ループ
        for (int i = 0; i < 2 + AtomsNum; i++)
        {
            line = new string[2 + AtomsNum];
            line[i] = sr_1.ReadLine() + "\r\n";
            // 球モデルをループでひとつづつ描画
            if (line != null && i > 1 && i < (2 + AtomsNum))
            {
                //Debug.Log(line[i]);
                // このへんでsetAtomModel呼び出し
                setAtomModel(line[i], locations);
            }
        }
    }
    // 改行処理?
    string SetDefaultText()
    {
        return "C#あ\n";
    }

    // 球モデル配置
    void setAtomModel(string str, Vector3[] locations)
    {
        SetAtoms SetAtoms = GetComponent<SetAtoms>();

        string[] name = new string[1];  // 原子名保持
        float[] location = new float[4];// 原子座標保保持

        stringFromElements(str, name, location);
        // 座標を抜き取って球モデルを配置
        locations[tmpCount] = new Vector3(location[1], location[2], location[3]);
        SetAtoms.CreateAtoms(name[0], locations[tmpCount]);

        tmpCount++;
    }

    // 文字列を名前と座標配列に分解するメソッド
    //     事前に呼び出し元で引数の変数と配列を宣言しておく必要あり
    void stringFromElements(string str, string[] name, float[] location)
    {
        int i = 0;

        // 空白全部除外している
        string[] AtomProperty = str.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
        // 名前と座標を各変数と配列に格納
        foreach (string s in AtomProperty)
        {
            if (0 == i)
                name[i] = s;
            else
                location[i] = float.Parse(s);

            i++;
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
