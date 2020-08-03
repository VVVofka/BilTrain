using System;
using UnityEngine;

[Serializable]
public class Layout {
    public float distAimInD;
    public float distCueInD;
    public float angAimDeg;
    public float kCue;

    public d2p pluze;
    public d2p paim;
    public d2p pvir;
    public d2p pcue;
    public d2p pTarg;

    public float angAimRad { get => d2p.deg2rad(angAimDeg); }
    public float distAimPhys { get => distAimInD * Field.BallD; }
    public float distCuePhys { get => distCueInD * Field.BallD; }

    Layout _fromInD, _toInD;
    static System.Random rand = new System.Random();

    public Layout(float DistAimInD, float DistCueInD, float AngleAimDeg, float KoefCue) {
        distAimInD = DistAimInD;
        distCueInD = DistCueInD;
        angAimDeg = AngleAimDeg;
        kCue = KoefCue;
    } // ////////////////////////////////////////////////////////////////////
    public Layout(Layout fromInD, Layout toInD) {
        _fromInD = fromInD;
        _toInD = toInD;
    } // ////////////////////////////////////////////////////////////////////
    //void tst() {
    //    d2p bs = new d2p(-1.5f, 3);
    //    d2p p0 = new d2p(5, -1);
    //    d2p p;
    //    float rad = d2p.deg2rad(80);

    //    p = d2p.rotateRef(bs, p0, rad);
    //} // /////////////////////////////////////////////////////////////////////////////////
    bool Set() {
        //tst();
        // Target point
        d2p pluzeabs = Field.luzeCornerAim(0f);

        // Aim ball
        paim = new d2p(Field.xmax, Field.zmax);
        paim = d2p.rotateRef(pluzeabs, paim, Mathf.PI + angAimRad);
        paim = d2p.setDist(pluzeabs, paim, distAimPhys);
        if(isOutRange(paim))
            return false;

        pluze = Field.luzeCornerAim(angAimRad);
        // Virtual ball
        float virdist = pluze.dist(paim) + Field.BallD;
        pvir = d2p.setDist(pluze, paim, virdist);
        if(isOutRange(pvir))
            return false;

        // Target point
        float kd = kCue * Field.BallD;
        float alfa = getAlfa(kd, distCuePhys);
        pTarg = d2p.rotateRef(paim, pvir, Mathf.Sign(kd) * Mathf.PI / 2 - alfa);
        pTarg = d2p.setDist(paim, pTarg, kd);

        // Cue ball
        pcue = d2p.rotateRef(paim, pvir, -alfa);
        pcue = d2p.setDist(paim, pcue, distCuePhys);
        if(isOutRange(pcue))
            return false;
        return true;
    } // ////////////////////////////////////////////////////////////////////////////////////////
    public bool SetBandRnd(int signAngAim, int signKCue) {
        for(int j = 0; j < 128; j++) {
            angAimDeg = rnd(_fromInD.angAimDeg, _toInD.angAimDeg) * Math.Sign(signAngAim);
            distAimInD = rnd(_fromInD.distAimInD, _toInD.distAimInD);
            distCueInD = rnd(_fromInD.distCueInD, _toInD.distCueInD);
            kCue = rnd(_fromInD.kCue, _toInD.kCue) * Math.Sign(signKCue);
            bool bsuccess = Set();
            if(bsuccess)
                return true;
        }
        return false;
    } // ////////////////////////////////////////////////////////////////////
    bool isOutRange(d2p p) {
        float xr = p.x + Field.BallR;
        float zr = p.z + Field.BallR;
        return !Field.inField(xr, zr);
    } // /////////////////////////////////////////////////////////////////////////////////
    public bool EQ(Layout other) {
        return distAimInD == other.distAimInD && distCueInD == other.distCueInD &&
            angAimDeg == other.angAimDeg && kCue == other.kCue;
    } // ///////////////////////////////////////////////////////////////////////
    float rnd(float from, float to) {
        float rnd = (float)rand.NextDouble();
        return from + rnd * (to - from);
    } // /////////////////////////////////////////////////////////////////////////
    float getAlfa(float kD, float distCue) {
        float kD2 = kD * kD;
        float a = 1 + kD2 / (distCue * distCue);
        float b = -2 * kD2 / distCue;
        float c = kD2 - Field.BallD * Field.BallD;
        Utils.Squdre quadro = new Utils.Squdre(a, b, c);
        float res = float.NaN;
        if(quadro.cnt == 2) {
            if(quadro.res1 >= 0 && quadro.res2 < 0)
                res = quadro.res1;
            else if(quadro.res2 >= 0 && quadro.res1 < 0)
                res = quadro.res2;
        } else if(quadro.cnt == 1) {
            res = quadro.res1;
        }
        return Mathf.Acos(res / Field.BallD);
    } // ////////////////////////////////////////////////////////////////////////////////
} // ***************************************************************************
