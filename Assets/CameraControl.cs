using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public GameObject player;
    public float lerpSpeed = 1f;

    private float vel;
    // Start is called before the first frame update
    void Start()
    {
        vel = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = transform.position;

        newPos.x = player.transform.position.x;
        newPos.y = Mathf.Lerp(transform.position.y, player.transform.position.y, Time.deltaTime * lerpSpeed);
        transform.position = newPos;
    }
}
