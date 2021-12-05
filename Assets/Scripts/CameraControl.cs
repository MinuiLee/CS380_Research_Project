using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public GameObject player;
    public float lerpSpeed = 1f;

    public float zoomFactor;
    private float originalOrthoSize;
    private Transform playerTransform;
    private Camera cameraComp;
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = player.transform;
        cameraComp = Camera.main;

        originalOrthoSize = cameraComp.orthographicSize;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CameraFollow();
        CameraZoom();
    }

    private void CameraZoom()
    {
        cameraComp.orthographicSize = Mathf.Clamp(originalOrthoSize * (playerTransform.position.y * zoomFactor), originalOrthoSize, Mathf.Infinity);
    }

    private void CameraFollow()
    {
        Vector3 newPos = transform.position;

        Rigidbody2D playerRD = player.GetComponent<Rigidbody2D>();

        newPos.x = player.transform.position.x;
        newPos.y = Mathf.Lerp(transform.position.y, player.transform.position.y, Time.deltaTime * (playerRD.velocity.magnitude * lerpSpeed));
        transform.position = newPos;
    }
}
