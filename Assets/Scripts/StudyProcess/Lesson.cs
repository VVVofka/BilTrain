using System;
using System.Collections;
using System.Collections.Generic;

public class Lesson : DKCue {
    List<ExerciseEnh> v = new List<ExerciseEnh>();
    public int ExercisesInLesson = 10;
    public int[] vluzes = {2};
    public int[] vsigns = {-1, 1};

    public int LoadRipe(List<Exercise> vripe) {
        v.Clear();
        DateTime now = DateTime.Now;
        foreach(Exercise p in vripe) {
            if(p.overdue(now) <= 0)
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
        return ExercisesInLesson - v.Count;
    } // ////////////////////////////////////////////////////////////////
    public void LoadNew(Topic topic) {
        for(int j = 0; j < ExercisesInLesson - v.Count; j++) {
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
    } // /////////////////////////////////////////////////////////////////////
    void Shuffle() {
        System.Random rand = new System.Random();
        for(int i = v.Count - 1; i >= 1; i--) {
            int j = rand.Next(i + 1);
            ExerciseEnh tmp = v[j];
            v[j] = v[i];
            v[i] = tmp;
        }
    } // ////////////////////////////////////////////////////////////////
} // *************************************************************
