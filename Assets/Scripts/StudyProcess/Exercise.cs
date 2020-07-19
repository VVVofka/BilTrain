using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class Exercise : DKCue {
    public Layout layout;
    public Topic topic;
    public Intervals interval = new Intervals();

    //public Exercise() { }     // Topic inTopic
    public Exercise(Topic inTopic, Layout inLayout) {     // Topic inTopic
        topic = inTopic;
        layout = inLayout;
    } // ///////////////////////////////////////////////////////////////
    
    public int overdue(DateTime dt) { return interval.difH(dt); }
    public bool EQ(Exercise other) { return layout.EQ(other.layout); }
    public new void SetRes(bool sucess) {
        base.SetRes(sucess);
        topic.SetRes(sucess);
        if(sucess)
            setSucess();
        else
            setWrong();
    } // ///////////////////////////////////////////////////////////////////////////////////////
    void setSucess() {
        
    } // /////////////////////////////////////////////////////////////////
   void setWrong() {

    } // /////////////////////////////////////////////////////////////////
} // ************************************************************************************
