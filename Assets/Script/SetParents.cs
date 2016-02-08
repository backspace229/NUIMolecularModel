using UnityEngine;
using System.Collections;
using System.IO;    //System.IO.FileInfo, System.IO.StreamReader
using System;   //Exception, System.Split
using System.Text;  //Encoding
using System.Collections.Generic;

public class SetParents : MonoBehaviour {

    int AtomsNum, tmpCount;
    private string guitxt = "";
    private string[] line;
    Rigidbody rigidParent;
    //FixedJoint joint;
    Vector3[] locations;
    public GameObject Parent;
    AtomsInfo atom;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

    /// <summary>
    /// 原子と結合の親を作る
    /// </summary>
    /// <param name="ParentName">親の名前</param>
    public void CreateParents(string ParentName)
    {
        Parent = new GameObject(ParentName);
        Parent.tag = "Parent";
        DontDestroyOnLoad(Parent);
        rigidParent = Parent.AddComponent<Rigidbody>();   // 親にRididbody
        rigidParent.useGravity = false;
        rigidParent.isKinematic = false;
        rigidParent.mass = 1.0f;
        rigidParent.drag = 10.0f;
        rigidParent.angularDrag = 10.0f;
        atom = Parent.AddComponent<AtomsInfo>();
    }

    /// <summary>
    /// ファイルが存在したら開き、分子モデルを作成する
    /// </summary>
    /// <param name="FileName">ファイル名</param>
    public void CreateMolecule(string FileName)
    {
        AtomsNum = 0;
        tmpCount = 0;

        // ファイルが存在するか調べ、存在したら開く
        //そのうちディレクトリを選べるように(?)
        ExistFile(FileName);

        SetBonds SetBonds = GetComponent<SetBonds>();
        SetBonds.SetAtomsList(Parent);
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, Screen.width, Screen.height), guitxt);
    }

    // ファイルの存在の有無(SetFuncGroupメソッドでも使う)
    void ExistFile(string FileName)
    {
        string fileName = @".\Assets\" + FileName + ".xyz";
        // ファイルがある
        if (System.IO.File.Exists(fileName))
        {
            guitxt = "Files exist!";
            OnGUI();
            Debug.Log("import_" + FileName);
            // ファイル読み込み()
            ReadFile(FileName);
        }
        // ファイルが無い
        else
        {
            // エラーメッセージ
            guitxt = "Files not exist...";
            OnGUI();
        }
    }

    //ファイル読み込み
    void ReadFile(string FileName)
    {
        FileInfo fi = new FileInfo(Application.dataPath + "/" + FileName + ".xyz");
        //FileInfo fi = new FileInfo(@".\Assets\" + FileName + ".xyz");
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
        // 座標を抜き取って球モデルを配置  z軸はマイナスにする
        locations[tmpCount] = new Vector3(location[1], location[2], -(location[3]));
        SetAtoms.CreateAtoms(Parent, name[0], locations[tmpCount]);
        atom.GetComponent<AtomsInfo>().childName.Add(name[0]);

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
}
