using System.Collections;
using System.Collections.Generic;

public class ExerciseEnh : Exercise {
    public int nluze = 2;
    public float sign = 1.0f;

    public ExerciseEnh(Exercise exercise, int NLuze, int Sign) : base(exercise.layout) {
        base.dkcue = exercise.dkcue;
        //base.layout = exercise.layout;
        base.interval = exercise.interval;
        nluze = NLuze;
        sign = Sign;
    } // ////////////////////////////////////////////////////////////////////////////
} // **********************************************************************
