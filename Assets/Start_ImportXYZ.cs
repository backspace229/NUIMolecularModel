using UnityEngine;
using System.Collections;
using System.IO;    //System.IO.FileInfo, System.IO.StreamReader
using System;   //Exception, System.Split
using System.Text;  //Encoding

public class Start_ImportXYZ : MonoBehaviour {

    public static readonly string IMPORT_FILE = "exampleChemical";
    public const double BOND_JUDGMENT = 1.1;

    // CreateAtoms のメソッドを使いたいが保留

    int AtomsNum = 0, tmpCount = 0;
    private string guitxt = "";
    private string[] line;
    public GameObject OPrefab, HPrefab, CPrefab;
    public GameObject ChemicalBondsPrefab;
    Rigidbody AtomRigid, BondRigid;
    Vector3[] locations;

	// Use this for initialization
	void Start () {
        Debug.Log("Start: ImportXYZandSetAtoms !");
        //guitxt = System.IO.Directory.GetCurrentDirectory();   // カレントディレクトリを調べる
        //Debug.Log(guitxt);

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
        Debug.Log("End: ImportXYZandSetAtoms !");
        Application.LoadLevel("Edit");
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
        Edit_SetAtoms SetAtoms;
        SetAtoms = GetComponent<Edit_SetAtoms>();

        string[] name = new string[1];  // 原子名保持
        float[] location = new float[4];// 原子座標保保持

        stringFromElements(str, name, location);
        // 座標を抜き取って球モデルを配置
        locations[tmpCount] = new Vector3(location[1], location[2], location[3]);
        SetAtoms.CreateAtoms(name[0], locations[tmpCount]);
        //GameObject Atom = Instantiate(AtomsPrefab, locations[tmpCount], Quaternion.identity) as GameObject;
        //Atom.name = name[0];    // 読み込んだファイルの原子名
        //AtomRigid = Atom.AddComponent<Rigidbody>();
        //AtomRigid.isKinematic = false;
        //AtomRigid.useGravity = false;
        //AtomRigid.drag = 10f;
        //DontDestroyOnLoad(Atom);// Sceneを切り替えてもObjectを保持

        //各座標を比較して距離を求める
        if (0 < tmpCount)   // tmpCountが0以上のとき
        {
            int i;

            for (i = tmpCount - 1; i >= 0; i--)
            {
                // 2つの座標ベクトルの比較
                float distance = Vector3.Distance(locations[tmpCount], locations[i]);
                //Debug.Log(distance);    // 数値確認用

                // if 距離が定数値を下回っていればその長さの棒モデルを描画
                if (BOND_JUDGMENT > distance)
                {
                    //まず向きを決めてから座標の位置を2座標の中心に変更する
                    //http://qiita.com/2dgames_jp/items/60274efb7b90fa6f986a
                    //向きを定義
                    float x, y, z, r;  //差分
                    float rad_x, rad_y, rad_z; //3点の角度
                    Vector3 position;       //座標
                    Quaternion rotation;    //回転

                    x = locations[tmpCount].x - locations[i].x;
                    y = locations[tmpCount].y - locations[i].y;
                    z = locations[tmpCount].z - locations[i].z;
                    r = Mathf.Sqrt(x * x + y * y + z * z);
                    rad_y = Mathf.Atan2(z, x) * Mathf.Rad2Deg;
                    rad_x = Mathf.Acos(y / r) * Mathf.Rad2Deg;
                    rad_z = 0;

                    position = new Vector3((locations[i].x + locations[tmpCount].x) / 2, (locations[i].y + locations[tmpCount].y) / 2, (locations[i].z + locations[tmpCount].z) / 2);
                    rotation = Quaternion.Euler(rad_z, -rad_y, -rad_x); //rad_xもマイナスにしたら動いた
                    //rotation = Quaternion.Euler(rad_z, -rad_y, rad_x); //動かない

                    //座標を変更
                    //表示
                    GameObject ChemicalBond = Instantiate(ChemicalBondsPrefab, position, rotation) as GameObject;
                    ChemicalBond.name = "ChemicalBond"; //オブジェクト名変更
                    BondRigid = ChemicalBond.AddComponent<Rigidbody>();
                    BondRigid.isKinematic = true;
                    BondRigid.useGravity = false;
                    DontDestroyOnLoad(ChemicalBond);

                    //長さなどの変更
                    //Debug.Log(ChemicalBond.transform.localScale);
                    ChemicalBond.transform.localScale = new Vector3(ChemicalBond.transform.localScale.x, distance / 2, ChemicalBond.transform.localScale.z);
                }
            }
        }
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
