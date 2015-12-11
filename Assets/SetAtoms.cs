using UnityEngine;
using System.Collections;

/**
 * 今: スペースキーを押すとランダム位置に原子出る
 * 
 * 目標:
 *   スペースを押したら官能基の選択肢出る
 *   クリックで選ぶ
 *   座標(0, 0, 0)に表示する
 *   マウスで移動できるようにする
 * 
 * いずれマウス操作をLeapMotion操作に置換する
 * 
 * Input.GetKeyで複数のキーが押されたときに判定することも可能
 * 
 * 固定ジョイント
 * http://docs.unity3d.com/ja/current/Manual/class-FixedJoint.html
 */
public class SetAtoms : MonoBehaviour {
    public GameObject O, H, C;
    GameObject obj;
    Rigidbody rigid;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

        float random_X, random_Y, random_Z;
        random_X = Random.Range(-5.0f, 5.0f);
        random_Y = Random.Range(-5.0f, 5.0f);
        random_Z = Random.Range(-5.0f, 5.0f);

        // 
        if (Input.GetKeyDown(KeyCode.O))
        {
            Debug.Log("push O-key");
            CreateAtoms("O", new Vector3(random_X, random_Y, random_Z));
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            Debug.Log("push H-key");
            CreateAtoms("H", new Vector3(random_X, random_Y, random_Z));
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("push C-key");
            CreateAtoms("C", new Vector3(random_X, random_Y, random_Z));
        }

        if (Input.GetKeyDown(KeyCode.Return)) {
            Application.LoadLevel("Export");    // Scene切り替え
        }
	}

    public void CreateAtoms(string AtomName, Vector3 Vec3)
    {
        switch (AtomName)
        {
            case "H":
                obj = Instantiate(H, Vec3, Quaternion.identity) as GameObject;
                break;
            case "O":
                obj = Instantiate(O, Vec3, Quaternion.identity) as GameObject;
                break;
            case "C":
                obj = Instantiate(C, Vec3, Quaternion.identity) as GameObject;
                break;
        }
        //switch (key)
        //{
        //    case KeyCode.O:
        //        break;
        //}
        obj.name = AtomName;    // 名前を変更
        rigid = obj.AddComponent<Rigidbody>();  // Rigidbodyコンポーネントを追加
        rigid.isKinematic = false;   // 物理計算しない
        rigid.useGravity  = false;  // 重力使用しない
        rigid.drag        = 10f;    // 空気抵抗の大きさ
        rigid.angularDrag = 10f; // 回転の空気抵抗
        DontDestroyOnLoad(obj);     // Scene を切り替えても Object を保持

    }
}
