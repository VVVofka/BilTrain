using UnityEngine;

public class BallAim : Ball{
    public float degMin = 5.0f;   // +- from normal calibr
    public float degMax = 30.0f;   // +- from normal calibr
    public float degStep = 5.0f;
    public float curDeg = 0;       
    public float curRad = 0;

    public BallAim() {
        distFromInD = 1.0f;  // in D ball
        distToInD = 5.0f;    // in D ball
        //distStep = 0.5f;  // in D ball
        curDistPhys = 0;       // phis
    } // ///////////////////////////////////////////////////////////////////////////////
    public void setRnd() {
        curDeg = getRnd(degMin, degMax, degStep);
        curRad = d2p.deg2rad(curDeg);
        setDistRnd();
    } // ///////////////////////////////////////////////////////////////////////////////
    public void setDeg(float deg, float distInD) {
        curDeg = deg;
        curRad = d2p.deg2rad(curDeg);
        curDistPhys = distInD;
    } // ///////////////////////////////////////////////////////////////////////////////
    public void dbg(string s = "") {
        Debug.Log(s + " deg=" + curDeg.ToString() + " dist=" + curDistPhys);
    } // ///////////////////////////////////////////////////////////////////////////////
} // ***************************************************************************************
