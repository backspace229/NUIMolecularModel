using UnityEngine;
using System.Collections;
using Leap;

public class PinchTest : MonoBehaviour
{
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
        Debug.Log(pinchPercent + "+" + pinchPercent);
    }
}
