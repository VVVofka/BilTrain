using System;
using System.Collections;
using System.Collections.Generic;

public class Exercise : DKCue {
    public Layout layout;
    public Topic topic;
    public Intervals interval;
    public int overdue(DateTime dt) { return interval.difH(dt); }
} // ************************************************************************************
