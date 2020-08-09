using System;
using UnityEngine;
[Serializable]
public class d2p {
    public float x;
    public float z;
    public d2p(float V = 0) { x = z = V; } // /////////////////////////////////////////////////////
    public d2p(d2p V) { x = V.x; z = V.z; } // ////////////////////////////////////////////////////
    public d2p(float X, float Z) { x = X; z = Z; } // /////////////////////////////////////////////
    public d2p(double X, double Z) { x = (float)X; z = (float)Z; } // /////////////////////////////
    public d2p(Vector3 other) { x = other.x; z = other.z; } // /////////////////////////////////////
    public d2p(GameObject other) { x = other.transform.position.x; z = other.transform.position.z; } //
    public void Set(float X, float Z) { x = X; z = Z; }
    public d2p CreateDp(float radian, float len) {
        float dz = len * Mathf.Sin(radian);
        float dx = len * Mathf.Cos(radian);
        return new d2p(x + dx, z - dz);
    } // //////////////////////////////////////////////////////////////////////////////////////////
    public float dist(d2p other) {
        float dx = x - other.x;
        float dz = z - other.z;
        return Mathf.Sqrt(dx * dx + dz * dz);
    } // //////////////////////////////////////////////////////////////////////////////////////////
    public float rad(d2p Base) {
        if(z == Base.z)
            return 0;
        return Mathf.Atan2(Base.z - z, x - Base.x);
    } // //////////////////////////////////////////////////////////////////////////////////////////
    // return /_ AOB
    static public float rad3(d2p A, d2p O, d2p B) {
        return B.rad(O) - A.rad(O);
    } // //////////////////////////////////////////////////////////////////////////////////////////
    public float deg(d2p Base) { return rad2deg(rad(Base)); }
    public static float rad2deg(float rad) { return 180f * rad / Mathf.PI; }
    public static float deg2rad(float deg) { return Mathf.PI * deg / 180f; }
    public static d2p rotate(float rad) {
        float dz = -Mathf.Sin(rad);
        float dx = Mathf.Cos(rad);
        return new d2p(dx, dz);
    } // //////////////////////////////////////////////////////////////////////////
    public static d2p rotateDeg(float deg) { return rotate(rad2deg(deg)); }
    public static d2p rotate(d2p Base, float rad, float len) {
        d2p p2 = rotate(rad);
        p2.x *= len;
        p2.z *= len;
        return p2.sum(Base);
    } // ////////////////////////////////////////////////////////////////////
    public static d2p sum(d2p p1, d2p p2) { return new d2p(p1.x + p2.x, p1.z + p2.z); }
    public d2p sum(d2p p1) { return new d2p(p1.x + x, p1.z + z); } // ///////////////
    public static d2p setDist(d2p baza, d2p other, float newsize) {
        float len = baza.dist(other);
        if(len == 0)
            return new d2p(baza);
        float k = newsize / len;
        float dx = k * (other.x - baza.x);
        float dz = k * (other.z - baza.z);
        return new d2p(baza.x + dx, baza.z + dz);
    } // ///////////////////////////////////////////////////////////////////////////
    public static d2p addDist(d2p baza, d2p other, float add) {
        float len = baza.dist(other);
        if(len == 0)
            return new d2p(other.x - add, other.z + add);
        float k = (len + add) / len;
        float dx = k * (other.x - baza.x);
        float dz = k * (other.z - baza.z);
        return new d2p(baza.x + dx, baza.z + dz);
    } // ///////////////////////////////////////////////////////////////////////////
    public d2p inv() { return new d2p(-x, -z); } // ////////////////////////////////////////////////
    public d2p invx() { return new d2p(-x, z); } // ////////////////////////////////////////////////
    public d2p invz() { return new d2p(x, -z); } // ////////////////////////////////////////////////
    public void setObj(ref GameObject obj) {
        obj.transform.position = new Vector3(x, obj.transform.position.y, z);
    } // //////////////////////////////////////////////////////////////////////////////////////////
    public void dbg(string s = "") {
        Debug.Log(s + x + "*" + z);
    } // /////////////////////////////////////////////////////////////////////////////////////////
    static public float normrad(float angl) {
        float pi2 = (2f * Mathf.PI);
        int n = (int)(angl / pi2);
        float ret = angl - n * pi2;
        if(ret > Mathf.PI)
            return ret - pi2;
        if(ret < -Mathf.PI)
            return ret + pi2;
        return ret;
    } // /////////////////////////////////////////////////////////////////////////////////////////
    static public bool Intersection(d2p a1, d2p a2, d2p b1, d2p b2, out d2p Cross) {
        d2p cross;
        Utils.Intersections.Info info;
        bool ret = Utils.Intersections.LineLine(a1, a2, b1, b2, out cross, out info);
        Cross = cross;
        return ret;
    } // ////////////////////////////////////////////////////////////////////////////////////////
    static public d2p rotateRefQQQ(d2p Base, d2p p, float rad) {
        float len = Base.dist(p);
        d2p nrm = new d2p(p.x - Base.x, p.z - Base.z);
        float alfa = -Mathf.Asin(nrm.z / len);
        float beta = alfa - rad;
        float x = len * Mathf.Cos(beta);
        float z = len * Mathf.Sin(beta);
        return new d2p(Base.x + x, Base.z + z);
    } // ////////////////////////////////////////////////////////////////////////////////////
    static public d2p rotateRef(d2p Base, d2p p, float rad) {
        d2p nrm = new d2p(p.x - Base.x, p.z - Base.z);
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);
        float x = +nrm.x * cos + nrm.z * sin;
        float z = -nrm.x * sin + nrm.z * cos;
        return new d2p(Base.x + x, Base.z + z);
    } // ////////////////////////////////////////////////////////////////////////////////////
    public string info { get => x + "*" + y; }
} // ********************************************************************************

