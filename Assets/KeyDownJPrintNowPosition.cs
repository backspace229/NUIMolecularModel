using UnityEngine;
using System.Collections;


/**
 * Jを押すとオブジェクト名表示
 * これを現在の原子を保存するプログラムに書き換える
 */
public class KeyDownJPrintNowPosition : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        // J入力した瞬間
        if (Input.GetKeyDown(KeyCode.J))
        {
            Debug.Log("push J_Key");
            GetFindObjectsOfType();

        }
	}

    void GetFindObjectsOfType()
    {
        // typeで指定した型のすべてのオブジェクトを配列で取得し、その要素数分繰り返す
        foreach (GameObject obj in UnityEngine.Object.FindObjectsOfType(typeof(GameObject)))
        {
            // シーン上に存在するオブジェクトならば処理
            if (obj.activeInHierarchy)
            {
                // GameObjectの名前を表示
                Debug.Log(obj.name);
            }
        }
    }
}
