/*
 * Description: Step 1 in generating level. This is a small section of a level, that contains a beat pattern.
 *              Each rhythm group contains a length, density and beat pattern.
 * 
 * 
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Verbs used for the grammar-based approach algorithm
public enum ACTION
{ 
    MOVE,
    JUMP,
    WAIT,
}


public class RhythmGroup
{
    public List<Rhythm> rhythms = new List<Rhythm>(); //beat pattern
    public float totalTime; //lenght of the beat pattern

    //Constructor for rhythm group containing the beat pattern.
    public RhythmGroup(float totalTime_)
    {
        totalTime = totalTime_;
    }

    //This adds a rhythm to the rhythm group.
    public bool AddRhythm(ACTION action_, float start_, float end_)
    {
        //Check if start time of the beat is in the time this rhythm group exists in.
        if (start_ > totalTime)
        {
            return false;
        }

        //Check if the end time of this rhythm is longer than when the rhythm group is set to end.
        if (end_ > totalTime && action_ != ACTION.JUMP)
        {
            end_ = totalTime;
        }

        //If here we passed the criteria to add the rhythm.

        //Creating the rhythm to then add to the group.
        Rhythm rhythm = new Rhythm(action_, start_, end_);

        rhythms.Add(rhythm);
        
        return true;
    }
}

public class Rhythm
{
    public ACTION action;//Verb of the rhythm. I.E is the rhythm a jump or move action?
    public float start; //Where in the beat pattern it exists. 
    public float end; //The duration of the beat.

    //Constructor for rhythm
    public Rhythm(ACTION action_, float start_, float end_)
    {
        action = action_;
        start = start_;
        end = end_;
    }
}

