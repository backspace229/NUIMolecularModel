using UnityEngine;
using System.Collections;
using Leap;

public class MagneticPinchTest : MonoBehaviour {

    private Controller controller = new Controller();

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        Frame frame = controller.Frame();
        HandList hands = frame.Hands;

        Hand hand = hands[0];
        float pinchPercent = hand.PinchStrength;

        foreach (Finger finger in frame.Fingers)
        {/*
            Debug.Log("Type: " + finger.Type() +        // 種類
                "\nTipPosition: " + finger.TipPosition +// 位置
                "\nTipVelocity: " + finger.TipVelocity +// 速度
                "\nDirection: " + finger.Direction);    // 向き*/

            // 人差し指でピンチしたときに判定されるようにしたいけど
            // 他の指でも判定されてしまう(まぁいいけど)
            if (0.8 < pinchPercent && Finger.FingerType.TYPE_INDEX == finger.Type())
            {
                Debug.Log("<color=red>hello: " + finger.Type() + "</color>");

            }
            else
            {
                Debug.Log("None");
            }
        }
	}
}
