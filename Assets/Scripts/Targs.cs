using UnityEngine;
using System.Collections.Generic;

public class Targ {
    public GameObject gobject;
    public Color clrSel = new Color();
    public Color clrUnSel;
    public Color clrFade = new Color();
    public d2p pnt;
    public bool selected;   // is was select

    public Targ(GameObject game_object) {
        gobject = game_object;
        clrSel = gobject.GetComponent<Renderer>().material.color;
        float lUnSel = 0.7f;
        clrUnSel = new Color(clrSel.r * lUnSel, clrSel.g * lUnSel, clrSel.b * lUnSel);
        clrFade = new Color(clrSel.r, clrSel.g, clrSel.b, 0.3f);
        pnt = new d2p(gobject);
        selected = false;
    } // //////////////////////////////////////////////////////////////////////////////
    public void reset() {
        gobject.GetComponent<Renderer>().material.color = clrUnSel;
        selected = false;
    } // /////////////////////////////////////////////////////////////////////////////
    public string info() {
        return selected ? "+" : "-" + " pnt:" + pnt.info;
    } // /////////////////////////////////////////////////////////////////////////////
} // ***********************************************************************************
public class Targs {    // in Controller
    public List<Targ> v = new List<Targ>();

    public int truepos;    // -1 0 +1
    public int selectLast;
    public bool sucess;
    public bool firstSelectItem;
    public bool seriesSucess;
    public Color clrSucess;
    public Color clrInvisible;
    public int cntSelect;
    public bool changePos;

    public d2p pball;
    GameObject goBall;
    d2p ptarg;

    public Targs(GameObject ball, GameObject object_left, GameObject object_center, GameObject object_right) {
        goBall = ball;
        pball = new d2p(goBall);
        truepos = selectLast = -2;
        sucess = seriesSucess = false;
        clrSucess = new Color(1, 1, 0);
        clrInvisible = new Color(0, 0, 0, 0);
        cntSelect = 0;
        firstSelectItem = true;

        v.Add(new Targ(object_left));
        v.Add(new Targ(object_center));
        v.Add(new Targ(object_right));
        foreach(var q in v)
            q.reset();
    } // //////////////////////////////////////////////////////////////////////////////
    public int Preset(d2p p_targ, float dkcue) {    //  call in waitSetBalls
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
        selectLast = -2;
        changePos = false;
        firstSelectItem = true;
        return truepos;
    } // //////////////////////////////////////////////////////////////////////////////
    void assign(int n, float shift = 0f) {
        Targ t = v[n];
        t.pnt = (shift == 0f) ? ptarg : d2p.addDist(pball, ptarg, shift);
        t.pnt.setObj(ref t.gobject);
    } // /////////////////////////////////////////////////////////////////////////////
    public bool setSelect(int select) {     // call in waitTakeAim
        changePos = (selectLast != select);
        selectLast = select;
        //sucess = (select == truepos);
        cntSelect = 0;
        foreach(var q in v)
            if(!q.selected)
                q.gobject.GetComponent<Renderer>().material.color = (q == targ) ? q.clrSel : q.clrUnSel;
        return changePos;
    } // /////////////////////////////////////////////////////////////////////////////
    // return true if end of attempt (any sucess)
    public bool setChoise(int select) {     // call in waitChoice
        changePos = (selectLast != select);
        if(changePos) {
            setSelect(select);
            return false;
        } else {        //  select == selectLast
            sucess = (select == truepos);
            if(++cntSelect == 1)
                seriesSucess = sucess;
            //if(firstSelectItem) {
                //firstSelectItem = false;
                if(sucess) {
                    foreach(var q in v)
                        q.gobject.GetComponent<Renderer>().material.color = (q == targ) ? clrSucess : clrInvisible;
                } else {
                    targ.gobject.GetComponent<Renderer>().material.color = targ.clrFade;
                }
            //}
            targ.selected = true;
        }
        return sucess;
    } // //////////////////////////////////////////////////////////////////////////////
    public Targ targ { get => v[selectLast + 1]; } // /////////////////////////////////
    public string info(string s0) {
        UnityEngine.Debug.Log(s0 + "start:" +
            "truepos=" + truepos +
            " selectLast=" + selectLast +
            " sucess=" + sucess +
            " firstSelectItem=" + firstSelectItem +
            " seriesSucess=" + seriesSucess +
            " cntSelect=" + cntSelect
            );
        string s3 = s0;
        for(int j = 0; j < v.Count; j++) {
            string s = v[j].info();
            s3 += " " + (j - 1).ToString() + ":" + s + ";";
            //UnityEngine.Debug.Log(s0 + " ->" + j.ToString() + ": " + s + ((j==v.Count-1)? " End Targs:":""));
        }
        UnityEngine.Debug.Log(s3);
        return s0;
    } // //////////////////////////////////////////////////////////////////////////////
} // ***********************************************************************************
