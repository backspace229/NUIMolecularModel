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
    Atoms atom1, atom2;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            Debug.Log("push B-key");
        }
    }

    // 全オブジェクトを取得して、"Atoms"tag を抜き出す
    public void SetAtomsList(GameObject Parent)
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
                // 相互に結合数を追加
                atom1 = AtomsList[j].GetComponent<Atoms>();
                atom2 = AtomsList[i].GetComponent<Atoms>();
                if (atom1.bondsNum < 1 || atom2.bondsNum < 1)
                    CalcBond(Parent, AtomsList[j], AtomsList[i]);
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
    void CalcBond(GameObject Parent, GameObject obj1, GameObject obj2)
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
            CreateBonds(Parent, position, rotation, distance);

            atom1.bondsNum += 1;
            atom2.bondsNum += 1;
        }
    }
    void CreateBonds(GameObject Parent, Vector3 position, Quaternion rotation, float distance)
    {
        //座標を変更
        //表示
        GameObject obj = Instantiate(ChemicalBondsPrefab, position, rotation) as GameObject;
        obj.name = "ChemicalBond"; // 名前変更
        obj.transform.parent = null;   // 親オブジェクト無しで初期化
        DontDestroyOnLoad(obj);// Object 保持

        //長さの変更
        obj.transform.localScale
            = new Vector3(obj.transform.localScale.x,
                          distance / 2,
                          obj.transform.localScale.z);

        // Rigidbodyコンポーネントを追加
        Rigidbody rigid = obj.AddComponent<Rigidbody>();
        rigid.isKinematic = false;  // 物理計算しない
        rigid.useGravity = false;   // 重力使用しない
        rigid.mass = 0.0f;          // 物体の重さ
        rigid.drag = 10.0f;         // 空気抵抗
        rigid.angularDrag = 10.0f;  // 回転の空気抵抗

        if (null != Parent)
        {
            FixedJoint fixJoint = obj.AddComponent<FixedJoint>();
            fixJoint.connectedBody = Parent.GetComponent<Rigidbody>();
            obj.transform.parent = Parent.transform;
        }
    }
}
