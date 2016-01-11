using UnityEngine;
using System.Collections;

public class BondTimer : MonoBehaviour {

    // Stopwatchオブジェクト作成    // Unity の Debug と競合するから長くしてる
    System.Diagnostics.Stopwatch BondTime = new System.Diagnostics.Stopwatch();

	// Use this for initialization
	void Start () {
        BondTime.Start();  // 開始
	}
	
	// Update is called once per frame
	void Update () {
        System.Threading.Thread.Sleep(1000);
        Debug.Log(BondTime.Elapsed);
        BondTime.Reset();
        BondTime.Start();
	}
}
