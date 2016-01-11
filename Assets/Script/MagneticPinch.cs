/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

using UnityEngine;
using System.Collections;
using Leap;

/** 
 * Detects pinches and grabs the closest rigidbody if it's within a given range.
 * 
 * Attach this script to the physics hand object assinged to the HandController in a scene.
 */
public class MagneticPinch : MonoBehaviour
{

    public const float TRIGGER_DISTANCE_RATIO = 0.7f;   // ひきつける距離?

    /** The stiffness of the spring force used to move the object toward the hand. */
    public float forceSpringConstant = 100.0f;
    /** The maximum range at which an object can be picked up.*/
    public float magnetDistance = 2.0f; // 磁力の有効距離?

    protected bool pinching_;   // 掴んでいるか(?)
    protected Collider grabbed_;// 握っているか(?)

    void Start() {
        pinching_ = false;
        grabbed_ = null;
    }

    /** Finds an object to grab and grabs it. */
    void OnPinch(Vector3 pinch_position)
    {
        //Debug.Log("OnPinch");

        pinching_ = true;

        // Check if we pinched a movable object and grab the closest one that's not part of the hand.
        Collider[] close_things = Physics.OverlapSphere(pinch_position, magnetDistance);
        Vector3 distance = new Vector3(magnetDistance, 0.0f, 0.0f);

        for (int j = 0; j < close_things.Length; ++j)
        {
            Vector3 new_distance = pinch_position - close_things[j].transform.position;
            if (close_things[j].GetComponent<Rigidbody>() != null && new_distance.magnitude < distance.magnitude &&
                !close_things[j].transform.IsChildOf(transform))
            {
                grabbed_ = close_things[j];
                distance = new_distance;
            }
        }
    }

    /** Clears the pinch state. */
    void OnRelease()
    {
        //Debug.Log("OnRelease");

        grabbed_ = null;
        pinching_ = false;
    }

    /**
     * Checks whether the hand is pinching and updates the position of the pinched object.
     */
    void Update() {
        // 手の情報を取得
        HandModel hand_model = GetComponent<HandModel>();
        Hand leap_hand = hand_model.GetLeapHand();

        if (leap_hand == null)
            return;

        // Scale trigger distance by thumb proximal bone length.
        // 親指の指先の位置
        Vector leap_thumb_tip = leap_hand.Fingers[0/*親指*/].TipPosition;
        // ピンチを判断するためにトリガとなる距離は
        // 親指の付け根から第一関節までの距離を基準に計算する
        float proximal_length = leap_hand.Fingers[0].Bone(Bone.BoneType.TYPE_PROXIMAL).Length;
        float trigger_distance = proximal_length * TRIGGER_DISTANCE_RATIO;


        // Check thumb tip distance to joints on all other fingers.
        // If it's close enough, start pinching.
        // 親指以外の指の関節位置と親指の指先の位置の距離を調べて、
        // 鳥がとなる値よりも近かったらピンチフラグをONにする
        bool trigger_pinch = false;
        for (int i = 1; i < HandModel.NUM_FINGERS && !trigger_pinch; ++i)
        {
            Finger finger = leap_hand.Fingers[i];

            for (int j = 0; j < FingerModel.NUM_BONES && !trigger_pinch; ++j)
            {
                Vector leap_joint_position = finger.Bone((Bone.BoneType)j).NextJoint;
                if (leap_joint_position.DistanceTo(leap_thumb_tip) < trigger_distance)
                    trigger_pinch = true;
            }
        }

        // ピンチしている位置を吸着点とする
        // この際、計算していた座標を Leap の世界から Unity の世界の座標に変換
        Vector3 pinch_position = hand_model.fingers[0].GetTipPosition();

        // Only change state if it's different.
        // ピンチしている最中でない && ピンチを検知したら OnPinch を発火
        // 逆の場合は OnRelease を発火
        if (trigger_pinch && !pinching_)
            OnPinch(pinch_position);
        else if (!trigger_pinch && pinching_)
            OnRelease();

        // Accelerate what we are grabbing toward the pinch.
        if (grabbed_ != null)
        {
            Vector3 distance = pinch_position - grabbed_.transform.position;
            grabbed_.GetComponent<Rigidbody>().AddForce(forceSpringConstant * distance);
        }
    }
}
