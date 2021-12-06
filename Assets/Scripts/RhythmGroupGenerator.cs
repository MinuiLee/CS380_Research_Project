
/* 
 * Description: This is where the rythm is made. Using the paramters passed in by the designer to determine
 * the passing of the beat and how each action is spaced out to create the ryhtm
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmGroupGenerator : MonoBehaviour
{
    public GameObject physicsData; //Referencing the jump physics data for each jump a player can make.
    private Player_Physics_Controller controller;
    //This is the passing of the rhythm.
    private enum BEAT_TYPE
    {
        REGULAR, //evenly spaced out beats
        SWING, //short and long beats mixed together
        RANDOM, //beats placed at random intervals
        TOTAL
    }

    //Number of actions in the rhythm
    private enum DENSITY
    { 
        LOW = 3,// Minimum amount of actions in a single rhythm, currently set to 1 but this can be changed.
        MEDIUM,
        HIGH,
        TOTAL
    }

    //Generating the beat pattern
    public RhythmGroup GenerateRhythmGroup(float totalTime)
    {
        controller = physicsData.GetComponent<Player_Physics_Controller>();
        RhythmGroup group = new RhythmGroup(totalTime);

        //Determing the beat pattern and density at random: This is what we want because it provides more of a variety for the level.
        BEAT_TYPE beatType = (BEAT_TYPE)Random.Range(0, (int)BEAT_TYPE.TOTAL-1); //Chosing at random the rhythm groups beat pattern.
        DENSITY densityType = (DENSITY)Random.Range((int)DENSITY.LOW, (int)DENSITY.TOTAL - 1); //Chosing at random the density for the beat pattern.

        if (Saving_State_Variables.density + (int)DENSITY.LOW < (int)DENSITY.TOTAL)
        {

            densityType = (DENSITY)Saving_State_Variables.density + (int)DENSITY.LOW;
        }

        if (Saving_State_Variables.beatPattern < (int)BEAT_TYPE.TOTAL)
        {
            beatType = (BEAT_TYPE)Saving_State_Variables.beatPattern;
        }

        group.AddRhythm(ACTION.MOVE, 0, totalTime);

        List<float> beats = GetBeats(totalTime, beatType, densityType);

        float lastJumpEnd = 0.0f;
        foreach (float beat in beats)
        {
            if (Mathf.Abs(lastJumpEnd - beat) < controller.shortJumpDuration) //Smaller than shortest jump duration
            {
                continue;
            }
            else
            {
                int jumpType = Random.Range(0, 3);

                float jumpDuration = 0.0f;

                

                switch (jumpType)
                {
                    case 0:
                        jumpDuration = controller.shortJumpDuration;
                        break;
                    case 1:
                        jumpDuration = controller.mediumJumpDuration;
                        break;
                    case 2:
                        jumpDuration = controller.longJumpDuration;
                        break;
                    default:
                        jumpDuration = 0.0f;
                        break;
                }

                group.AddRhythm(ACTION.JUMP, beat, beat + jumpDuration);
                lastJumpEnd = beat + jumpDuration;
            }
        }

        return group;
    }

    //Generating the beats pattern using grammar actions: MOVE and JUMP
    private List<float> GetBeats(float totalTime, BEAT_TYPE beatType, DENSITY densityType)
    {
        List<float> beats = new List<float>();

        float density = (int) densityType;
        int size = (int)Mathf.Floor(totalTime / (float)density);

        float shortBeat = totalTime / (2 * (totalTime - 1));
        float longBeat = 3 * shortBeat;

        for (int i = 0; i < size; ++i)
        {
            switch (beatType)
            {
                case BEAT_TYPE.REGULAR:
                    beats.Add(i * (totalTime / size));
                    break;
                case BEAT_TYPE.SWING:
                    if(i % 2 == 0)
                    {
                        beats.Add(i / 2 * (longBeat + shortBeat));
                    }
                    else
                    {
                        beats.Add((i - 1) / 2 * (longBeat + shortBeat) + longBeat);
                    }
                    break;
                case BEAT_TYPE.RANDOM:
                    float beat = Random.Range(0.1f, totalTime);
                    while (beats.Contains(beat))
                    {
                        beat = Random.Range(0.1f, totalTime);
                    }
                    beats.Add(beat);
                    break;
            }
        }

        // sorts the beats by lowest to highest
        if(beatType == BEAT_TYPE.RANDOM)
        {
            beats.Sort();
            beats.Reverse();
        }

        return beats;
    }
}
