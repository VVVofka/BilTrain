using System;
using System.Diagnostics;

[Serializable]
public class Exercise : DKCue {
    public Layout layout;
    public TimeIntervals interval = new TimeIntervals();
    bool nowrong = true;

    //public Exercise() { }     // Topic inTopic
    public Exercise(Layout inLayout) {     // Topic inTopic
        layout = inLayout;
        nowrong = true;
    } // ///////////////////////////////////////////////////////////////
    public new void SetRes(bool sucess) {
        nowrong &= sucess;
    } // ///////////////////////////////////////////////////////////////
    public void SaveRes() {
        base.SetRes(nowrong);
        interval.setResult(nowrong);
    } // ///////////////////////////////////////////////////////////////
    public int overdue(DateTime dt) { return interval.HouresExpired(dt); }
    public bool EQ(Exercise other) { return layout.EQ(other.layout); }
    public new string info(string s0) {
        string s = "Exercise dkue:" + base.info + layout.info + interval.info;
        UnityEngine.Debug.Log(s0 + s);
        return s;
    }
} // ************************************************************************************
