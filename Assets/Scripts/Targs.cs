using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Targ {
    public GameObject gobject;
    public Color clr = new Color();
    public Color clrFade = new Color();
    public bool selected;
    public d2p pnt;

    public Targ(GameObject game_object) {
        gobject = game_object;
        clr = gobject.GetComponent<Renderer>().material.color;
        clrFade = new Color(clr.r, clr.g, clr.b, 0.3f);
        selected = false;
        pnt = new d2p(gobject);
    } // //////////////////////////////////////////////////////////////////////////////
    public void reset() {
        gobject.GetComponent<Renderer>().material.color = clr;
        selected = false;
    } // /////////////////////////////////////////////////////////////////////////////
} // ***********************************************************************************
public class Targs {    // in Controller
    public List<Targ> v = new List<Targ>();

    public int truepos = 0;    // -1 0 +1
    public bool sucess = false;
    public Color clrSucess = new Color(1, 1, 0);
    public int selectLast = 0;

    public d2p pball = null;
    GameObject goBall = null;
    d2p ptarg = null;

    public Targs(GameObject ball, GameObject object_left, GameObject object_center, GameObject object_right) {
        goBall = ball;
        pball = new d2p(goBall);

        v.Add(new Targ(object_left));
        v.Add(new Targ(object_center));
        v.Add(new Targ(object_right));
        foreach(var q in v)
            q.reset();
    } // //////////////////////////////////////////////////////////////////////////////
    public int Reset(d2p p_targ, float dkcue) {
        ptarg = p_targ;
        foreach(var q in v)
            q.reset();
        truepos = Field.rand.Next(-1, 1);
        sucess = false;
        pball = new d2p(goBall);
        if(dkcue > 0) {
            float d = -Field.BallD * dkcue;
            switch(truepos) {
            case -1: {
                assign(0);
                assign(1, d);
                assign(2, (d + d));
                break;
            }
            case 0: {
                assign(0, -d);
                assign(1);
                assign(2, d);
                break;
            }
            case 1: {
                assign(0, -(d + d));
                assign(1, -d);
                assign(2);
                break;
            }
            default:
                break;
            }
        }
        return truepos;
    } // //////////////////////////////////////////////////////////////////////////////
    void assign(int n, float shift = 0f) {
        v[n].pnt = (shift == 0f) ? ptarg : d2p.addDist(pball, ptarg, shift);
        v[n].pnt.setObj(ref v[n].gobject);
    } // /////////////////////////////////////////////////////////////////////////////
    // return true if changed
    public bool setSelect(int select) {
        selectLast = select;
        sucess = (select == truepos);
        int idx = select + 1;
        Targ targ = v[idx];
        bool ret = targ.selected != true;
        if(ret) {
            targ.selected = true;
            Color clr = sucess ? clrSucess : targ.clrFade;
            targ.gobject.GetComponent<Renderer>().material.color = clr;
        }
        return ret;
    } // //////////////////////////////////////////////////////////////////////////////

} // ***********************************************************************************
