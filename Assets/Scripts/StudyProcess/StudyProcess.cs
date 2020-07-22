using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class StudyProcess {
    const string ver = "ver:(0.0.3)";
    const string TopicsFileDefault = "Develop.tpcs";
    const string RipeExercisesFileDefault = "Develop.rpex";
    const string LessonFileDefault = "Develop.lesn";

    Topics topics;
    RipeExercises ripeExercises;
    Lesson lesson;

    float dkcue { get => (topics.dkcue + ripeExercises.dkcue + lesson.dkcue + lesson.curLayout.dkcue) / 4.0f; }

    public StudyProcess() {
        LoadTopicFile(TopicsFileDefault);
        LoadRipeExercisesFile(RipeExercisesFileDefault);
        LoadLessonFile(LessonFileDefault);

        lesson.OnChoose += OnChoose;
        //lesson.RegisterHandler(new Lesson.LessonStateHandler(DelStuded));
    } // //////////////////////////////////////////////////////////////////
    public Layout curlay { get => lesson.curLayout; } // ///////////////////
    public Layout moveNext() {
        Layout lay = lesson.moveNext();
        if(lay != null)
            return lay;
        LoadLesson();
        return lesson.moveFirst();
    } // //////////////////////////////////////////////////////////////////////
    void LoadLessonFile(string fname) {
        BinaryFormatter formatter = new BinaryFormatter();
        bool fileOk;
        if(File.Exists(fname)) {
            using(FileStream fs = new FileStream(fname, FileMode.Open)) {
                string readver = (string)formatter.Deserialize(fs);
                fileOk = (readver == ver);
                if(fileOk)
                    lesson = (Lesson)formatter.Deserialize(fs);
            }
            if(!fileOk) {
                File.Delete(fname);
                CreateLessonFile(fname);
            }
        } else
            CreateLessonFile(fname);
    } // //////////////////////////////////////////////////////////////////
    public Layout LoadLesson() {
        List<Exercise> vripe = ripeExercises.getRiped(lesson.ExercisesInLesson);
        int rest = lesson.LoadRipe(vripe);
        if(rest > 0) {
            Topic topic = topics.topic;
            if(topic.cntCur >= topic.cntMax)
                topic = topics.ToNextTopic();
            lesson.LoadNew(topic);
        }
        lesson.moveFirst();
        return lesson.curLayout;
    } // ///////////////////////////////////////////////////////////////////
    void LoadTopicFile(string fname) {
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
    } // //////////////////////////////////////////////////////////////////
    void LoadRipeExercisesFile(string fname) {
        bool fileOk;
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
    } // ///////////////////////////////////////////////////////////////////////////
    void CreateTopicFile(string fname) {
        topics = new Topics();
        BinaryFormatter formatter = new BinaryFormatter();
        using(FileStream fs = new FileStream(fname, FileMode.Create)) {
            formatter.Serialize(fs, ver);
            formatter.Serialize(fs, topics);
        }
    } // ////////////////////////////////////////////////////////////////////////
    void CreateLessonFile(string fname) {
        lesson = new Lesson();
        BinaryFormatter formatter = new BinaryFormatter();
        using(FileStream fs = new FileStream(fname, FileMode.Create)) {
            formatter.Serialize(fs, ver);
            formatter.Serialize(fs, lesson);
        }
    } // ////////////////////////////////////////////////////////////////////////
    void CreateRipeExercisesFile(string fname) {
        ripeExercises = new RipeExercises();
        BinaryFormatter formatter = new BinaryFormatter();
        using(FileStream fs = new FileStream(fname, FileMode.Create)) {
            formatter.Serialize(fs, ver);
            formatter.Serialize(fs, ripeExercises);
        }
    } // ////////////////////////////////////////////////////////////////////////
    public void Close() {
        CreateTopicFile(TopicsFileDefault);
        CreateRipeExercisesFile(RipeExercisesFileDefault);
        CreateLessonFile(LessonFileDefault);
    } // ////////////////////////////////////////////////////////////////////////
    public void SetRes(bool sucess) {
        lesson.SetRes(sucess);

    } // ///////////////////////////////////////////////////////////////////////
    void OnChoose(bool isSucess) {
        foreach(var x in lesson.vstuded) {

        }
    } // ///////////////////////////////////////////////////////////////////////
} // *******************************************************************************************
