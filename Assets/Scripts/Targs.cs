using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Targ{
    public GameObject gobject;
    public Color clr = new Color();
    public Color clrFade = new Color();
    public bool selected;

    public Targ(GameObject game_object) {
        gobject = game_object;
        clr = gobject.GetComponent<Renderer>().material.color;
        clrFade = new Color(clr.r, clr.g, clr.b, 0.3f);
        selected = false;
    } // //////////////////////////////////////////////////////////////////////////////
    public void reset() {
        gobject.GetComponent<Renderer>().material.color = new Color(clr.r, clr.g, clr.b);
        selected = false;
    } // /////////////////////////////////////////////////////////////////////////////
} // ***********************************************************************************
public class Targs {    // in Controller
    public List<Targ> v = new List<Targ>();
    public int truepos = 0;    // -1 0 +1
    public bool sucess = false;
    public Color clrSucess = new Color(1, 1, 0);
    public int selectLast = 0;
    
    public Targs(GameObject object_left,
        GameObject object_center, 
        GameObject object_right ) {
        v.Add(new Targ(object_left));
        v.Add(new Targ(object_center));
        v.Add(new Targ(object_right));
        Reset();
    } // //////////////////////////////////////////////////////////////////////////////
    public int Reset() {
        foreach(var q in v) 
            q.reset();
        truepos = Field.rand.Next(-1, 1);
        sucess = false;
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
