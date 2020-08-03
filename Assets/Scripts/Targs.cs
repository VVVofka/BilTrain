using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Targ{
    public GameObject gobject;
    public Color clr = new Color();
    public Targ(GameObject game_object, Color color) {
        gobject = game_object;
        clr = color;
    } // //////////////////////////////////////////////////////////////////////////////
} // ***********************************************************************************
public class Targs {
    public List<Targ> v = new List<Targ>();
    
    public Targs(GameObject object_left, Color color_left,
        GameObject object_center, Color color_center,
        GameObject object_right, Color color_right) {
        v.Add(new Targ(object_left, color_left));
        v.Add(new Targ(object_center, color_center));
        v.Add(new Targ(object_right, color_right));
    } // //////////////////////////////////////////////////////////////////////////////
} // ***********************************************************************************
