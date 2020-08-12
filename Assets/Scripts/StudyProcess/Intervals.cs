using System;

[Serializable]
public class Intervals {
    static int[] vhours = {1, 3, 24, 3 * 24, 7 * 24, 28 * 24, 95 * 24 };
    int index = -1;
    DateTime lastDT = DateTime.Now;

    public void setResult(bool val) {
        if(val) {
            index++;
            lastDT = DateTime.Now;
        } else {
            if(--index < 0)
                index = 0;
        }
    } // ///////////////////////////////////////////////////////////////////////

    public int HouresExpired(DateTime dt) { // if > 0 - expired
        if(index < 0)
            return int.MaxValue;
        DateTime expect = lastDT.AddHours(vhours[index]);
        TimeSpan d = dt.Subtract(expect);
        return d.Hours;
    } // ///////////////////////////////////////////////////////////////////////
    //public bool EQ(Intervals other) {
    //    return index == other.index && lastDT == other.lastDT;
    //} // ///////////////////////////////////////////////////////////////////////
    public string info { get {
            string s = " Interval: idx=" + index.ToString();
            if (index >= 0) s += " DT:" + lastDT.ToString() + " expired:" + HouresExpired(DateTime.Now);
            return s;
        }
    }
} // ****************************************************************************
