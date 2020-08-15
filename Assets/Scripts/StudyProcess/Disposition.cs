using System;
[Serializable]
public class Disposition {
    public Layout5 layout5;
    public int nluze;
    public TimeIntervals tintervals;

    public Disposition() {
        layout5 = new Layout5();
        nluze = 2;
        tintervals = new TimeIntervals();
    } // //////////////////////////////////////////////////////////////////////////

} // ********************************************************************************
