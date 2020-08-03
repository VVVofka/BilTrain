using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public enum TrueAim {
    left = -1,
    center = 0,
    right = 1,
    none = -2
} // ***********************************************************************************

public class StudyProcess {
    const string ver = "ver:(0.1.42)";
    const string TopicsFileDefault = "Develop.tpcs";
    const string RipeExercisesFileDefault = "Develop.rpex";
    const string LessonFileDefault = "Develop.lesn";

    Topics topics;
    RipeExercises ripeExercises;
    Lesson lesson;
    public TrueAim curAim = TrueAim.none;

    static Random rand = new Random();

    public float dkcue {
        get {
            curAim = (TrueAim)rand.Next(-1, 1);
            return (topics.dkcue + topics.topic.dkcue + lesson.dkcue + lesson.curExercise.dkcue) / 4;
        }
    }

    public StudyProcess() {
        LoadTopicFile(TopicsFileDefault);
        LoadRipeExercisesFile(RipeExercisesFileDefault);
        //LoadLessonFile(LessonFileDefault);
        lesson = new Lesson();
        LoadLesson();

        lesson.On_Choose += OnChoose;
        lesson.OnEndOfLesson += OnEndLesson;
        //lesson.RegisterHandler(new Lesson.LessonStateHandler(DelStuded));
    } // //////////////////////////////////////////////////////////////////
    public Layout layout { get => lesson.layout; } // ///////////////////
    public Layout moveNext() {
        Layout lay = lesson.layout;
        if(lay != null)
            return lay;
        LoadLesson();
        return lesson.layout;
    } // //////////////////////////////////////////////////////////////////////
    //void LoadLessonFile(string fname) {
    //    BinaryFormatter formatter = new BinaryFormatter();
    //    bool fileOk;
    //    if(File.Exists(fname)) {
    //        using(FileStream fs = new FileStream(fname, FileMode.Open)) {
    //            string readver = (string)formatter.Deserialize(fs);
    //            fileOk = (readver == ver);
    //            if(fileOk)
    //                lesson = (Lesson)formatter.Deserialize(fs);
    //        }
    //        if(!fileOk) {
    //            File.Delete(fname);
    //            CreateLessonFile(fname);
    //        }
    //    } else
    //        CreateLessonFile(fname);
    //} // //////////////////////////////////////////////////////////////////
    public Layout LoadLesson() {
        try {
            List<Exercise> vripe = ripeExercises.getRiped(Lesson.ExercisesInLesson);
            int rest = lesson.LoadRipe(vripe);
            if(rest > 0) {
                Topic topic = topics.topic;
                if(topic.cntCur >= topic.cntMax)
                    topic = topics.ToNextTopic();
                lesson.LoadNew(topic);
            }
            return lesson.layout;
        } catch(Exception ex) {
            Console.WriteLine($"Исключение in public Layout LoadLesson(): {ex.Message}");
            Console.WriteLine($"Метод: {ex.TargetSite}");
            Console.WriteLine($"Трассировка стека: {ex.StackTrace}");
            //throw;
            return null;
        }
    } // ///////////////////////////////////////////////////////////////////
    void LoadTopicFile(string fname) {
        try {
            BinaryFormatter formatter = new BinaryFormatter();
            bool fileOk;
            if(File.Exists(fname)) {
                using(FileStream fs = new FileStream(fname, FileMode.Open)) {
                    string readver = (string)formatter.Deserialize(fs);
                    fileOk = (readver == ver);
                    if(fileOk)
                        topics = (Topics)formatter.Deserialize(fs);
                }
                if(!fileOk) {
                    File.Delete(fname);
                    CreateTopicFile(fname);
                }
            } else
                CreateTopicFile(fname);
        } catch(Exception ex) {
            Console.WriteLine($"Исключение in {fname}: {ex.Message}");
            Console.WriteLine($"Метод: {ex.TargetSite}");
            Console.WriteLine($"Трассировка стека: {ex.StackTrace}");
            //throw;
        }
    } // //////////////////////////////////////////////////////////////////
    void LoadRipeExercisesFile(string fname) {
        bool fileOk;
        try {
            BinaryFormatter formatter = new BinaryFormatter();
            if(File.Exists(fname)) {
                using(FileStream fs = new FileStream(fname, FileMode.Open)) {
                    string readver = (string)formatter.Deserialize(fs);
                    fileOk = (readver == ver);
                    if(fileOk) {
                        ripeExercises = (RipeExercises)formatter.Deserialize(fs);
                    }
                }
                if(!fileOk) {
                    File.Delete(fname);
                    CreateRipeExercisesFile(fname);
                }
            } else
                CreateRipeExercisesFile(fname);
        } catch(Exception ex) {
            Console.WriteLine($"Исключение in {fname}: {ex.Message}");
            Console.WriteLine($"Метод: {ex.TargetSite}");
            Console.WriteLine($"Трассировка стека: {ex.StackTrace}");
            //throw;
        }
    } // ///////////////////////////////////////////////////////////////////////////
    void CreateTopicFile(string fname) {
        try {
            topics = new Topics();
            BinaryFormatter formatter = new BinaryFormatter();
            using(FileStream fs = new FileStream(fname, FileMode.Create)) {
                formatter.Serialize(fs, ver);
                formatter.Serialize(fs, topics);
            }
        } catch(Exception ex) {
            Console.WriteLine($"Исключение in {fname}: {ex.Message}");
            Console.WriteLine($"Метод: {ex.TargetSite}");
            Console.WriteLine($"Трассировка стека: {ex.StackTrace}");
            //throw;
        }
    } // ////////////////////////////////////////////////////////////////////////
    //void CreateLessonFile(string fname) {
    //    lesson = new Lesson();
    //    BinaryFormatter formatter = new BinaryFormatter();
    //    using(FileStream fs = new FileStream(fname, FileMode.Create)) {
    //        formatter.Serialize(fs, ver);
    //        formatter.Serialize(fs, lesson);
    //    }
    //} // ////////////////////////////////////////////////////////////////////////
    void CreateRipeExercisesFile(string fname) {
        try {
            ripeExercises = new RipeExercises();
            BinaryFormatter formatter = new BinaryFormatter();
            using(FileStream fs = new FileStream(fname, FileMode.Create)) {
                formatter.Serialize(fs, ver);
                formatter.Serialize(fs, ripeExercises);
            }
        } catch(Exception ex) {
            Console.WriteLine($"Исключение in {fname}: {ex.Message}");
            Console.WriteLine($"Метод: {ex.TargetSite}");
            Console.WriteLine($"Трассировка стека: {ex.StackTrace}");
        }
    } // ////////////////////////////////////////////////////////////////////////
    public void Close() {
        try {
            CreateTopicFile(TopicsFileDefault);
            CreateRipeExercisesFile(RipeExercisesFileDefault);
            //CreateLessonFile(LessonFileDefault);
        } catch(Exception ex) {
            Console.WriteLine($"Исключение in Close(): {ex.Message}");
            Console.WriteLine($"Метод: {ex.TargetSite}");
            Console.WriteLine($"Трассировка стека: {ex.StackTrace}");
        }
    } // ////////////////////////////////////////////////////////////////////////
    public bool SetRes(TrueAim aim) {
        try {
            bool sucess = (aim == curAim);
            topics.SetRes(sucess);
            lesson.SetRes(sucess);
            return sucess;
        } catch(Exception ex) {
            Console.WriteLine($"Исключение in SetRes({aim}): {ex.Message}");
            Console.WriteLine($"Метод: {ex.TargetSite}");
            Console.WriteLine($"Трассировка стека: {ex.StackTrace}");
            return false;
        }
    } // ///////////////////////////////////////////////////////////////////////
    void OnChoose(bool isSucess) {
        try {
            foreach(var x in lesson.vstuded) {
            }
            on_aim?.Invoke(curAim);
        } catch(Exception ex) {
            Console.WriteLine($"Исключение in SetRes({isSucess}): {ex.Message}");
            Console.WriteLine($"Метод: {ex.TargetSite}");
            Console.WriteLine($"Трассировка стека: {ex.StackTrace}");
        }
    } // ///////////////////////////////////////////////////////////////////////
    void OnEndLesson() {
    } // /////////////////////////////////////////////////////////////////////////
    public delegate void StateHandlerAim(TrueAim realAim);   // Объявляем делегат
    public event StateHandlerAim on_aim;                 // Создаем переменную делегата
} // *******************************************************************************************
