using System.Collections;
using System.Collections.Generic;

public class ExerciseEnh : Exercise {
    int sign;
    int nluze;

    //public ExerciseEnh() {    }
    public ExerciseEnh(Exercise exercise, int signum, int NLuze) {
        layout = exercise.layout;
        topic = exercise.topic;
        interval = exercise.interval;
        base.dkcue = exercise.dkcue;
        sign = signum;
        nluze = NLuze;
    } // ////////////////////////////////////////////////////////////////////////////
} // **********************************************************************
