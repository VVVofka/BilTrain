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
        //distAim = DistAim;
        //distCue = DistCue;
        //angAimDeg = AngleAimDeg;
        //kCue = KoefCue;
    } // ////////////////////////////////////////////////////////////////////
    public bool EQ(Layout other) {
        return distAim == other.distAim && distCue == other.distCue &&
            angAimDeg == other.angAimDeg && kCue == other.kCue;
    } // ///////////////////////////////////////////////////////////////////////
} // ***************************************************************************
