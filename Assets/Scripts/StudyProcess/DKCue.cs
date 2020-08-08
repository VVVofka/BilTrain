using System;
using System.Collections.Generic;

[Serializable]
public class DKCue {
    public float dkcue { get; protected set; } = 1.0f;
    protected float stepdk = 0.01f;
    protected int level = 85;
    Queue<sbyte> q = new Queue<sbyte>(100);
    int sum = 0;
    int cnt = 0;

    public void SetRes(bool sucess) {
        if(cnt > 100) {
            sum -= q.Dequeue();
        } else {
            cnt++;
        }
        if(sucess) {
            q.Enqueue(1);
            if((++sum * 100) / cnt > level)
                dkcue *= 1 - stepdk;
        } else {
            q.Enqueue(0);
            if((sum * 100) / cnt <= level)
                dkcue *= 1 + stepdk;
        }
    } // ///////////////////////////////////////////////////////////////////////////////////////
    public string info { get => dkcue.ToString() + " "; }
} // *************************************************************************************************
