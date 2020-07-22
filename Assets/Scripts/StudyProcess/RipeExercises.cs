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
        vriped = resfind.GetRange(0, outCount);
        return vriped;
    } // ///////////////////////////////////////////////////////////////////////////////////////

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
