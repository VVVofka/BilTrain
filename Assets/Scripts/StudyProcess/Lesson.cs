using System;
using System.Collections.Generic;

[Serializable]
public class Lesson : DKCue {
    List<ExerciseEnh> v = new List<ExerciseEnh>();
    public List<Exercise> vstuded = new List<Exercise>();

    public int ExercisesInLesson = 3;
    public int[] vluzes = {2}; // TODO: Luzes
    public int[] vsigns = {1, -1};
    List<Exercise> vripe = new List<Exercise>();

    public Layout layout { get => v.Count > 0 ? v[0].layout : null; }
    public ExerciseEnh curExercise { get => v.Count > 0 ? v[0] : null; }

    public int cntStuded { get => vstuded.Count; }
    public int cntUnStuded { get => v.Count; }

    public int LoadRipe(List<Exercise> vRipe) {
        vripe = vRipe;
        v.Clear();
        DateTime now = DateTime.Now;
        int j = 0;
        foreach(Exercise exercise in vripe) {
            if(exercise.overdue(now) <= 0)
                break;
            Layout lay = exercise.layout;
            foreach(int luz in vluzes)
                foreach(int signAng in vsigns)
                    foreach(int signK in vsigns) {
                        if(lay.SetBandRnd(signAng, signK)) {
                            v.Add(new ExerciseEnh(new Exercise(lay), luz));
                            if(++j >= ExercisesInLesson)
                                return 0;
                        }
                    }
        }
        return ExercisesInLesson - j;
    } // ////////////////////////////////////////////////////////////////
    public int LoadNew(Topics topics) {
        topics.resetNew();
        fillv(topics);
        Shuffle();
        dkcue = 1.0f;
        return v.Count;
    } // /////////////////////////////////////////////////////////////////////
    void fillv(Topics topics) {
        int jmax = ExercisesInLesson - v.Count;
        for(int j = 0; ;) {
            foreach(int luz in vluzes)
                foreach(int signAng in vsigns)
                    foreach(int signK in vsigns) {
                        Topic topic = topics.curTopic;
                        if(topic.cntInStudyCur + topic.cntInStudyNew >= topic.cntInStudyMax) {
                            //if(topic.cntInStudyNew == 0)
                            //    topic.cntInStudyCur--;
                            //else
                            //    topic.cntInStudyNew--;
                            topic = topics.ToNextTopic();
                        }
                        Layout lay = new Layout(topic.from, topic.to);
                        if(lay.SetBandRnd(signAng, signK)) {
                            if(j++ >= jmax)
                                return;
                            v.Add(new ExerciseEnh(new Exercise(lay), luz));
                            topic.cntInStudyNew++;
                        }
                    }
        }
    } // /////////////////////////////////////////////////////////////////////
    void Shuffle() {
        for(int i = v.Count - 1; i >= 1; i--) {
            int j = Field.rand.Next(i + 1);
            ExerciseEnh tmp = v[j];
            v[j] = v[i];
            v[i] = tmp;
        }
    } // ////////////////////////////////////////////////////////////////
    public new void SetRes(bool sucess) {
        try {
            base.SetRes(sucess);
            curExercise.SetRes(sucess);
            if(sucess) {
                vstuded.Add(curExercise);
                v.Remove(curExercise);
                if(v.Count <= 0) {
                    OnEndOfLesson?.Invoke();
                }
            }
            Shuffle();
            On_Choose?.Invoke(sucess);
        } catch(Exception ex) {
            Console.WriteLine($"Исключение in SetRes({sucess}): {ex.Message}");
            Console.WriteLine($"Метод: {ex.TargetSite}");
            Console.WriteLine($"Трассировка стека: {ex.StackTrace}");
        }
    } // ///////////////////////////////////////////////////////////////////////////////////////

    public delegate void StateHandlerOnChoose(bool isSucess);   // Объявляем делегат
    public event StateHandlerOnChoose On_Choose;                 // Создаем переменную делегата
    public delegate void StateHandlerOnEndOfLesson();           // Объявляем делегат
    public event StateHandlerOnEndOfLesson OnEndOfLesson;       // Создаем переменную делегата
    //public void RegisterHandler(LessonStateHandler deleg){_deleg += deleg;}  // Регистрируем делегат
} // *************************************************************
