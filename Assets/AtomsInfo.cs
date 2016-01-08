using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AtomsInfo : MonoBehaviour {

    /// <summary>
    /// 原子から伸びている結合数
    /// </summary>
    public int bondsNum = 0;
    public List<string> childName = new List<string>();

    public AtomsInfo()
    {
    }

}
