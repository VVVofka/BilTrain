using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class Topic : DKCue {
    public string name { get; private set; } = "no name";
    public Layout from { get; private set; }
    public Layout to { get; private set; }
    public int cntInStudyMax { get; private set; }
    public int cntInStudyCur = 0;
    public int cntInStudyNew = 0;

    public Topic(string Name, int countStudy,
                float distAimFrom,
                float distCueFrom,
                float angAimDegFrom,
                float kCueFrom,

                float distAimTo,
                float distCueTo,
                float angAimDegTo,
                float kCueTo) {
        name = Name;
        cntInStudyMax = countStudy;
        cntInStudyCur = 0;
        from = new Layout(distAimFrom, distCueFrom, angAimDegFrom, kCueFrom);
        to = new Layout(distAimTo, distCueTo, angAimDegTo, kCueTo);
    } // /////////////////////////////////////////////////////////////////////////////////
    public string infoAim { get => frmt("Aim", from.distAimInD, to.distAimInD); }
    public string infoCue { get => frmt("Cue", from.distCueInD, to.distCueInD); }
    public string infoAng { get => frmt("Угол", from.angAimDeg, to.angAimDeg); }
    public string infoK { get => frmt("Резка", from.kCue, to.kCue); }
    public new string info { get => base.info + " " + infoAim + " " + infoCue + " " + infoAng + " " + infoK; }

    string frmt(string str, float a, float b) {
        string s = str + ":" + a.ToString();
        if(a != b)
            s += ".." + b.ToString();
        return s;
    } // ///////////////////////////////////////////////////////////////////////////////////
} // ***************************************************************************************
