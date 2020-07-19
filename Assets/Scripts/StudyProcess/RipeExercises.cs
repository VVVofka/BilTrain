using System;
using System.Collections.Generic;

[Serializable]
public class RipeExercises : DKCue {
    List<Exercise> vripe = new List<Exercise>();

    public void Add(Exercise exercise) {
        Exercise aaa = vripe.Find(x => x.EQ(exercise));
        if(aaa == null)
            vripe.Add(exercise);
        else
            aaa = exercise;
    } // ////////////////////////////////////////////////////////////////////////////////////
    public List<Exercise> getRiped(int outCount) {
        List<Exercise> ret = new List<Exercise>();
        Sortv();
        int cnt = Math.Min(outCount, vripe.Count);
        for(int j = 0; j < cnt; j++) {
            if(vripe[j].overdue(DateTime.Now) <= 0)
                break;
            ret.Add(vripe[j]);
        }
        return ret;
    } // ///////////////////////////////////////////////////////////////////////////////////////
    void Sortv() {
        vripe.Sort(delegate (Exercise x, Exercise y) {
            DateTime dt = DateTime.Now;
            if(x.overdue(dt) > y.overdue(dt))
                return -1;
            return 0;
        });
    } // ////////////////////////////////////////////////////////////////

} // *************************************************************************************
