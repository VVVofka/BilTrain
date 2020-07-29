using System;
using UnityEngine;

[Serializable]
public class Layout {
    public float distAimInD { get; private set; }
    public float distCueInD { get; private set; }
    public float angAimDeg { get; private set; }
    public float kCue { get; set; }

    public d2p pluze { get; private set; }
    public d2p paim { get; private set; }
    public d2p pvirt { get; private set; }
    public d2p ppcue { get; private set; }
    public d2p ptargCentre { get; private set; }
    public d2p ptargLeft { get; private set; }
    public d2p ptargRight { get; private set; }

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
        //distAimInD = rnd(fromInD.distAimInD, toInD.distAimInD);
        //distCueInD = rnd(fromInD.distCueInD, toInD.distCueInD);
        //angAimDeg = rnd(fromInD.angAimDeg, toInD.angAimDeg);
        //kCue = rnd(fromInD.kCue, toInD.kCue);
    } // ////////////////////////////////////////////////////////////////////
    public bool Set() {
        int j = 128;
        do {
            // Target point
            angAimDeg = rnd(_fromInD.angAimDeg, _toInD.angAimDeg);
            pluze = Field.luzeCornerAim(angAimRad);

            // Aim ball
            paim = d2p.rotate(pluze, (3f / 4f) * Mathf.PI + angAimRad, distAimPhys);
            if(isOutRange(paim))
                continue;

            // Virtual ball
            float virdist = pluze.dist(paim) + Field.BallD;
            d2p pvir = d2p.setDist(pluze, paim, virdist);
            if(isOutRange(pvir))
                continue;

            distAimInD = rnd(_fromInD.distAimInD, _toInD.distAimInD);
            distCueInD = rnd(_fromInD.distCueInD, _toInD.distCueInD);
            kCue = rnd(_fromInD.kCue, _toInD.kCue);

            float kd = kCue * Field.BallD;
            float alfa = getAlfa(kd, distCuePhys);

            break;
        } while(--j > 0);
        return j > 0;
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
