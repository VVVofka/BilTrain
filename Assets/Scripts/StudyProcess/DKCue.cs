using System;

[Serializable]
public class DKCue {
    public float dkcue { get; protected set; } = 1.0f;
    protected float stepdk = 0.01f;

    public void SetRes(bool sucess) {
        if(sucess)
            dkcue *= 1 - stepdk;
        else
            dkcue *= 1 + stepdk;
    } // ///////////////////////////////////////////////////////////////////////////////////////
} // *************************************************************************************************
