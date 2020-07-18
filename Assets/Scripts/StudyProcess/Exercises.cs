using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Exercises {
    List<Exercise> v;
    int[] vluzes = {2};
    int[] vsigns =  {1, -1};
    float dkcue = 1.0f;
    float stepk = 0.01f;

    public float setResult(bool sucess) {
        if(sucess)
            dkcue = dkcue * (1 - stepk);
        else
            dkcue = dkcue * (1 + stepk);
        return dkcue;
    } // /////////////////////////////////////////////////////////////////////////////////

    public void Create(int outCount, List<ExerciseEnh> list) {
        list.Clear();
        Sortv();
        for(int j=0; j<outCount; j++) {
            if(v[j].overdue(DateTime.Now) <= 0)
                break;
            foreach(int luz in vluzes)
                foreach(int sign in vsigns) 
                    list.Add(new ExerciseEnh(v[j], sign, luz));
        }
        Shuffle(list);
    } // ///////////////////////////////////////////////////////////////////////////////////////
    void Sortv() {
        v.Sort(delegate (Exercise x, Exercise y) {
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
