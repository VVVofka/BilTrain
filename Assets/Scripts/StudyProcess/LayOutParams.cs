[System.Serializable]
public class LayoutParams
    {
    public float distAimInD;
    public float distCueInD;
    public float angAimDeg;
    public float kCue;

    public LayoutParams() {
      distAimInD = 0;
      distCueInD = 0;
      angAimDeg = 0;
      kCue = 1;
    } // ////////////////////////////////////////////////////////////////////////////////////////

    public LayoutParams(float dist_aim_in_d, float dist_cue_in_d, float ang_aim_deg, float k_cue) {
      distAimInD = dist_aim_in_d;
      distCueInD = dist_cue_in_d;
      angAimDeg = ang_aim_deg;
      kCue = k_cue;
    } // //////////////////////////////////////////////////////////////////////////////////////
}
