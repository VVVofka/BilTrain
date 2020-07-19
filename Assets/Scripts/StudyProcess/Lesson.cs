using System;
using System.Collections.Generic;

public class Lesson : DKCue {
    List<ExerciseEnh> v = new List<ExerciseEnh>();
    public int ExercisesInLesson = 10;
    public int[] vluzes = {0, 2};
    public int[] vsigns = {-1, 1};
    public Layout curLayout { get => v.Count > 0 ? v[nlayout].layout : null; }
    public Exercise curExercise { get => v.Count > 0 ? v[nlayout] : null; }
    int nlayout;

    public Layout moveNext() {
        if(++nlayout >= v.Count) {
            nlayout = 0;
            return null;
        }
        return curLayout;
    } // /////////////////////////////////////////////////////////////////////////////////////
    public Layout moveFirst() {
        nlayout = 0;
        if(v.Count == 0)
            return null;
        dkcue = 1.0f;
        return curLayout;
    } // /////////////////////////////////////////////////////////////////////////////////////

    public int LoadRipe(List<Exercise> vripe) {
        v.Clear();
        DateTime now = DateTime.Now;
        int j = 0;
        foreach(Exercise p in vripe) {
            if(p.overdue(now) <= 0)
                break;
            if(++j >= ExercisesInLesson)
                break;
            Layout lay = p.layout;
            float distAim = lay.distAim;
            float distCue = lay.distCue;
            foreach(int luz in vluzes)
                foreach(int signAng in vsigns) {
                    float angle = lay.angAimDeg * signAng;
                    foreach(int signK in vsigns) {
                        float kCue = lay.kCue * signK;
                        Layout layout = new Layout(distAim, distCue, angle, kCue);
                        Exercise psign = new Exercise(p.topic, layout);
                        v.Add(new ExerciseEnh(psign, luz));
                    }
                }
        }
        return ExercisesInLesson - j;
    } // ////////////////////////////////////////////////////////////////
    public void LoadNew(Topic topic) {
        int jmax = ExercisesInLesson - v.Count;
        for(int j = 0; j < jmax; j++) {
            Layout lay = new Layout(topic.from, topic.to);
            float distAim = lay.distAim;
            float distCue = lay.distCue;
            foreach(int luz in vluzes)
                foreach(int signAng in vsigns) {
                    float angle = lay.angAimDeg * signAng;
                    foreach(int signK in vsigns) {
                        float kCue = lay.kCue * signK;
                        Layout layout = new Layout(distAim, distCue, angle, kCue);
                        Exercise psign = new Exercise(topic, layout);
                        v.Add(new ExerciseEnh(psign, luz));
                    }
                }
        }
        Shuffle();
        dkcue = 1.0f;
    } // /////////////////////////////////////////////////////////////////////
    void Shuffle() {
        Random rand = new Random();
        for(int i = v.Count - 1; i >= 1; i--) {
            int j = rand.Next(i + 1);
            ExerciseEnh tmp = v[j];
            v[j] = v[i];
            v[i] = tmp;
        }
    } // ////////////////////////////////////////////////////////////////
    public new void SetRes(bool sucess) {
        base.SetRes(sucess);
        curExercise.SetRes(sucess);
    } // ///////////////////////////////////////////////////////////////////////////////////////

} // *************************************************************
