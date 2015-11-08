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
 */
public class KeyDownSetFunctionalGroup : MonoBehaviour
{
    public GameObject OxygenPrefab, HydrogenPrefab;
    int n = 0;  // OxygenObjectが呼び出されるとインクリメントする

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        float random_X, random_Y, random_Z;
        random_X = Random.Range(-1.0f, 1.0f);
        random_Y = Random.Range(-1.0f, 1.0f);
        random_Z = Random.Range(-1.0f, 1.0f);
        // スペースキー押す
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("push Space");
            GameObject Oxygen = Instantiate(OxygenPrefab, new Vector3(random_X, random_Y, random_Z), Quaternion.identity) as GameObject;
            Oxygen.name = "O";
            DontDestroyOnLoad(Oxygen);  // Sceneを切り替えてもObjectを保持
            n++;
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("push F");
            GameObject Hydrogen = Instantiate(HydrogenPrefab, new Vector3(random_X, random_Y, random_Z), Quaternion.identity) as GameObject;
            Hydrogen.name = "H";
            DontDestroyOnLoad(Hydrogen);  // Sceneを切り替えてもObjectを保持
            n++;
        }
        if (n > 10) {
            Application.LoadLevel("Export");    // Scene切り替え
        }
	}
}
