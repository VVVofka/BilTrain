using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class Topic : DKCue {
    public string name { get; private set; } = "no name";
    public Layout from { get; private set; }
    public Layout to { get; private set; }

    public Topic(string Name,
                float distAimFrom,
                float distCueFrom,
                float angAimDegFrom,
                float kCueFrom,
                
                float distAimTo,
                float distCueTo,
                float angAimDegTo,
                float kCueTo) {
        name = Name;
        from = new Layout(distAimFrom, distCueFrom, angAimDegFrom, kCueFrom);
        to = new Layout(distAimTo, distCueTo, angAimDegTo, kCueTo);
    } // /////////////////////////////////////////////////////////////////////////////////
} // ***************************************************************************************
