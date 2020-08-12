using System;
using System.Collections.Generic;

[Serializable]
public class Lesson : DKCue {
    List<ExerciseEnh> v = new List<ExerciseEnh>();
    public ExerciseEnh curExercise;
    public int cntTotal;

    public int ExercisesInLesson = 2;   // TODO: ExercisesInLesson = 10
    public int[] vluzes = {2};          // TODO: Luzes
    public int[] vsigns = {1, -1};

    public Layout layout {
        get => v.Count > 0 ? curExercise.layout : null;
    }
    //public ExerciseEnh curExercise { get => v.Count > 0 ? v[0] : null; }

    public int cntUnStuded { get => v.Count; }

    public int LoadRipe(List<Exercise> vripe) {
        v.Clear();
        curExercise = null;
        DateTime now = DateTime.Now;
        foreach(Exercise exercise in vripe) {
            if(exercise.overdue(now) <= 0)
                break;
            Layout lay = exercise.layout;
            foreach(int luz in vluzes)
                foreach(int signAng in vsigns)
                    foreach(int signK in vsigns) {
                        if(lay.SetBandRnd(signAng, signK)) {
                            curExercise = new ExerciseEnh(new Exercise(lay), luz);
                            v.Add(curExercise);
                        }
                    }
        }
        Shuffle();
        dkcue = 1.0f;
        return cntTotal = v.Count;
    } // ////////////////////////////////////////////////////////////////
      //public int LoadRipe(List<Exercise> vripe) {
      //     v.Clear();
      //     curExercise = null;
      //     DateTime now = DateTime.Now;
      //     int j = 0;
      //     foreach(Exercise exercise in vripe) {
      //         if(exercise.overdue(now) <= 0)
      //             break;
      //         Layout lay = exercise.layout;
      //         foreach(int luz in vluzes)
      //             foreach(int signAng in vsigns)
      //                 foreach(int signK in vsigns) {
      //                     if(lay.SetBandRnd(signAng, signK)) {
      //                         curExercise = new ExerciseEnh(new Exercise(lay), luz);
      //                         v.Add(curExercise);
      //                         if(++j >= ExercisesInLesson)
      //                             return 0;
      //                     }
      //                 }
      //     }
      //     cntTotal = v.Count;
      //     return ExercisesInLesson - j;
      // } // ////////////////////////////////////////////////////////////////    
    //public int LoadNew(Topics topics) {
    //    topics.resetNew();
    //    fillv(topics);
    //    Shuffle();
    //    dkcue = 1.0f;
    //    return v.Count;
    //} // /////////////////////////////////////////////////////////////////////
    //void fillv(Topics topics) {
    //    int jmax = ExercisesInLesson - v.Count;
    //    for(int j = 0; ;) {
    //        foreach(int luz in vluzes)
    //            foreach(int signAng in vsigns)
    //                foreach(int signK in vsigns) {
    //                    Topic topic = topics.curTopic;
    //                    if(topic.cntInStudyCur + topic.cntInStudyNew >= topic.cntInStudyMax) {
    //                        //if(topic.cntInStudyNew == 0)
    //                        //    topic.cntInStudyCur--;
    //                        //else
    //                        //    topic.cntInStudyNew--;
    //                        topic = topics.ToNextTopic();
    //                    }
    //                    Layout lay = new Layout(topic.from, topic.to);
    //                    if(lay.SetBandRnd(signAng, signK)) {
    //                        if(j++ >= jmax)
    //                            return;
    //                        curExercise = new ExerciseEnh(new Exercise(lay), luz);
    //                        v.Add(curExercise);
    //                        topic.cntInStudyNew++;
    //                    }
    //                }
    //    }
    //} // /////////////////////////////////////////////////////////////////////
    void Shuffle() {
        for(int i = v.Count - 1; i >= 1; i--) {
            int j = Field.rand.Next(i + 1);
            ExerciseEnh tmp = v[j];
            v[j] = v[i];
            v[i] = tmp;
        }
        curExercise = v[0];
        cntTotal = v.Count;
    } // ////////////////////////////////////////////////////////////////
    public new bool SetRes(bool sucess) {
        base.SetRes(sucess);
        if(curExercise != null) {
            curExercise.SetRes(sucess);
            if(sucess) 
                v.Remove(curExercise);
            if(v.Count > 0) {
            //    Shuffle();
                return true;
            }
        }
        return false;
    } // ///////////////////////////////////////////////////////////////////////////////////////
    public new void info(string s0) {
        UnityEngine.Debug.Log(s0 + "Lesson v:");
        foreach(var q in v) 
            q.info(s0 + " ");
        UnityEngine.Debug.Log(s0 + "End lesson");
    } // //////////////////////////////////////////////////////////////////////////////////////
} // *************************************************************
