using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class Exercise : DKCue {
    public Layout layout;
    public Topic topic;
    public Intervals interval;

    public Exercise(Topic inTopic, Layout inLayout) {     // Topic inTopic
        topic = inTopic;
        layout = inLayout;
    } // ///////////////////////////////////////////////////////////////
    
    public int overdue(DateTime dt) { return interval.difH(dt); }
    public bool EQ(Exercise other) { return layout.EQ(other.layout); }
} // ************************************************************************************
