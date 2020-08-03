using System;
using System.Collections.Generic;

[Serializable]
public class RipeExercises {
    List<Exercise> vripe = new List<Exercise>();
    public List<Exercise> vriped = new List<Exercise>();

    public void Add(Exercise exercise) {
        try {
            Exercise aaa = vripe.Find(x => x.EQ(exercise));
            if(aaa == null)
                vripe.Add(exercise);
            else
                aaa = exercise;
        } catch(Exception ex) {
            Console.WriteLine($"Исключение in RipeExercises.Add(): {ex.Message}");
            Console.WriteLine($"Метод: {ex.TargetSite}");
            Console.WriteLine($"Трассировка стека: {ex.StackTrace}");
        }
    } // ////////////////////////////////////////////////////////////////////////////////////
    public List<Exercise> getRiped(int outCount) {
        try {
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
        } catch(Exception ex) {
            Console.WriteLine($"Исключение in RipeExercises.GetRiped({outCount}): {ex.Message}");
            Console.WriteLine($"Метод: {ex.TargetSite}");
            Console.WriteLine($"Трассировка стека: {ex.StackTrace}");
            return null;
        }
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
