using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class StudyProcess {
    string ver = "ver:(0.0.1)";
    Topics topics;
    RipeExercises ripeExercises;
    string topicFile = "topics.dat";
    string RipeExercisesFile = "RipeExercises.dat";

    public StudyProcess() {
        LoadTopicFile();
        LoadRipeExercisesFile();
    } // //////////////////////////////////////////////////////////////////
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
        using(FileStream fs = new FileStream(topicFile, FileMode.Create)) {
            formatter.Serialize(fs, ver);
            formatter.Serialize(fs, topics);
        }
    } // ////////////////////////////////////////////////////////////////////////
    void CreateRipeExercisesFile() {
        ripeExercises = new RipeExercises();
        BinaryFormatter formatter = new BinaryFormatter();
        using(FileStream fs = new FileStream(RipeExercisesFile, FileMode.Create)) {
            formatter.Serialize(fs, ver);
            formatter.Serialize(fs, ripeExercises);
        }
    } // ////////////////////////////////////////////////////////////////////////
    public void Close() {
        CreateTopicFile();
        CreateRipeExercisesFile();
    } // ////////////////////////////////////////////////////////////////////////
} // *******************************************************************************************
