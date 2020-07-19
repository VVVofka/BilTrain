using System.Collections;
using System.Collections.Generic;

public class ExerciseEnh : Exercise {
    int nluze;

    //public ExerciseEnh() {    }
    public ExerciseEnh(Exercise exercise, int NLuze) {
        base.dkcue = exercise.dkcue;
        base.layout = exercise.layout;
        base.topic = exercise.topic;
        base.interval = exercise.interval;
        nluze = NLuze;
    } // ////////////////////////////////////////////////////////////////////////////
} // **********************************************************************
