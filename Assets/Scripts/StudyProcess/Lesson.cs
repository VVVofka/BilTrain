using System;
using System.Collections.Generic;

[Serializable]
public class Lesson : DKCue {
    List<ExerciseEnh> v = new List<ExerciseEnh>();
    public List<Exercise> vstuded = new List<Exercise>();

    public static int ExercisesInLesson = 10;
    public int[] vluzes = {0, 2};
    public int[] vsigns = {-1, 1};
    List<Exercise> vripe = new List<Exercise>();   

    public Layout layout { get => v.Count > 0 ? v[0].layout : null; }
    public ExerciseEnh curExercise { get => v.Count > 0 ? v[0] : null; }

    public int LoadRipe(List<Exercise> vRipe) {
        try {
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
            float distAim = lay.distAimInD;
            float distCue = lay.distCueInD;
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
        } catch(Exception ex) {
            Console.WriteLine($"Исключение in LoadRipe(): {ex.Message}");
            Console.WriteLine($"Метод: {ex.TargetSite}");
            Console.WriteLine($"Трассировка стека: {ex.StackTrace}");
            return -1;
        }
    } // ////////////////////////////////////////////////////////////////
    public void LoadNew(Topic topic) {
        try {
        int jmax = ExercisesInLesson - v.Count;
        for(int j = 0; j < jmax; j++) {
            Layout lay = new Layout(topic.from, topic.to);
            float distAim = lay.distAimInD;
            float distCue = lay.distCueInD;
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
        } catch(Exception ex) {
            Console.WriteLine($"Исключение in LoadNew({topic.name}): {ex.Message}");
            Console.WriteLine($"Метод: {ex.TargetSite}");
            Console.WriteLine($"Трассировка стека: {ex.StackTrace}");
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
