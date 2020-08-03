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
    static public float distCueCam = 9.7f;  // 97 cm

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
    //public static d2p luzeCornerAimBAK(float angleRad) {
    //    float x,z,k = 4 * angleRad / Mathf.PI; // 45dgr = Pi / 4
    //    if(angleRad <= 0) {
    //        z = zmax;
    //        x = xmax + k * BallR;
    //    } else {
    //        x = xmax;
    //        z = zmax - k * BallR;
    //    }
    //    return new d2p(x, z);
    //} // ////////////////////////////////////////////////////////////////////////////
    public static d2p luzeCornerAim(float angleRad) {
        float dx, dz;
        float k = luzeCornerWidth * 0.70711f;
        float q = 0.5f * (k + BallR);
        float a45 = Mathf.PI / 4;
        if(angleRad >= 0) {
            dx = q + (angleRad / a45) * (k - q);
            dz = q + (angleRad / a45) * (BallR - q);
        } else {
            dx = q + (angleRad / a45) * (q - BallR);
            dz = q + (angleRad / a45) * (q - k);
        }
        return new d2p(xmax - dx, zmax - dz);
    } // ////////////////////////////////////////////////////////////////////////////
    public static bool inField(float x, float z) {
        return x < xmax && z < zmax && x > -xmax && z > -zmax;
    } // /////////////////////////////////////////////////////////////////////////////
} // **********************************************************************************
