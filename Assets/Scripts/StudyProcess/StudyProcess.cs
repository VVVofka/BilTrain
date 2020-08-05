﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public enum TrueTarg {
    left = -1,
    center = 0,
    right = 1,
    none = -2
} // ***********************************************************************************

public class StudyProcess {
    const string ver = "ver:(0.1.59)";
    const string TopicsFileDefault = "Develop.tpcs";
    const string RipeExercisesFileDefault = "Develop.rpex";
    const string LessonFileDefault = "Develop.lesn";

    public Topics topics;
    RipeExercises ripeExercises;
    public Lesson lesson;
    public TrueTarg curTarg = TrueTarg.none;

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
            List<Exercise> vripe = ripeExercises.getRiped(lesson.ExercisesInLesson);
            int rest = lesson.LoadRipe(vripe);
            if(rest > 0) 
                lesson.LoadNew(topics);
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
    public bool SetRes(TrueTarg aim) {
        try {
            bool sucess = (aim == curTarg);
            lesson.SetRes(sucess);
            topics.SetRes(sucess);
            ripeExercises.setResult(lesson.curExercise, sucess);
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
            on_aim?.Invoke(curTarg);
        } catch(Exception ex) {
            Console.WriteLine($"Исключение in SetRes({isSucess}): {ex.Message}");
            Console.WriteLine($"Метод: {ex.TargetSite}");
            Console.WriteLine($"Трассировка стека: {ex.StackTrace}");
        }
    } // ///////////////////////////////////////////////////////////////////////
    void OnEndLesson() {
        foreach(var q in lesson.vstuded) 
            ripeExercises.Add(q);
        lesson.vstuded.Clear();
        LoadLesson();
    } // /////////////////////////////////////////////////////////////////////////
    public float dkcue() {
        int rnd = Field.rand.Next(-1, 1);
        switch(rnd) {
        case -1:
            curTarg = TrueTarg.left;
            break;
        case 0:
            curTarg = TrueTarg.center;
            break;
        case 1:
            curTarg = TrueTarg.right;
            break;
        default:
            curTarg = TrueTarg.none;
            break;
        }
        return (topics.dkcue + topics.curTopic.dkcue + lesson.dkcue + lesson.curExercise.dkcue) / 4;
    } // ////////////////////////////////////////////////////////////////////////////////////////////////

    public delegate void StateHandlerAim(TrueTarg realAim);   // Объявляем делегат
    public event StateHandlerAim on_aim;                 // Создаем переменную делегата
} // *******************************************************************************************
