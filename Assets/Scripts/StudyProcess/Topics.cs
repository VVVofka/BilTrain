using System.Collections.Generic;
using System;
using System.Diagnostics;

[Serializable]
public class Topics : DKCue {
    public int ntopic { get; private set; } = 0;
    List<Topic> v = new List<Topic>();

    public Topics() {
        v.Add(new Topic("TestDbg", 1,
            5.0f, 5.5f, 10.0f, 0.2f,     // distAimFrom, distCueFrom, angAimDegFrom, kCueFrom,
            5.0f, 5.5f, 10.0f, 0.2f      // distAimTo,   distCueTo,   angAimDegTo,   kCueTo
            ));

        //topics.Add(new Topic("Intro", 30, 
        //    3.0f, 4.0f, 10.0f, 0.2f,     // distAimFrom, distCueFrom, angAimDegFrom, kCueFrom,
        //    5.0f, 7.0f, 30.0f, 0.4f      // distAimTo,   distCueTo,   angAimDegTo,   kCueTo
        //    ));
        //topics.Add(new Topic("Second step I", 20,
        //    1.0f, 3.0f, 05.0f, 0.05f,     // distAimFrom, distCueFrom, angAimDegFrom, kCueFrom,
        //    2.5f, 5.0f, 15.0f, 0.25f      // distAimTo,   distCueTo,   angAimDegTo,   kCueTo
        //   ));
        //topics.Add(new Topic("Second step II", 25,
        //    1.0f, 3.0f, 05.0f, 0.3f,     // distAimFrom, distCueFrom, angAimDegFrom, kCueFrom,
        //    2.5f, 5.0f, 15.0f, 0.5f      // distAimTo,   distCueTo,   angAimDegTo,   kCueTo
        //   ));
    } // ////////////////////////////////////////////////////////////////////////////////////

    public Topic curTopic { get => v[ntopic]; }
    public int Count { get => v.Count; }
    public new void SetRes(bool sucess) {
        base.SetRes(sucess);
        curTopic.SetRes(sucess);
    }

    public Topic ToNextTopic() {
        if(++ntopic >= v.Count) {
            return ToFirstTopic();
        }
        return v[ntopic];
    } // ////////////////////////////////////////////////////////////////////////////////////
    public Topic ToFirstTopic() {
        ntopic = 0;
        return v[0];
    } // ///////////////////////////////////////////////////////////////////////////////
    public void resetNew() {
        foreach(var q in v)
            q.cntInStudyNew = 0;
    } // /////////////////////////////////////////////////////////////////////////////////////
    public void setNew() {
        foreach(var q in v)
            q.cntInStudyCur += q.cntInStudyNew;
    } // /////////////////////////////////////////////////////////////////////////////////////
    public new string info() {
        string s1 ="Start Topics(count=" + v.Count.ToString() + ") :";
        string s = s1  + "\n";
        UnityEngine.Debug.Log(s);
        foreach(var q in v) {
            s1 = q.info;
            s += s1 + "\n";
            UnityEngine.Debug.Log(s1);
        }
        s1 = "End Topics (ntopic=" + ntopic.ToString() + ")";
        UnityEngine.Debug.Log(s1);
        return s + "\n" + s1;
    } // /////////////////////////////////////////////////////////////////////////////////////
} // ****************************************************************************************
