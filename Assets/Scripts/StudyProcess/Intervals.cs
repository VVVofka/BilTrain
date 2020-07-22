using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class Intervals {
    static int[] vhours = {1, 3, 24, 3 * 24, 7 * 24, 28 * 24, 95 * 24 };
    int index = -1;
    DateTime lastDT = DateTime.Now;

    public bool isComplete { get => index >= vhours.Length; }
    public void setResult(bool val) {
        if(val) {
            index++;
            lastDT = DateTime.Now;
        } else {
            if(--index < 0)
                index = 0;
        }
    } // ///////////////////////////////////////////////////////////////////////
    //public void setSuc() { setResult(true); }
    //public void setErr() { setResult(false); }

    public int difH(DateTime dt) { // if > 0 - expired
        DateTime expect = lastDT.AddHours(vhours[index]);
        TimeSpan d = dt.Subtract(expect);
        return d.Hours;
    } // ///////////////////////////////////////////////////////////////////////
    public bool EQ(Intervals other) {
        return index == other.index && lastDT == other.lastDT;
    } // ///////////////////////////////////////////////////////////////////////
} // ****************************************************************************
