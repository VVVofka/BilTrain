using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Luze {
    GameObject obj;
    public d2p pointLeft, pointRight, pointCenter;

    public Luze(GameObject obj_set) {
        obj = obj_set;
        pointCenter = new d2p(obj.transform.position);
        pointLeft = pointCenter.CreateDp(Mathf.PI + rad, len2); 
        pointRight = pointCenter.CreateDp(rad, len2);           

    } // //////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public float len2 { get => obj.transform.localScale.x / 2; }
    public float deg { get => obj.transform.rotation.eulerAngles.y; }
    public float rad { get => d2p.deg2rad(deg); }
} // **************************************************************
