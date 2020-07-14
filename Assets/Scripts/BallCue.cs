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

        distFrom = 3.0f;  // in D ball
        distTo = 9.0f;    // in D ball
        distStep = 0.5f;  // in D ball
        curDist = 0;       // phis
    } // ///////////////////////////////////////////////////////////////////////////////
    public void setRnd() {
        curK = getK();
        curDist = getRnd(distFrom, distTo, distStep) * Field.BallD;
    } // ///////////////////////////////////////////////////////////////////////////////
    float getK() {
        int nfrom = (int)(kBallMin / kBallStep);
        int nto = (int)(kBallMax / kBallStep);
        int n = Random.Range(nfrom, nto + 1);
        return Mathf.Sign(Random.Range(-1, 1)) * n * kBallStep;
    } // /////////////////////////////////////////////////////////////////////////////
    public void dbg(string s = "") {
        Debug.Log(s + " k=" + curK.ToString() + " dist=" + curDist);
    } // ///////////////////////////////////////////////////////////////////////////////
} // *****************************************************************************
