using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class SetBonds : MonoBehaviour
{
    public GameObject ChemicalBondsPrefab;
    //Rigidbody rigid;  // 元のやつ
    public const double BOND_JUDGMENT = 1.1;
    GameObject OH;
    Rigidbody rigidParent;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.B))
        //{
        //    Debug.Log("push B");
        //    testFuncGroup();
        //}
        // 60フレームごとに関数を呼び出す
        if (0 == Time.frameCount % 60)
        {
            testFuncGroup();
        }
    }

    // 官能基つくるためのテスト
    public void testFuncGroup()
    {
        List<GameObject> FuncGroup = new List<GameObject>();
        // Atomsタグがついた全てのオブジェクトをFuncGroupに追加
        foreach (GameObject obj1 in UnityEngine.Object.FindObjectsOfType(typeof(GameObject)))
        {
            if (obj1.tag == "Atoms")
                FuncGroup.Add(obj1);
        }
        // Parentタグがついたオブジェクトの子を判別し、該当したものはFuncGroupから削除
        //foreach (GameObject obj2 in UnityEngine.Object.FindObjectsOfType(typeof(GameObject)))
        //{
        //    if (obj2.tag == "Parent")
        //    {
        //        for (int i = 0; i < FuncGroup.Count; i++)
        //        {
        //            if (obj2.transform.IsChildOf(FuncGroup[i].transform.parent))
        //            {
        //                //Debug.Log(FuncGroup[i]);
        //                //FuncGroup.Remove(FuncGroup[i]);
        //            }
        //        }
        //    }
        //}


        // OHの官能基
        for (int i = 0; i < FuncGroup.Count; i++)
        {
            for (int j = 0; j < i; j++)
            {
                if ((FuncGroup[i].name == "H" && FuncGroup[j].name == "O") ||
                    (FuncGroup[j].name == "H" && FuncGroup[i].name == "O"))
                {
                    OH = new GameObject("OH");
                    if (FuncGroup[i].transform.parent != OH.transform &&
                         FuncGroup[j].transform.parent != OH.transform)
                    {

                        //OH = new GameObject("OH");
                        DontDestroyOnLoad(OH);
                        rigidParent = OH.AddComponent<Rigidbody>();
                        rigidParent.useGravity = false;
                        rigidParent.isKinematic = false;
                        CalcBond(FuncGroup[i], FuncGroup[j]);
                        FuncGroup[i].transform.parent = OH.transform;
                        FuncGroup[j].transform.parent = OH.transform;
                        //return 0; // 抜けるために書いただけ
                    }
                }
            }
        }
        //return 0;
    }

    // 全オブジェクトを取得して、"Atoms"tag を抜き出す
    public void CreateBonds()
    {
        List<GameObject> AtomsList = new List<GameObject>();    // 格納する配列
        foreach (GameObject obj in UnityEngine.Object.FindObjectsOfType(typeof(GameObject)))
        {
            if (obj.tag == "Atoms")
                AtomsList.Add(obj); // Atomsタグがついてるやつ入れる
        }
        // Bondのために2つの分子を比較
        // aaa Rigidbody rigid = AtomsList[0].GetComponent<Rigidbody>();   // ここから
        // aaa FixedJoint joint;
        for (int i = 0; i < AtomsList.Count; i++)
        {
            for (int j = 0; j < i; j++)
            {
                CalcBond(AtomsList[j], AtomsList[i]);
            }
            if (i != 0)
            {
                //Debug.Log(AtomsList[i]);
                //Debug.Log(rigid);
                //aaaaaaaaaaaaaaaaa
                //joint = AtomsList[i].AddComponent<FixedJoint>();
                //joint.connectedBody = rigid;
            }
        }   // ここまで
    }
    // 向きなどの計算
    void CalcBond(GameObject obj1, GameObject obj2)
    {
        // 2つの座標ベクトルの比較
        float distance = Vector3.Distance(obj1.transform.position, obj2.transform.position);

        if (obj1.activeInHierarchy && obj2.activeInHierarchy && // シーン上に存在し、
            BOND_JUDGMENT > distance)   // 距離が定数値を下回っていれば
        {
            //Debug.Log(obj1.name + ", " + obj2.name);

            //向きを定義
            float x, y, z, r,           //差分
                  rad_x, rad_y, rad_z;  //3点の角度
            Vector3 position;       //座標
            Quaternion rotation;    //回転

            //まず向きを決めてから座標の位置を2座標の中心に変更する
            //http://qiita.com/2dgames_jp/items/60274efb7b90fa6f986a
            x = obj1.transform.position.x - obj2.transform.position.x;
            y = obj1.transform.position.y - obj2.transform.position.y;
            z = obj1.transform.position.z - obj2.transform.position.z;
            r = Mathf.Sqrt(x * x + y * y + z * z);
            rad_y = Mathf.Atan2(z, x) * Mathf.Rad2Deg;
            rad_x = Mathf.Acos(y / r) * Mathf.Rad2Deg;
            rad_z = 0;

            position = new Vector3((obj2.transform.position.x + obj1.transform.position.x) / 2,
                                   (obj2.transform.position.y + obj1.transform.position.y) / 2,
                                   (obj2.transform.position.z + obj1.transform.position.z) / 2);
            rotation = Quaternion.Euler(rad_z, -rad_y, -rad_x); //rad_xもマイナスにしたら動いた
            //rotation = Quaternion.Euler(rad_z, -rad_y, rad_x); //動かない

            //座標を変更
            //表示
            GameObject ChemicalBond = Instantiate(ChemicalBondsPrefab, position, rotation) as GameObject;
            ChemicalBond.name = "ChemicalBond"; // 名前変更
            ChemicalBond.transform.parent = null;   // 親オブジェクト無しで初期化
            DontDestroyOnLoad(ChemicalBond);// Object 保持

            //長さの変更
            ChemicalBond.transform.localScale
                = new Vector3(ChemicalBond.transform.localScale.x,
                              distance / 2,
                              ChemicalBond.transform.localScale.z);

            // Rigidbodyコンポーネントを追加
            Rigidbody rigid = ChemicalBond.AddComponent<Rigidbody>();
            rigid.isKinematic = false;  // 物理計算しない
            rigid.useGravity = false;   // 重力使用しない
            rigid.drag = 5.0f;          // 空気抵抗
            rigid.angularDrag = 10f;    // 回転の空気抵抗
        }
    }
}
