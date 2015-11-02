using UnityEngine;
using System.Collections;

public class ClickPositionCreatePrefabScript : MonoBehaviour {

    // 生成したいPrefab
    public GameObject Prefab;
    // クリックした位置座標
    private Vector3 clickPosition;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        // マウス入力で左クリックした瞬間
        if (Input.GetMouseButtonDown(0))
        {
            // Vector3でマウスがクリックした位置情報を取得する
            clickPosition = Input.mousePosition;
            // Z軸修正
            clickPosition.z = 10f;
            Instantiate(Prefab, Camera.main.ScreenToWorldPoint(clickPosition), Prefab.transform.rotation);
        }
	}
}
