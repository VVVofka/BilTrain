using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball {
    public float distFrom = 1.0f;  // in D ball
    public float distTo = 4.0f;    // in D ball
    //public float distStep = 0.5f;  // in D ball
    public float curDist = 0;       // phis

    //float xmax = 0, zmax = 0;
    
    public void setField (d2p bound, float ballDiam) {
        //xmax += Mathf.Abs(bound.x);
        //zmax += Mathf.Abs(bound.z);
    } // ///////////////////////////////////////////////////////////
    protected float getRnd(float min, float max, float step) {
        float lenhalf = Mathf.Abs( (max - min) / 2);
        int nmax = (int)(lenhalf / step);
        int n = Random.Range(0, nmax + 1);
        float d = Mathf.Sign(Random.Range(-1, 1)) * n * step;
        return min + lenhalf + d;
    } // /////////////////////////////////////////////////////////////////////
    protected float getDist() {
        return Random.Range(distFrom, distTo) * Field.BallD;
    } // ///////////////////////////////////////////////////////////////////////////////

} // *****************************************************************
