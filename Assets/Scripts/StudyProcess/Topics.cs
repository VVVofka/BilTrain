using System.Collections.Generic;
using System;

[Serializable]
public class Topics : DKCue {
    public int ntopic { get; private set; } = 0;
    List<Topic> topics = new List<Topic>();

    public Topics() {
        topics.Add(new Topic("Intro", 10, 
            1.5f, 3.0f, 0.0f, 0.2f,
            1.5f, 5.0f, 0.0f, 0.4f
            ));
        topics.Add(new Topic("Second step I", 20,
            1.0f, 3.0f, 05.0f, 0.05f,
            2.5f, 5.0f, 15.0f, 0.25f
           ));
        topics.Add(new Topic("Second step II", 25,
            1.0f, 3.0f, 05.0f, 0.3f,
            2.5f, 5.0f, 15.0f, 0.5f
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
