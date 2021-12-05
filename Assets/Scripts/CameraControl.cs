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
        Rigidbody2D playerRD = player.GetComponent<Rigidbody2D>();

        float calculatedOrthoSize = originalOrthoSize * (playerTransform.position.y * zoomFactor);
        float clampedOrthoSize = Mathf.Clamp(calculatedOrthoSize, originalOrthoSize, Mathf.Infinity);
        float newOrthoSize = Mathf.Lerp(cameraComp.orthographicSize, clampedOrthoSize, Mathf.Clamp(Time.deltaTime * (playerRD.velocity.magnitude * lerpSpeed), Time.deltaTime, Mathf.Infinity)); ;
        cameraComp.orthographicSize = newOrthoSize;
    }

    private void CameraFollow()
    {
        Vector3 newPos = transform.position;

        Rigidbody2D playerRD = player.GetComponent<Rigidbody2D>();

        newPos.x = player.transform.position.x;
        newPos.y = Mathf.Lerp(transform.position.y, player.transform.position.y, Mathf.Clamp(Time.deltaTime * (playerRD.velocity.magnitude * lerpSpeed), Time.deltaTime, Mathf.Infinity));
        transform.position = newPos;
    }
}
