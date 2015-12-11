﻿using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class SetBonds : MonoBehaviour {
    public GameObject ChemicalBondsPrefab;
    //Rigidbody rigid;  // 元のやつ
    public const double BOND_JUDGMENT = 1.1;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        //if (0 == Time.frameCount % 60)
        //{
        //    CreateBonds();
        //}
        //// 60フレームごとに関数を呼び出す
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
        Rigidbody rigid = AtomsList[0].GetComponent<Rigidbody>();   // ここから
        FixedJoint joint;
        for (int i = 0; i < AtomsList.Count; i++)
        {
            for (int j = 0; j < i; j++)
            {
                CalcBond(AtomsList[j], AtomsList[i]);
            }
            if (i != 0)
            {
                Debug.Log(AtomsList[i]);
                Debug.Log(rigid);
                joint = AtomsList[i].AddComponent<FixedJoint>();
                joint.connectedBody = rigid;

            }
        }   // ここまで

        //for (int i = 0; i < AtomsList.Count - 1; i++) // 元のやつ
        //{
        //    for (int j = i + 1; j < AtomsList.Count; j++)
        //    {
        //        CalcBond(AtomsList[i], AtomsList[j]);
        //    }
        //}
    }
    // 向きなどの計算
    void CalcBond(GameObject obj1, GameObject obj2) {
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
            ChemicalBond.name = "ChemicalBond"; //オブジェクト名変更
            // 元のやつ
            //rigid = ChemicalBond.AddComponent<Rigidbody>(); // Rigidbodyコンポーネントを追加
            //rigid.isKinematic = true;       // 物理計算
            //rigid.useGravity  = false;      // 重力使用しない
            //rigid.angularDrag = 100f;       // 回転の空気抵抗
            // ここまで
            DontDestroyOnLoad(ChemicalBond);// Object 保持

            //長さの変更
            ChemicalBond.transform.localScale
                = new Vector3(ChemicalBond.transform.localScale.x,
                              distance / 2,
                              ChemicalBond.transform.localScale.z);

            Rigidbody rigid = ChemicalBond.AddComponent<Rigidbody>();
            FixedJoint joint = ChemicalBond.AddComponent<FixedJoint>(); // ここから
            rigid.isKinematic = false;
            rigid.useGravity = false;
            joint.connectedBody = obj1.GetComponent<Rigidbody>();    // ここまで
            joint.breakForce = 10000000000000f;
            joint.breakTorque = 10000000000000f;
        }
    }
}
