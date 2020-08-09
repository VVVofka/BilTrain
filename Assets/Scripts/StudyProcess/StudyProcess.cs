using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class StudyProcess {
    const string ver = "ver:(0.1.88)";
    const string TopicsFileDefault = "Develop.tpcs";
    const string RipeExercisesFileDefault = "Develop.rpex";
    const string LessonFileDefault = "Develop.lesn";

    public Topics topics;
    RipeExercises ripeExercises;
    public Lesson lesson;
    public Targs targs;
    public bool initComplete = false;

    float kCue0 = 0.3f;

    public StudyProcess() {
    } // //////////////////////////////////////////////////////////////////

    public void Create(GameObject ball, GameObject object_left, GameObject object_center, GameObject object_right) {
        targs = new Targs(ball, object_left, object_center, object_right);
        LoadTopicFile(TopicsFileDefault);
        LoadRipeExercisesFile(RipeExercisesFileDefault);
        //LoadLessonFile(LessonFileDefault);
        LoadLesson();
        initComplete = true;
    } // /////////////////////////////////////////////////////////////////////////////
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
    public void LoadLesson() {
        lesson = new Lesson();
        List<Exercise> vripe = ripeExercises.getRiped(lesson.ExercisesInLesson);
        int rest = lesson.LoadRipe(vripe);
        if(rest > 0)
            lesson.LoadNew(topics);
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
    // Return sucess (end attempt)
    public bool SetRes(int choose) {
        bool bSeriesSucess = targs.setSelect(choose);
        return bSeriesSucess;
    } // ///////////////////////////////////////////////////////////////////////
    public void saveRes() {
        bool sucess = targs.seriesSucess;
        ripeExercises.setResult(lesson.curExercise, sucess);
        topics.SetRes(sucess);
        bool bContinueLesson = lesson.SetRes(sucess);
        if(!bContinueLesson)
            LoadLesson();
    } // ///////////////////////////////////////////////////////////////////////
    public float dkcue() {
        return kCue0 * (topics.dkcue + topics.curTopic.dkcue + lesson.dkcue + lesson.curExercise.dkcue) / 4;
    } // ////////////////////////////////////////////////////////////////////////////////////////////////
    public void info() {
        UnityEngine.Debug.Log("Study process:");
        topics.info();
        lesson.info();
        ripeExercises.info();
        targs.info("Targs ");
        UnityEngine.Debug.Log("End Study process:");
    } // ////////////////////////////////////////////////////////////////////////////////////////////////
} // *******************************************************************************************
