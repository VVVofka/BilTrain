using System;
using UnityEngine;
public class fpair {
    public float a, b;
    public fpair(float A=0, float B=0) { a = A; b = B; }
}
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
        fpair alfa = getAlfa(kd, distCuePhys);
        d2p ptarg1 = d2p.rotateRef(paim, pvir, Mathf.Sign(kd) * Mathf.PI / 2 - alfa.a); ;
        d2p ptarg2 = d2p.rotateRef(paim, pvir, Mathf.Sign(kd) * Mathf.PI / 2 - alfa.b); ;
        //pTarg = d2p.rotateRef(paim, pvir, Mathf.Sign(kd) * Mathf.PI / 2 - alfa);
        ptarg1 = d2p.setDist(paim, ptarg1, kd);
        ptarg2 = d2p.setDist(paim, ptarg2, kd);
        pTarg = ptarg1;
        //pTarg = d2p.setDist(paim, pTarg, kd);

        // Cue ball
        d2p pcue1 = d2p.rotateRef(paim, pvir, -alfa.a);
        d2p pcue2 = d2p.rotateRef(paim, pvir, -alfa.b);
        //pcue = d2p.rotateRef(paim, pvir, -alfa);
        pcue1 = d2p.setDist(paim, pcue1, distCuePhys);
        pcue2 = d2p.setDist(paim, pcue2, distCuePhys);
        pcue = pcue1;
        //pcue = d2p.setDist(paim, pcue, distCuePhys);
        if(isOutRange(pcue))
            return false;
        return true;
    } // ////////////////////////////////////////////////////////////////////////////////////////
    public bool SetBandRnd(int signAngAim, int signKCue) {
        for(int j = 0; j < 128; j++) {
            angAimDeg = okr( rnd(_fromInD.angAimDeg, _toInD.angAimDeg) * Math.Sign(signAngAim));
            distAimInD = okr(rnd(_fromInD.distAimInD, _toInD.distAimInD));
            distCueInD = okr(rnd(_fromInD.distCueInD, _toInD.distCueInD));
            kCue = okr(rnd(_fromInD.kCue, _toInD.kCue) * Math.Sign(signKCue));
            bool bsuccess = Set();
            if(bsuccess)
                return true;
        }
        return false;
    } // ////////////////////////////////////////////////////////////////////
    float okr(float x) { return (int)(x * 10000) / 10000.0f; }
    bool isOutRange(d2p p) {
        float xr = p.x + Field.BallR;
        float zr = p.z + Field.BallR;
        return !Field.inField(xr, zr);
    } // /////////////////////////////////////////////////////////////////////////////////
    public bool EQ(Layout other) {
        return Mathf.Approximately(distAimInD,other.distAimInD) &&
            Mathf.Approximately(distCueInD, other.distCueInD) &&
            Mathf.Approximately(angAimDeg, other.angAimDeg) &&
            Mathf.Approximately(kCue, other.kCue);
    } // ///////////////////////////////////////////////////////////////////////
    float rnd(float from, float to) {
        float rnd = (float)Field.rand.NextDouble();
        return from + rnd * (to - from);
    } // /////////////////////////////////////////////////////////////////////////
     fpair getAlfa(float kD, float distCue) {
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
        return new fpair(Mathf.Acos(quadro.res1 / Field.BallD), Mathf.Acos(quadro.res2 / Field.BallD));
        //return Mathf.Acos(res / Field.BallD);
    } // ////////////////////////////////////////////////////////////////////////////////
    public string info { get => "aim:" + paim.x + "*" + paim.z + " cue:" + pcue.x + "*" + pcue.z; }
} // ***************************************************************************
