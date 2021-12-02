using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public int groupNumber = 5;
    public float minTotalTime = 5.0f;
    public float maxTotalTime = 10.0f;

    RhythmGroupGenerator rhythmGroupGenerator;
    GeometryGenerator geometryGenerator;

    List<RhythmGroup> rhythmGroups = new List<RhythmGroup>();

    private void Start()
    {
        rhythmGroupGenerator = GetComponent<RhythmGroupGenerator>();
        geometryGenerator = GetComponent<GeometryGenerator>();

        // generate rhythm groups
        for(int i = 0; i < groupNumber; ++i)
        {
            rhythmGroups.Add(rhythmGroupGenerator.GenerateRhythmGroup(Random.Range(minTotalTime, maxTotalTime)));
        }

        Vector2 startPoint = Vector2.zero;

        for (int i = 0; i < groupNumber; ++i)
        {
            startPoint = geometryGenerator.GenerateGeometry(rhythmGroups[i], startPoint);
        }
    }
}
