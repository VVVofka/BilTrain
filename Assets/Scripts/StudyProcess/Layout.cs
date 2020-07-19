using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class Layout {
    public float distAim { get; private set; }
    public float distCue { get; private set; }
    public float angAimDeg { get; private set; }
    public float kCue { get; private set; }

    public Layout(float DistAim, float DistCue, float AngleAimDeg, float KoefCue) {
        distAim = DistAim;
        distCue = DistCue;
        angAimDeg = AngleAimDeg;
        kCue = KoefCue;
    } // ////////////////////////////////////////////////////////////////////
    public Layout(Layout from, Layout to) {
        distAim = UnityEngine.Random.Range(from.distAim, to.distAim) * Field.BallD;
        distCue = UnityEngine.Random.Range(from.distCue, to.distCue) * Field.BallD;
        angAimDeg = UnityEngine.Random.Range(from.angAimDeg, to.angAimDeg);
        kCue = UnityEngine.Random.Range(from.kCue, to.kCue);
    } // ////////////////////////////////////////////////////////////////////
    public bool EQ(Layout other) {
        return distAim == other.distAim && distCue == other.distCue &&
            angAimDeg == other.angAimDeg && kCue == other.kCue;
    } // ///////////////////////////////////////////////////////////////////////
} // ***************************************************************************
