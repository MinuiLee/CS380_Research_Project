
/* 
 * Description: Player movement controller, that also stores jump physics data for how far a certain jump goes in the x and y direction, given the hold time for the space bar. 
 * 
 * Reason for this: In order for the geometry generator to create playable levels, it needs to know how far a player can jump given how long the player may press the space bar
 *              so that it can correctly space out the geometry to maximize its challenge. 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Physics_Controller : MonoBehaviour
{
    
    private float spaceButtonPressLength;
    private float buttonPressStartTime;
    private int jumpType;
    private float playerJumpVel;
    [HideInInspector]
    public float movementXSpeed;

    //Physics data that geometry generator uses. 
    [HideInInspector]
    public float shortJumpDuration; 
    
    [HideInInspector]
    public float mediumJumpDuration;

    [HideInInspector]
    public float longJumpDuration;

    public float shortJump;
    public float mediumJump;
    public float longJump;
    private Rigidbody2D RB;

    public bool canJump = false;
    public bool spaceUp = true;

    // Start is called before the first frame update
    void Start()
    {
        movementXSpeed = 5.0f;
        RB = GetComponent<Rigidbody2D>();
        spaceButtonPressLength = 0.0f;
        playerJumpVel = 0.0f;
        shortJump = 3.0f;
        mediumJump = 10.0f;
        longJump =  20.0f;

        //calculating jump durations given the heights of each type of jump.
        shortJumpDuration = CalculateJumpDuration(shortJump);
        mediumJumpDuration = CalculateJumpDuration(mediumJump);
        longJumpDuration = CalculateJumpDuration(longJump);
    }

    // Update is called once per frame
    void Update()
    {
        //Calculating the time for a space bar press 
        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            buttonPressStartTime = Time.time;
            canJump = false;
            spaceUp = false;
        }

        if (Input.GetKey(KeyCode.D))
        {
            RB.velocity = new Vector2(movementXSpeed, RB.velocity.y);
        }

        if (Input.GetKey(KeyCode.A))
        {
            RB.velocity = new Vector2(-movementXSpeed, RB.velocity.y);
        }

        //Determing the type of  jump the player will do.
        if (Input.GetKey(KeyCode.Space) == true && buttonPressStartTime != 0.0f && !spaceUp)
        {
            spaceButtonPressLength = Time.time - buttonPressStartTime;
            jumpType = TypeOfJump(spaceButtonPressLength);

            //Doing the jump by calculating its velocity given the jump that is based from the space bar hold length.
            switch (jumpType)
            {
                case 0:
                    playerJumpVel = CalculateJumpVelocity(shortJumpDuration/2.0f);
                    break;
                case 1:
                    playerJumpVel = CalculateJumpVelocity(mediumJumpDuration / 2.0f);
                    break;
                case 2:
                    playerJumpVel = CalculateJumpVelocity(longJumpDuration / 2.0f);
                    break;
                default:
                    playerJumpVel = 0.0f;
                    break;
            }

            playerJumpVel = Mathf.Lerp(RB.velocity.y, playerJumpVel, 0.5f);

            RB.velocity = new Vector2(RB.velocity.x, playerJumpVel);

            if (spaceButtonPressLength >= 2.0f)
                buttonPressStartTime = 0.0f;
        }
        
        if (Input.GetKeyUp(KeyCode.Space))
        {
            spaceUp = true;
        }
    }

    //Returns 0 = short, 1 = medium, 2 = long (JUMPS)
    public int TypeOfJump(float buttonPressLength)
    {
        if (spaceButtonPressLength <= 0.25f)
        {
            return 0; //short jump
        }
        else
        {
            if (spaceButtonPressLength > 0.25f && spaceButtonPressLength <= 0.75f)
            {
                return 1; //medium jump
            }
            else
            {
                if (spaceButtonPressLength > 0.75f)
                    return 2; //long jump
            }
        }

        return -1;//Invalid return but just in case any of the other checks fail.
    }

    //Calculating jump duration based on given height.
    public float CalculateJumpDuration(float height)
    {
        //This calculates from start to peak.
        float time = 0.0f;

        time = Mathf.Sqrt(Mathf.Abs(height / (.5f * Physics.gravity.y)));

        return 2 * time; //Doubling the start to peak time because it wont change going down.
    }

    //Calculating initial velocity of the player from the given jump height.
    private float CalculateJumpVelocity(float jumpDuration)
    {
        return Mathf.Abs(Physics.gravity.y * jumpDuration);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        canJump = true;
        Debug.Log("Jump");
    }
}
