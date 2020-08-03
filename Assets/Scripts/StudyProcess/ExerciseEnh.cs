using System.Collections;
using System.Collections.Generic;

public class ExerciseEnh : Exercise {
    public int nluze = 2;

    public ExerciseEnh(Exercise exercise, int NLuze) : base(exercise.layout) {
        base.dkcue = exercise.dkcue;
        //base.layout = exercise.layout;
        base.interval = exercise.interval;
        nluze = NLuze;
    } // ////////////////////////////////////////////////////////////////////////////
} // **********************************************************************
