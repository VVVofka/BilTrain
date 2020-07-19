using System.Collections;
using System.Collections.Generic;

public class ExerciseEnh : Exercise {
    public int nluze;

    public ExerciseEnh(Exercise exercise, int NLuze) : base(exercise.topic, exercise.layout) {
        base.dkcue = exercise.dkcue;
        //base.layout = exercise.layout;
        //base.topic = exercise.topic;
        base.interval = exercise.interval;
        nluze = NLuze;
    } // ////////////////////////////////////////////////////////////////////////////
} // **********************************************************************
