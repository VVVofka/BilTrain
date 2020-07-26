using System.Collections.Generic;
using System;

[Serializable]
public class Topics : DKCue {
    public int ntopic { get; private set; } = 0;
    List<Topic> topics = new List<Topic>();

    public Topics() {
        topics.Add(new Topic("Intro", 30, 
            3.0f, 4.0f, 10.0f, 0.2f,     // distAimFrom, distCueFrom, angAimDegFrom, kCueFrom,
            5.0f, 7.0f, 30.0f, 0.4f      // distAimTo,   distCueTo,   angAimDegTo,   kCueTo
            ));
        topics.Add(new Topic("Second step I", 20,
            1.0f, 3.0f, 05.0f, 0.05f,     // distAimFrom, distCueFrom, angAimDegFrom, kCueFrom,
            2.5f, 5.0f, 15.0f, 0.25f      // distAimTo,   distCueTo,   angAimDegTo,   kCueTo
           ));
        topics.Add(new Topic("Second step II", 25,
            1.0f, 3.0f, 05.0f, 0.3f,     // distAimFrom, distCueFrom, angAimDegFrom, kCueFrom,
            2.5f, 5.0f, 15.0f, 0.5f      // distAimTo,   distCueTo,   angAimDegTo,   kCueTo
           ));
    } // ////////////////////////////////////////////////////////////////////////////////////

    public Topic topic { get => topics[ntopic]; }
    public new void SetRes(bool sucess) { base.SetRes(sucess); topic.SetRes(sucess);}
    
    public Topic ToNextTopic() {
        if(++ntopic >= topics.Count) {
            ntopic = 0;
            EndOfTopics?.Invoke();
            return null;
        }
        return topic;
    } // ////////////////////////////////////////////////////////////////////////////////////
    public delegate void TopicsStateHandler();      // Объявляем делегат
    public event TopicsStateHandler EndOfTopics;    // Создаем переменную делегата

} // ****************************************************************************************
