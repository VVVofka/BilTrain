using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCue : Ball {
    public float kBallStep = 0.125f;
    public float kBallMin;  //  1.5f * kBallStep;
    public float kBallMax;
    public float curK = 0;

    public BallCue() {
        kBallMin = 1.0f * kBallStep;
        kBallMax = 1.0f - kBallStep;

        distFromInD = 3.0f;  // in D ball
        distToInD = 9.0f;    // in D ball
        //distStep = 0.5f;  // in D ball
        curDistPhys = 0;       // phis
    } // ///////////////////////////////////////////////////////////////////////////////
    public void setRnd() {
        setKRnd();
        setDistRnd();
    } // ///////////////////////////////////////////////////////////////////////////////
    public void setVal(float k, float dist) {
        curK = k;
        curDistPhys = dist;
    } // ///////////////////////////////////////////////////////////////////////////////
    void setKRnd() {
        int nfrom = (int)(kBallMin / kBallStep);
        int nto = (int)(kBallMax / kBallStep);
        int n = Random.Range(nfrom, nto + 1);
        int sign = Random.Range(0, 1) * 2 - 1;
        curK = sign * n * kBallStep;
    } // /////////////////////////////////////////////////////////////////////////////
    public void dbg(string s = "") {
        Debug.Log(s + " k=" + curK.ToString() + " dist=" + curDistPhys);
    } // ///////////////////////////////////////////////////////////////////////////////
} // *****************************************************************************
