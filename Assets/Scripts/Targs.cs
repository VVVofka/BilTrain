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

    public Targs(GameObject object_left, GameObject object_center, GameObject object_right) {
        v.Add(new Targ(object_left));
        v.Add(new Targ(object_center));
        v.Add(new Targ(object_right));
        Reset(null, -1f);
    } // //////////////////////////////////////////////////////////////////////////////
    public int Reset(d2p pTarg, float k_cue) {
        foreach(var q in v)
            q.reset();
        truepos = Field.rand.Next(-1, 1);
        sucess = false;
        if(pTarg != null) {
            switch(truepos) {
            case -1: {
                lay.pTarg.setObj(ref aimLeft);
                d2p ptarg1 = d2p.addDist(lay.paim, lay.pTarg, -Field.BallD * dkcue);
                ptarg1.setObj(ref aimCenter);
                d2p ptarg2 = d2p.addDist(lay.paim, lay.pTarg, -2 * Field.BallD * dkcue);
                ptarg2.setObj(ref aimRight);
                break;
            }
            case 0: {
                v[1].pnt.setObj(ref v[1].gobject);
                v[0].pnt = d2p.addDist(lay.paim, lay.pTarg, Field.BallD * dkcue);
                ptarg1.setObj(ref aimLeft);
                d2p ptarg2 = d2p.addDist(lay.paim, lay.pTarg, -Field.BallD * dkcue);
                ptarg2.setObj(ref aimRight);
                break;
            }
            case 1: {
                lay.pTarg.setObj(ref aimRight);
                d2p ptarg1 = d2p.addDist(lay.paim, lay.pTarg, 2 * Field.BallD * dkcue);
                ptarg1.setObj(ref aimLeft);
                d2p ptarg2 = d2p.addDist(lay.paim, lay.pTarg, Field.BallD * dkcue);
                ptarg2.setObj(ref aimCenter);
                break;
            }
            default:
                break;
            }
        }
        return truepos;
    } // //////////////////////////////////////////////////////////////////////////////
    // return true if changed
    public bool setSelect(int select) {
        selectLast = select;
        sucess = (select == truepos);
        int idx = select + 1;
        Targ targ = v[idx];
        bool ret = targ.selected != true;
        if(ret) {
            targ.selected = true;
            targ.clr = sucess ? clrSucess : targ.clrFade;
        }
        return ret;
    } // //////////////////////////////////////////////////////////////////////////////

} // ***********************************************************************************
