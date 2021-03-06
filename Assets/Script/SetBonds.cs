﻿using UnityEngine;
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
    AtomsInfo atom1, atom2;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (0 == Time.frameCount % 3)
            SwitchingIsKinematic();
        if (Input.GetKeyDown(KeyCode.B))
        {
            Debug.Log("push B-key");
            List<GameObject> Parent = new List<GameObject>();
            List<GameObject> FuncGroup = new List<GameObject>();
            foreach (GameObject obj in UnityEngine.Object.FindObjectsOfType(typeof(GameObject)))
            {
                if ("Parent" == obj.tag && "Molecule" != obj.name)
                    FuncGroup.Add(obj);
                if ("Molecule" == obj.name)
                    Parent.Add(obj);
            }
            for (int i = 0; i < FuncGroup.Count; i++)
            {
                BondsFuncGroup(Parent[0], FuncGroup[i]);
            }
        }
    }

    /// <summary>
    /// 親オブジェクトを渡しているので、子オブジェクトを検索して使う
    /// </summary>
    /// <param name="Parent">Moleculeオブジェクト</param>
    /// <param name="FuncGroup">官能基オブジェクト</param>
    void BondsFuncGroup(GameObject Parent, GameObject FuncGroup){
        AtomsInfo ParentInfo = Parent.GetComponent<AtomsInfo>();
        //AtomsInfo FuncInfo = FuncGroup.GetComponent<AtomsInfo>();

        //*
        bool flag = false;
        // Molecule内の原子分ループ
        for (int i = 0; i < Parent.transform.childCount; i++)
        {
            for (int j = 0; j < FuncGroup.transform.childCount; j++)
            {
                if ("Atoms" == Parent.transform.GetChild(i).gameObject.tag && "Atoms" == FuncGroup.transform.GetChild(j).gameObject.tag)
                    flag = CalcFuncGroup(Parent.transform.GetChild(i).gameObject, FuncGroup.transform.GetChild(j).gameObject);
                if (flag)
                {
                    CalcBond(Parent, Parent.transform.GetChild(i).gameObject, FuncGroup.transform.GetChild(j).gameObject);
                    break;
                }
            }
            if (flag) break;
        }

        if (flag)
        {
            //ここでFuncGroup内の全てのオブジェクトをParentに移動させる
            // Parent内の全ての子オブジェクトを取得
            // ループごとにFuncGroup.transform.childCountの値が変わってしまったのでこの書き方
            for (int i = FuncGroup.transform.childCount - 1; i >= 0; i--)
            {
                //Debug.Log(FuncGroup.transform.GetChild(i).gameObject);
                if ("Atoms" == FuncGroup.transform.GetChild(i).tag)
                {
                    ParentInfo.childName.Add(FuncGroup.transform.GetChild(i).name);
                    //ParentList.Add(FuncGroup.transform.GetChild(i).gameObject);

                }

                FixedJoint fixJoint = FuncGroup.transform.GetChild(i).gameObject.GetComponent<FixedJoint>();
                fixJoint.connectedBody = Parent.GetComponent<Rigidbody>();
                FuncGroup.transform.GetChild(i).gameObject.transform.parent = Parent.transform;
                if (0 == i) Destroy(FuncGroup);
            }
        }
        //Debug.Log(flag);
    }

    /// <summary>
    /// 2つのオブジェクトを比較し、距離が一定以内にあればtrueを返す
    /// </summary>
    /// <param name="obj1">比較する球1つめ</param>
    /// <param name="obj2">比較する球2つめ</param>
    /// <returns></returns>
    bool CalcFuncGroup(GameObject obj1, GameObject obj2)
    {
        // 2つの座標ベクトルの比較
        float distance = Vector3.Distance(obj1.transform.position, obj2.transform.position);
        bool flag = false;  // 初期化

        if (obj1.activeInHierarchy && obj2.activeInHierarchy && // シーン上に存在し、
            BOND_JUDGMENT > distance)   // 距離が定数値を下回っていれば
        {
            flag = true;
        }
        return flag;
    }


    /// <summary>
    /// 全オブジェクトを取得して、"Atoms"tag を抜き出す
    /// </summary>
    /// <param name="Parent">親とするオブジェクト</param>
    public void SetAtomsList(GameObject Parent)
    {
        List<GameObject> AtomsList = new List<GameObject>();    // 格納する配列
        foreach (GameObject obj in UnityEngine.Object.FindObjectsOfType(typeof(GameObject)))
        {
            if (obj.tag == "Atoms")
                AtomsList.Add(obj); // Atomsタグがついてるやつ入れる
        }

        // Bondのために2つの分子を比較
        for (int i = 0; i < AtomsList.Count; i++)
        {
            for (int j = 0; j < i; j++)
            {
                // 結合数を取得
                atom1 = AtomsList[j].GetComponent<AtomsInfo>();
                atom2 = AtomsList[i].GetComponent<AtomsInfo>();
                // 結合数が1未満もしくは2つのオブジェクトの親が異なるとき(距離は考慮しない)
                if (atom1.bondsNum < 1 || atom2.bondsNum < 1)
                {
                    // 結合させる
                    CalcBond(Parent, AtomsList[j], AtomsList[i]);
                }
            }
        }
    }
    // 向きなどの計算
    /// <summary>
    /// 2つの球オブジェクトの距離を比較し、距離が定数値を下回っていれば、
    /// 棒オブジェクトの向きや長さを計算する
    /// </summary>
    /// <param name="Parent">親とするオブジェクト</param>
    /// <param name="obj1">比較する球1つめ</param>
    /// <param name="obj2">比較する球2つめ</param>
    public void CalcBond(GameObject Parent, GameObject obj1, GameObject obj2)
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

            // 相互に結合数を追加
            atom1 = obj1.GetComponent<AtomsInfo>();
            atom1.bondsNum += 1;
            atom2 = obj2.GetComponent<AtomsInfo>();
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

        // 親子関係付け処理
        if (null != Parent)
        {
            FixedJoint fixJoint = obj.AddComponent<FixedJoint>();
            fixJoint.connectedBody = Parent.GetComponent<Rigidbody>();
            obj.transform.parent = Parent.transform;
        }
    }

    void SwitchingIsKinematic()
    {
        Rigidbody rigid;

        foreach (GameObject obj in UnityEngine.Object.FindObjectsOfType(typeof(GameObject)))
        {
            if ("Molecule" == obj.name)
            {
                rigid = obj.GetComponent<Rigidbody>();
                if (rigid.isKinematic)
                {
                    rigid.isKinematic = false;

                }
                else
                {
                    rigid.isKinematic = true;
                }
            }
        }
    }
}
