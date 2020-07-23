//using UnityEngine;

//public class Ball {
//    public float distFromInD = 1.0f;  // in D ball
//    public float distToInD = 4.0f;    // in D ball
//    //public float distStep = 0.5f;  // in D ball
//    public float curDistPhys = 0;       // phis
//    public float distInD { get => curDistPhys / Field.BallD; }    // in D ball

//    static protected float getRnd(float min, float max, float step) {
//        float lenhalf = Mathf.Abs( (max - min) / 2);
//        int nmax = (int)(lenhalf / step);
//        int n = Random.Range(0, nmax + 1);
//        float d = Mathf.Sign(Random.Range(-1, 1)) * n * step;
//        return min + lenhalf + d;
//    } // /////////////////////////////////////////////////////////////////////
//    protected void setDistRnd() {
//        curDistPhys = Random.Range(distFromInD, distToInD) * Field.BallD;
//    } // ///////////////////////////////////////////////////////////////////////////////

//} // *****************************************************************
