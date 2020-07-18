using System.Collections.Generic;

public class Topics {
    public int ntopic { get; private set; } = 0;
    public int cntloop { get; private set; } = 0;
    List<Topic> topics = new List<Topic>();

    public Topics() {
        topics.Add(new Topic("Intro", 
            1.5f, 3.0f, 0.0f, 0.2f,
            1.5f, 5.0f, 0.0f, 0.4f
            ));
        topics.Add(new Topic("Second step I", 
            1.0f, 3.0f, 05.0f, 0.05f,
            2.5f, 5.0f, 15.0f, 0.25f
           ));
        topics.Add(new Topic("Second step II", 
            1.0f, 3.0f, 05.0f, 0.3f,
            2.5f, 5.0f, 15.0f, 0.5f
           ));
    } // ////////////////////////////////////////////////////////////////////////////////////

    public Topic topic { get => topics[ntopic]; }
    public int cntInLesson { get => 10; }
    public Topic MoveNext() {
        if(++ntopic >= topics.Count) {
            ntopic = 0;
            cntloop++;
        }
        return topic;
    } // ////////////////////////////////////////////////////////////////////////////////////
} // ****************************************************************************************
