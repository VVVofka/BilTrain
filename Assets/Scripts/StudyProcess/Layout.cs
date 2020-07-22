using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class Layout : DKCue {
    public float distAim { get; private set; }
    public float distCue { get; private set; }
    public float angAimDeg { get; private set; }
    public float kCue { get; set; }
    static Random rand = new Random();

    public Layout(float DistAim, float DistCue, float AngleAimDeg, float KoefCue) {
        distAim = DistAim;
        distCue = DistCue;
        angAimDeg = AngleAimDeg;
        kCue = KoefCue;
    } // ////////////////////////////////////////////////////////////////////
    public Layout(Layout from, Layout to) {
        distAim = rnd(from.distAim, to.distAim) * Field.BallD;
        distCue = rnd(from.distCue, to.distCue) * Field.BallD;
        angAimDeg = rnd(from.angAimDeg, to.angAimDeg);
        kCue = rnd(from.kCue, to.kCue);
    } // ////////////////////////////////////////////////////////////////////
    public bool EQ(Layout other) {
        return distAim == other.distAim && distCue == other.distCue &&
            angAimDeg == other.angAimDeg && kCue == other.kCue;
    } // ///////////////////////////////////////////////////////////////////////
    float rnd(float from, float to) {
        float rnd = (float)rand.NextDouble();
        return from + rnd * (to - from);
    } // /////////////////////////////////////////////////////////////////////////
} // ***************************************************************************
