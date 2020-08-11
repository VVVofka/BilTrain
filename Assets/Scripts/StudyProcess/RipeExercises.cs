using System;
using System.Collections.Generic;

[Serializable]
public class RipeExercises {
    List<Exercise> vripe = new List<Exercise>();
    public List<Exercise> vriped = new List<Exercise>();

    public void Add(Exercise exercise) {
        Exercise aaa = vripe.Find(x => x.EQ(exercise));
        if(aaa == null)
            vripe.Add(exercise);
        else
            aaa = exercise;
    } // ////////////////////////////////////////////////////////////////////////////////////
    public List<Exercise> getRiped(int outCount) {
        DateTime dt = DateTime.Now;
        List<Exercise> resfind = vripe.FindAll(delegate (Exercise x) {
            return x.overdue(dt) > 0;
        });
        resfind.Sort(delegate (Exercise x, Exercise y) {
            return x.overdue(dt) > y.overdue(dt) ? -1 : 0;
        });
        if(resfind.Count > outCount)
            vriped = resfind.GetRange(0, outCount);
        else
            vriped = vripe;
        return vriped;
    } // ///////////////////////////////////////////////////////////////////////////////////////
    public void setResult(Exercise q, bool val) {
        Exercise aaa = vripe.Find(x => x.EQ(q));
        Exercise bbb = vriped.Find(x => x.EQ(q));
        q.SetRes(val);
        if(aaa == null)
            vripe.Add(q);
        else
            aaa = q;
        vriped.Remove(bbb);
    } // //////////////////////////////////////////////////////////////////////////////////////
    public void info(string s0) {
        UnityEngine.Debug.Log(s0 + "Start RipeExercises:");
        UnityEngine.Debug.Log(s0 + " vripe(count=" + vripe.Count + ")");
        foreach(var q in vripe)
            q.info(s0 + "  ");
        UnityEngine.Debug.Log(s0 + " vriped(count=" + vriped.Count + ")");
        foreach(var q in vriped)
            q.info(s0 + "  ");
        UnityEngine.Debug.Log(s0 + "End RipeExercises.");
    } // /////////////////////////////////////////////////////////////////////////////////////
    //void Sortv() {
    //    vripe.Sort(delegate (Exercise x, Exercise y) {
    //        DateTime dt = DateTime.Now;
    //        if(x.overdue(dt) > y.overdue(dt))
    //            return -1;
    //        return 0;
    //    });
    //} // ////////////////////////////////////////////////////////////////
    //void Sortvfind() {
    //    DateTime dt = DateTime.Now;
    //    List<Exercise> resfind = vripe.FindAll(delegate (Exercise x) {
    //        return x.overdue(dt) > 0;
    //    });
    //    resfind.Sort(delegate (Exercise x, Exercise y) {
    //        return x.overdue(dt) > y.overdue(dt) ? -1 : 0;
    //    });
    //} // ////////////////////////////////////////////////////////////////

} // *************************************************************************************
