using System;
using System.Collections.Generic;

[Serializable]
public class RipeExercises : DKCue {
    List<Exercise> vripe = new List<Exercise>();
    int[] vluzes = {2};
    int[] vsigns =  {1, -1};

    public void Add(Exercise exercise) {
        Exercise aaa = vripe.Find(x => x.EQ(exercise));
        if(aaa == null)
            vripe.Add(exercise);
        else
            aaa = exercise;
    } // ////////////////////////////////////////////////////////////////////////////////////
    public void Extract(int outCount, List<ExerciseEnh> list) {
        list.Clear();
        Sortv();
        for(int j = 0; j < outCount; j++) {
            if(vripe[j].overdue(DateTime.Now) <= 0)
                break;
            foreach(int luz in vluzes)
                foreach(int sign in vsigns)
                    list.Add(new ExerciseEnh(vripe[j], sign, luz));
        }
        Shuffle(list);
    } // ///////////////////////////////////////////////////////////////////////////////////////
    void Sortv() {
        vripe.Sort(delegate (Exercise x, Exercise y) {
            DateTime dt = DateTime.Now;
            if(x.overdue(dt) > y.overdue(dt))
                return -1;
            return 0;
        });
    } // ////////////////////////////////////////////////////////////////
    static void Shuffle(List<ExerciseEnh> arr) {
        System.Random rand = new System.Random();
        for(int i = arr.Count - 1; i >= 1; i--) {
            int j = rand.Next(i + 1);
            ExerciseEnh tmp = arr[j];
            arr[j] = arr[i];
            arr[i] = tmp;
        }
    } // ////////////////////////////////////////////////////////////////
} // *************************************************************************************
