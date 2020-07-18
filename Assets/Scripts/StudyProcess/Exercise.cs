using System;
using System.Collections;
using System.Collections.Generic;

public class Exercise {
    public Layout layout;
    public Topic topic;
    public Intervals interval;
    public float dkcue = 1.0f;
    public int overdue(DateTime dt) { return interval.difH(dt); }
} // ************************************************************************************
