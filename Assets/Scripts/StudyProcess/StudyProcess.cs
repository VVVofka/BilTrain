using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class StudyProcess {
    const string ver = "ver:(0.0.1)";
    const string topicFile = "topics.dat";
    const string RipeExercisesFile = "RipeExercises.dat";

    Topics topics;
    RipeExercises ripeExercises;
    Lesson lesson = new Lesson();

    public StudyProcess() {
        LoadTopicFile();
        LoadRipeExercisesFile();
    } // //////////////////////////////////////////////////////////////////
    public void Run() {
        List<Exercise> vripe = ripeExercises.getRiped(lesson.ExercisesInLesson, lesson.vluzes, lesson.vsigns);
        int rest = lesson.LoadRipe(vripe);
        if(rest > 0)
            lesson.LoadNew(topics.topic);
    } // ///////////////////////////////////////////////////////////////////
    void LoadTopicFile() {
        BinaryFormatter formatter = new BinaryFormatter();
        bool fileOk;
        if(File.Exists(topicFile)) {
            using(FileStream fs = new FileStream(topicFile, FileMode.Open)) {
                string readver = (string)formatter.Deserialize(fs);
                fileOk = (readver == ver);
                if(fileOk) {
                    topics = (Topics)formatter.Deserialize(fs);
                }
            }
            if(!fileOk) {
                File.Delete(topicFile);
                CreateTopicFile();
            }
        } else 
            CreateTopicFile();
    } // //////////////////////////////////////////////////////////////////
    void LoadRipeExercisesFile() {
        bool fileOk;
        BinaryFormatter formatter = new BinaryFormatter();
        if(File.Exists(RipeExercisesFile)) {
            using(FileStream fs = new FileStream(RipeExercisesFile, FileMode.Open)) {
                string readver = (string)formatter.Deserialize(fs);
                fileOk = (readver == ver);
                if(fileOk) {
                    ripeExercises = (RipeExercises)formatter.Deserialize(fs);
                }
            }
            if(!fileOk) {
                File.Delete(RipeExercisesFile);
                CreateRipeExercisesFile();
            }
        } else
            CreateRipeExercisesFile();
    } // ///////////////////////////////////////////////////////////////////////////
    void CreateTopicFile() {
        topics = new Topics();
        BinaryFormatter formatter = new BinaryFormatter();
        using FileStream fs = new FileStream(topicFile, FileMode.Create);
        formatter.Serialize(fs, ver);
        formatter.Serialize(fs, topics);
    } // ////////////////////////////////////////////////////////////////////////
    void CreateRipeExercisesFile() {
        ripeExercises = new RipeExercises();
        BinaryFormatter formatter = new BinaryFormatter();
        using FileStream fs = new FileStream(RipeExercisesFile, FileMode.Create);
        formatter.Serialize(fs, ver);
        formatter.Serialize(fs, ripeExercises);
    } // ////////////////////////////////////////////////////////////////////////
    public void Close() {
        CreateTopicFile();
        CreateRipeExercisesFile();
    } // ////////////////////////////////////////////////////////////////////////
} // *******************************************************************************************