//public class d2v {
//    public d2p beg;
//    public d2p end;

//    public d2v() { beg = new d2p(); end = new d2p(1f); }
//    public d2v(d2p Begin, d2p End) { beg = new d2p(Begin); end = new d2p(End); }
//    public d2v(d2v Other) { beg = new d2p(Other.beg); end = new d2p(Other.end); }
//    public d2v(float x0, float z0, float x1, float z1) { beg = new d2p(x0, z0); end = new d2p(x1, z1); }
//    public d2v(d2p begin, float radian, float len) {
//        beg = new d2p(begin);
//        float x = begin.x - len * Mathf.Cos(radian);
//        float z = begin.z + len * Mathf.Sin(radian);
//        end = new d2p(x, z);
//    } // //////////////////////////////////////////////////////////////////////////////////////////
//    public d2v(Vector3 begin, float radian, float len) {
//        beg = new d2p(begin);
//        float x = begin.x - len * Mathf.Cos(radian);
//        float z = begin.z + len * Mathf.Sin(radian);
//        end = new d2p(x, z);
//    } // //////////////////////////////////////////////////////////////////////////////////////////
//    public d2v(d2v other, float radian, float len) {
//        float radother = other.rad;
//        d2v ret = new d2v(other, radother + radian, len);
//    } // //////////////////////////////////////////////////////////////////////////////////////////
//    public float len {
//        get => end.dist(beg);
//        set {
//            float l = len;
//            if(l == 0)
//                return;
//            float k = value / l;
//            end.x = beg.x + k * (end.x - beg.x);
//            end.z = beg.z + k * (end.z - beg.z);
//        }
//    } // //////////////////////////////////////////////////////////////////////////////////////////
//    public float deg { get => end.deg(beg); }
//    public float rad { get => end.rad(beg); }

//    public d2p rotateRad(float new_rad) {
//        float newrad = rad + new_rad;
//        end.x = beg.x + len * Mathf.Cos(newrad);
//        end.z = beg.z + len * Mathf.Sin(newrad);
//        return end;
//    } // //////////////////////////////////////////////////////////////////////////////////////////
//    public d2p rotateDeg(float new_deg) {
//        float newrad = Mathf.PI * (deg + new_deg) / 180f;
//        return rotateRad(newrad);
//    } // //////////////////////////////////////////////////////////////////////////////////////////
//    public d2v invert() {
//        float dx =  end.x - beg.x;
//        float dz = end.z - beg.z;
//        return new d2v(beg, new d2p(beg.x - dx, beg.z - dz));
//    } // //////////////////////////////////////////////////////////////////////////////////////////
//    public void dbg(string s = "") {
//        Debug.Log(s + beg.x + "*" + beg.z + " -> " + end.x + "*" + end.z);
//    } // /////////////////////////////////////////////////////////////////////////////////////////
//} // *****************************************************************************************
