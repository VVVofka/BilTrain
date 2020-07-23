using System;

[Serializable]
public class Layout {
    public float distAimInD { get; private set; }
    public float distCueInD { get; private set; }
    public float angAimDeg { get; private set; }
    public float kCue { get; set; }
    static Random rand = new Random();

    public Layout(float DistAimInD, float DistCueInD, float AngleAimDeg, float KoefCue) {
        distAimInD = DistAimInD;
        distCueInD = DistCueInD;
        angAimDeg = AngleAimDeg;
        kCue = KoefCue;
    } // ////////////////////////////////////////////////////////////////////
    public Layout(Layout fromInD, Layout toInD) {
        distAimInD = rnd(fromInD.distAimInD, toInD.distAimInD) * Field.BallD;
        distCueInD = rnd(fromInD.distCueInD, toInD.distCueInD) * Field.BallD;
        angAimDeg = rnd(fromInD.angAimDeg, toInD.angAimDeg);
        kCue = rnd(fromInD.kCue, toInD.kCue);
    } // ////////////////////////////////////////////////////////////////////
    public bool EQ(Layout other) {
        return distAimInD == other.distAimInD && distCueInD == other.distCueInD &&
            angAimDeg == other.angAimDeg && kCue == other.kCue;
    } // ///////////////////////////////////////////////////////////////////////
    float rnd(float from, float to) {
        float rnd = (float)rand.NextDouble();
        return from + rnd * (to - from);
    } // /////////////////////////////////////////////////////////////////////////
} // ***************************************************************************
