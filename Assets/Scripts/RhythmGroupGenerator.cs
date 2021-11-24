using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmGroupGenerator : MonoBehaviour
{
    public float jumpDuration = 0.3f;

    private enum BEAT_TYPE
    {
        REGULAR,
        SWING,
        RANDOM,
        TOTAL
    }

    private enum DENSITY
    { 
        HIGH = 1,
        MEDIUM,
        LOW,
        TOTAL
    }

    public RhythmGroup GenerateRhythmGroup(float totalTime)
    {
        RhythmGroup group = new RhythmGroup(totalTime);
        BEAT_TYPE beatType = (BEAT_TYPE)Random.Range(0, (int)BEAT_TYPE.TOTAL-1);
        DENSITY densityType = (DENSITY)Random.Range(1, (int)DENSITY.TOTAL - 1);

        group.AddRhythm(ACTION.MOVE, 0, totalTime);

        List<float> beats = GetBeats(totalTime, beatType, densityType);

        float lastJumpEnd = 0.0f;
        foreach(float beat in beats)
        {
            if(Mathf.Abs(lastJumpEnd - beat) < 0.3f)
            {
                continue;
            }
            else
            {
                group.AddRhythm(ACTION.JUMP, beat, beat + jumpDuration);
                lastJumpEnd = beat + jumpDuration;
            }
        }

        return group;
    }

    private List<float> GetBeats(float totalTime, BEAT_TYPE beatType, DENSITY densityType)
    {
        List<float> beats = new List<float>();

        float density = Mathf.Sqrt((float)densityType * 3f);
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
