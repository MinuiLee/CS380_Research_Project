/*
 * Description: This takes the data from the rhythm generator and using the available geometry functions,
 * creates platforms out of it based on the rhythms duration for jump and movement.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Used for movement and jump states for processing the rhythms.
public class rhythmQueue
{
    public float time;
    public int type;

    //Constructor for rhythm
    public rhythmQueue(float time_, int type_)
    {
        time = time_;
        type = type_;
    }
}


public class GeometryGenerator : MonoBehaviour
{
    public GameObject safePlatform;
    public GameObject dangerPlatform;
    public GameObject dangerPlatformTiered;
    public GameObject physicsData;
    private Player_Physics_Controller controller;

    public float minHeight;
    public float maxHeight;
    private List<rhythmQueue> movementState;
    private List<rhythmQueue> jumpState;
    private Vector2 endPoint; //Place in geometry placing of level.


    public Vector2 GenerateGeometry(RhythmGroup group, Vector2 startPoint)
    {
        movementState = new List<rhythmQueue>();
        jumpState = new List<rhythmQueue>();
        endPoint = startPoint;

        float height = 0.0f;
        float length = 10.0f;

        CreateRestBlock(endPoint.x, endPoint.y, length); //create a rest block in between rhythms to give the player a break from the rhythms.

        GenerateRhythmQueue(group);//Process each rhythm in the group. Organize them by movement and jump state.

        //Using the jump state data, place jump platforms and move platforms for the beat.
        for (int i = 0; i < jumpState.Count; ++i)
        {
            float rhythmDuration = group.totalTime - jumpState[i].time; //Get length of rhythm.

            if (i < jumpState.Count - 1)
            {
                rhythmDuration = jumpState[i + 1].time - jumpState[i].time; //ensures jumps are well spaced and playable by looking ahead to the next jump time.
            }

            //Determing length and height for jump block
            int jumpType = jumpState[i].type;
            switch (jumpType)
            {
                case 0:
                    height = controller.shortJump;
                    break;
                case 1:
                    height = controller.mediumJump;
                    break;
                case 2:
                    height = controller.longJump;
                    break;
                default:
                    height = controller.shortJump;
                    break;
            }

            float jumpTime =  controller.CalculateJumpDuration(height);
            length = jumpTime * controller.movementXSpeed; //Figure out the distance traveled in the x direction given the jump height.

            int randJump = Random.Range(0, 3);

            if(randJump == 0)
            {
                if (jumpType == 0)
                {
                    height -= 1.0f;
                }

                if (jumpType == 1)
                {
                    height -= height / 6.0f;
                }

                if (jumpType == 2)
                {
                    height -= height / 4.0f;
                }

                CreateDangerousBlock(endPoint.x, endPoint.y, length - length / 4.0f, height);
            }
            else
            {
                if (randJump == 1)
                {
                    if (jumpType == 1)
                    {
                        height -= height / 3.0f;
                    }

                    if (jumpType == 2)
                    {
                        height -= height / 2.5f;
                    }

                    CreateDangerousBlockTiered(endPoint.x, endPoint.y, length - length / 6.0f, height);
                }
                else
                {
                    if (randJump == 2)
                    {
                        float oldX = endPoint.x;
                        CreateDangerousBlock(endPoint.x, endPoint.y, length, height);

                        CreateStraightBlockAboveDanger(oldX, endPoint.y + height, length);
                    }
                }
            }

            //With remaining time left after the jump create a move block.
            float timeLeft = rhythmDuration - jumpTime;


            //Jump to next platform
            //Basic square platfrom
            //Stacked platform

            if(timeLeft > 0.0f)
            {
                CreateStraightBlock(endPoint.x,endPoint.y, timeLeft * controller.movementXSpeed);
            }
        }

        return endPoint;
    }

    private void GenerateRhythmQueue(RhythmGroup group)
    {
        controller = physicsData.GetComponent<Player_Physics_Controller>();

        movementState.Clear();
        jumpState.Clear();
        

        float lastBeatTime = 0.0f;
        foreach(Rhythm rhythm in group.rhythms)
        {
            //Check if we need  a waiting period, next rhythm starts at a different point in the pattern.
            if(lastBeatTime != rhythm.start)
            {
                float waitTime = rhythm.start - lastBeatTime;
                lastBeatTime = rhythm.start;

                if(waitTime > 0.0f)
                {
                    movementState.Add(new rhythmQueue(waitTime, (int)ACTION.WAIT));
                    Debug.Log("Waiting: " + waitTime);
                }
            }

            if(rhythm.action == ACTION.MOVE)
            {
                movementState.Add(new rhythmQueue(rhythm.end - rhythm.start, (int) ACTION.MOVE));//Move contains length of move.
                Debug.Log("moving: " + (rhythm.end - rhythm.start));

            }

            if (rhythm.action == ACTION.JUMP)
            {
                float jumpDuration = rhythm.end - rhythm.start;
                int jumpType = 0;

                Debug.Log("jumpingdur: " + jumpDuration);

                if (jumpDuration <= controller.shortJumpDuration)
                {
                    jumpType = 0;
                }
                else
                {
                    if (jumpDuration >= controller.mediumJumpDuration && jumpDuration < controller.longJumpDuration)
                    {
                        jumpType = 1;
                    }
                    else
                    {
                        if (jumpDuration >= controller.longJumpDuration)
                        {
                            jumpType = 2;
                        }
                    }
                }

                jumpState.Add(new rhythmQueue(rhythm.start, jumpType)); //start time of jump and type of jump.

                Debug.Log("jumping: " + rhythm.start + " type:" + jumpType);

            }

            lastBeatTime = rhythm.end; //start time + duration of action.
        }
    }

    //Geometry functions that the geometry generator uses to place platforms. (Still needs more variation in the platforms for jumps but it would go here.)

    private void CreateDangerousBlock(float xLeft, float yTop, float length, float height)
    {
        GameObject block = Instantiate(dangerPlatform, Vector3.zero, Quaternion.identity);
        block.transform.localScale = new Vector3(length, height, 1);
        block.transform.position = new Vector3(xLeft + length / 2, yTop + height/2, 0);

        endPoint += new Vector2(length, 0);
    }

    private void CreateDangerousBlockTiered(float xLeft, float yTop, float length, float height)
    {
        GameObject block = Instantiate(dangerPlatformTiered, Vector3.zero, Quaternion.identity);
        block.transform.localScale = new Vector3(length, height, 1);
        block.transform.position = new Vector3(xLeft + length / 2, yTop + height / 2, 0);

        endPoint += new Vector2(length, 0);
    }

    private void CreateStraightBlockAboveDanger(float xLeft, float yTop, float length)
    {
        GameObject block = Instantiate(safePlatform, Vector3.zero, Quaternion.identity);
        block.transform.localScale = new Vector3(length, 1, 1);
        block.transform.position = new Vector3(xLeft + length / 2, yTop, 0);
    }

    private void CreateStraightBlock(float xLeft, float yTop, float length)
    {
        GameObject block = Instantiate(safePlatform, Vector3.zero, Quaternion.identity);
        block.transform.localScale = new Vector3(length, 1, 1);
        block.transform.position = new Vector3(xLeft + length / 2, yTop, 0);

        endPoint += new Vector2(length, 0);
    }

    private void CreateRestBlock(float xLeft, float yTop, float length)
    {
        GameObject block = Instantiate(safePlatform, Vector3.zero, Quaternion.identity);
        block.transform.localScale = new Vector3(length, 1, 1);
        block.transform.position = new Vector3(xLeft + length / 2, yTop, 0);
        endPoint += new Vector2(length, 0);
    }
}
