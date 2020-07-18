using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class Intervals {
    static int[] vhours = {1, 3, 24, 3 * 24, 7 * 24, 28 * 24, 95 * 24 };
    int index = 0;
    DateTime lastDT = DateTime.Now;

    public void setVal(bool val) {
        if(val)
            index++;
        else
            index--;
        lastDT = DateTime.Now;
    } // ///////////////////////////////////////////////////////////////////////
    public void setSuc() { setVal(true); }
    public void setErr() { setVal(false); }

    public int difH(DateTime dt) { // if > 0 - expired
        DateTime expect = lastDT.AddHours(vhours[index]);
        TimeSpan d = dt.Subtract(expect);
        return d.Hours;
    } // ///////////////////////////////////////////////////////////////////////
    public bool EQ(Intervals other) {
        return index == other.index && lastDT == other.lastDT;
    } // ///////////////////////////////////////////////////////////////////////
} // ****************************************************************************
