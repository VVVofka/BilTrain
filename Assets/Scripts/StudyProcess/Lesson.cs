using System;
using System.Collections.Generic;

[Serializable]
public class Lesson : DKCue {
    List<ExerciseEnh> v = new List<ExerciseEnh>();
    public List<Exercise> vstuded = new List<Exercise>();

    public static int ExercisesInLesson = 10;
    public int[] vluzes = {0, 2};
    public int[] vsigns = {1, -1};
    List<Exercise> vripe = new List<Exercise>();

    public Layout layout { get => v.Count > 0 ? v[0].layout : null; }
    public ExerciseEnh curExercise { get => v.Count > 0 ? v[0] : null; }

    public int LoadRipe(List<Exercise> vRipe) {
        try {
            vripe = vRipe;
            v.Clear();
            DateTime now = DateTime.Now;
            int j = 0;
            foreach(Exercise exercise in vripe) {
                if(exercise.overdue(now) <= 0)
                    break;
                if(++j >= ExercisesInLesson)
                    break;
                Layout lay = exercise.layout;
                foreach(int luz in vluzes)
                    foreach(int signAng in vsigns)
                        foreach(int signK in vsigns) {
                            if(lay.SetBandRnd(signAng, signK))
                                v.Add(new ExerciseEnh(new Exercise(lay), luz, signK));
                        }
            }
            return ExercisesInLesson - j;
        } catch(Exception ex) {
            Console.WriteLine($"Исключение in LoadRipe(): {ex.Message}");
            Console.WriteLine($"Метод: {ex.TargetSite}");
            Console.WriteLine($"Трассировка стека: {ex.StackTrace}");
            return -1;
        }
    } // ////////////////////////////////////////////////////////////////
    public int LoadNew(Topic topic) {
        try {
            int jmax = ExercisesInLesson - v.Count;
            for(int j = 0; j < jmax; j++) {
                Layout lay = new Layout(topic.from, topic.to);
                foreach(int luz in vluzes)
                    foreach(int signAng in vsigns) {
                        foreach(int signK in vsigns) {
                            if(lay.SetBandRnd(signAng, signK))
                                v.Add(new ExerciseEnh(new Exercise(lay), luz, signK));
                        }
                    }
            }
            Shuffle();
            dkcue = 1.0f;
            return v.Count;
        } catch(Exception ex) {
            Console.WriteLine($"Исключение in LoadNew({topic.name}): {ex.Message}");
            Console.WriteLine($"Метод: {ex.TargetSite}");
            Console.WriteLine($"Трассировка стека: {ex.StackTrace}");
            return -1;
        }
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
        try {
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
