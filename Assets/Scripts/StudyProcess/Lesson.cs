﻿using System;
using System.Collections.Generic;

[Serializable]
public class Lesson : DKCue {
    List<ExerciseEnh> v = new List<ExerciseEnh>();
    public List<Exercise> vstuded = new List<Exercise>();

    public int ExercisesInLesson = 10;
    public int[] vluzes = {0, 2};
    public int[] vsigns = {-1, 1};
    int index;
    List<Exercise> vripe = new List<Exercise>();   // ptr

    public Layout curLayout { get => v.Count > 0 ? v[index].layout : null; }
    public ExerciseEnh curExercise { get => v.Count > 0 ? v[index] : null; }
    public bool isEndOfLesson { get => index >= v.Count; }

    public Layout moveNext() {
        if(++index >= v.Count) {
            index = 0;
            return null;
        }
        return curLayout;
    } // /////////////////////////////////////////////////////////////////////////////////////
    public Layout moveFirst() {
        index = 0;
        vstuded.Clear();
        if(v.Count == 0)
            return null;
        dkcue = 1.0f;
        return curLayout;
    } // /////////////////////////////////////////////////////////////////////////////////////

    public int LoadRipe(List<Exercise> vRipe) {
        vripe = vRipe;
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
                        Exercise psign = new Exercise(layout);
                        v.Add(new ExerciseEnh(psign, luz, signK));
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
                        Exercise psign = new Exercise(layout);
                        v.Add(new ExerciseEnh(psign, luz, signK));
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
        if(sucess) {
            vstuded.Add(curExercise);
            v.Remove(curExercise);
            if(v.Count <= 0)
                OnEndOfLesson?.Invoke();
        } else {

        }
        Shuffle();
        On_Choose?.Invoke(sucess);
    } // ///////////////////////////////////////////////////////////////////////////////////////

    public delegate void StateHandlerOnChoose(bool isSucess);   // Объявляем делегат
    public event StateHandlerOnChoose On_Choose;                 // Создаем переменную делегата
    public delegate void StateHandlerOnEndOfLesson();           // Объявляем делегат
    public event StateHandlerOnEndOfLesson OnEndOfLesson;       // Создаем переменную делегата
    //public void RegisterHandler(LessonStateHandler deleg){_deleg += deleg;}  // Регистрируем делегат
} // *************************************************************