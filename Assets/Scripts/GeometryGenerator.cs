using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeometryGenerator : MonoBehaviour
{
    public GameObject square;
    public float minHeight;
    public float maxHeight;

    public Vector2 GenerateGeometry(RhythmGroup group, Vector2 startPoint)
    {
        Vector2 endPoint = startPoint;

        foreach(Rhythm rhythm in group.rhythms)
        {
            if (rhythm.action == ACTION.MOVE) continue;

            float height = Random.Range(minHeight, maxHeight);
            float length = rhythm.end - rhythm.start;

            CreateNewSquare(endPoint.x, endPoint.y, length, height);

            endPoint += new Vector2(length, height);
        }

        return endPoint;
    }

    private void CreateNewSquare(float xLeft, float yTop, float length, float height)
    {
        GameObject block = Instantiate(square, Vector3.zero, Quaternion.identity);
        block.transform.localScale = new Vector3(length, height, 0);
        block.transform.position = new Vector3(xLeft + length / 2, yTop - height / 2, 0);
    }

}
