using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEditor.Profiling.Memory.Experimental;
using UnityEngine;

public class Field {
    static public float xmax = 17.8f;
    static public float zmax = 8.85f;
    static public float BallD = 0.68f;
    static public float luzeCornerWidth = 0.73f;

    static float luzeCornerR = 0.05f;

    public static float BallR {get => BallD / 2f;}
    public static d2p endArc { get => new d2p(xmax + BallD, zmax + BallD); }

    public static d2p luzeCornerShortO {
        get {
            float x = xmax + luzeCornerR;
            float z = zmax + luzeCornerR - (luzeCornerWidth + 2 * luzeCornerR) * 0.7071067811865475244f;  // <0
            return new d2p(x, z);
        }
    }
    public static d2p luzeCornerLongO {
        get {
            float z = zmax + luzeCornerR;
            float x = xmax + luzeCornerR - (luzeCornerWidth + 2 * luzeCornerR) * 0.7071067811865475244f;  // <0
            return new d2p(x, z);
        }
    }
    public static d2p luzeCornerAimCenter {
        get {
            d2p shrt = luzeCornerShortO;
            d2p lng = luzeCornerLongO;
            return new d2p(0.5f * (shrt.x + lng.x), 0.5f * (shrt.z + lng.z));
        }
    }
    public static d2p luzeCornerAimLong { get => new d2p(xmax - BallD, zmax); }
    public static d2p luzeCornerAimShort { get => new d2p(xmax, zmax - BallD); }
    public static d2p luzeCornerAim(float angle) {
        float x,z,k = 4 * angle / Mathf.PI; // 45dgr = Pi / 4
        if(angle <= 0) {
            z = zmax;
            x = xmax + k * BallR;
        } else {
            x = xmax;
            z = zmax - k * BallR;
        }
        return new d2p(x, z);
    }

} // **********************************************************************************
