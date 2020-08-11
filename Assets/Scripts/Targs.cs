using UnityEngine;
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
    public string info(int n) {
        return n.ToString() + " " + selected.ToString() + " pnt:" + pnt.info;
    }
} // ***********************************************************************************
public class Targs {    // in Controller
    public List<Targ> v = new List<Targ>();

    public int truepos;    // -1 0 +1
    public int selectLast;
    public int selectPrev;
    public bool sucess;
    public bool firstSelectItem;
    public bool seriesSucess;
    public Color clrSucess;
    public Color clrInvisible;
    public int cntSelect;

    public d2p pball;
    GameObject goBall;
    d2p ptarg;

    public Targs(GameObject ball, GameObject object_left, GameObject object_center, GameObject object_right) {
        goBall = ball;
        pball = new d2p(goBall);
        truepos = selectLast = selectPrev = -2;
        sucess = firstSelectItem = seriesSucess = false;
        clrSucess = new Color(1, 1, 0);
        clrInvisible = new Color(0, 0, 0, 0);
        cntSelect = 0;

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
        sucess = seriesSucess = false;
        pball = new d2p(goBall);
        cntSelect = 0;
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
        selectPrev = selectLast = -2;
        return truepos;
    } // //////////////////////////////////////////////////////////////////////////////
    void assign(int n, float shift = 0f) {
        v[n].pnt = (shift == 0f) ? ptarg : d2p.addDist(pball, ptarg, shift);
        v[n].pnt.setObj(ref v[n].gobject);
    } // /////////////////////////////////////////////////////////////////////////////
    // return true if end of attempt (any sucess)
    public bool setSelect(int select) {
        sucess = (select == truepos);
        if(++cntSelect == 1)
            seriesSucess = sucess;
        selectPrev = selectLast;
        selectLast = select;
        int idx = select + 1;
        Targ targ = v[idx];
        firstSelectItem = !targ.selected;
        if(firstSelectItem) {
            targ.selected = true;
            if(sucess) {
                foreach(var q in v)
                    q.gobject.GetComponent<Renderer>().material.color = (q == targ) ? clrSucess : clrInvisible;
            } else {
                targ.gobject.GetComponent<Renderer>().material.color = targ.clrFade;
            }
        }
        return seriesSucess;
    } // //////////////////////////////////////////////////////////////////////////////
    public Targ targ { get => v[selectLast + 1]; } // /////////////////////////////////
    public string info(string s0) {
        UnityEngine.Debug.Log(s0 + "start:" +
            "truepos=" + truepos +
            " selectLast=" + selectLast +
            " selectPrev=" + selectPrev +
            " sucess=" + sucess +
            " firstSelectItem=" + firstSelectItem +
            " seriesSucess=" + seriesSucess +
            " cntSelect=" + cntSelect
            );
        for(int j = 0; j < v.Count; j++) {
            string s = v[j].info(j);
            UnityEngine.Debug.Log(s0 + " ->" + j.ToString() + ": " + s + ((j==v.Count-1)? " End Targs:":""));
        }
        return s0;
    } // //////////////////////////////////////////////////////////////////////////////
} // ***********************************************************************************
