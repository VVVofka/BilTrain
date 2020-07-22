using System;

[Serializable]
public class Exercise : DKCue {
    public Layout layout;
    public Intervals interval = new Intervals();

    //public Exercise() { }     // Topic inTopic
    public Exercise(Layout inLayout) {     // Topic inTopic
        layout = inLayout;
    } // ///////////////////////////////////////////////////////////////
    
    public int overdue(DateTime dt) { return interval.difH(dt); }
    public bool EQ(Exercise other) { return layout.EQ(other.layout); }
} // ************************************************************************************
