using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ACTION
{ 
    MOVE,
    JUMP,
}

public class RhythmGroup
{
    public List<Rhythm> rhythms = new List<Rhythm>();
    public float totalTime;

    public RhythmGroup(float totalTime_)
    {
        totalTime = totalTime_;
    }

    public bool AddRhythm(ACTION action_, float start_, float end_)
    {
        if (start_ > totalTime)
        {
            return false;
        }

        if (end_ > totalTime && action_ != ACTION.JUMP)
        {
            end_ = totalTime;
        }

        Rhythm rhythm = new Rhythm(action_, start_, end_);

        rhythms.Add(rhythm);

        return true;
    }
}

public class Rhythm
{
    public ACTION action;
    public float start;
    public float end;

    public Rhythm(ACTION action_, float start_, float end_)
    {
        action = action_;
        start = start_;
        end = end_;
    }
}

