using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBox : MonoBehaviour
{
    public float jumpVelocity = 10;
    public LayerMask groundLayer;
    public LayerMask otherObjectLayer;
    public float boxHeight = 0.5f;

    private Vector2 playerSize;
    private Vector2 boxSize;

    public bool jumpRequest = false;
    public bool grounded;
    private bool onGround = false;
    private bool onOtherObject = false;

    private Rigidbody2D rigidBody;

    public static JumpBox jumpBox;
/*    private Animator _animator;*/

    // Start is called before the first frame update
    void Start()
    {
        jumpBox = this;
        rigidBody = GetComponent<Rigidbody2D>();
        playerSize = GetComponent<SpriteRenderer>().bounds.size;
        boxSize = new Vector2(playerSize.x * 0.1f, boxHeight);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump") && grounded && !Player.playerObject.casting)
        {
            jumpRequest = true;
        }
    }

    private void FixedUpdate()
    {
        if (jumpRequest)
        {
            if (!Player.playerObject.bouncedByMushroom)
            {
                rigidBody.velocity += new Vector2(0, jumpVelocity);
            }
            else
            {
                rigidBody.velocity += new Vector2(0, jumpVelocity*0.3f);
            }
            //Player's jump will have smaller impact when bounced by a mushroom
            jumpRequest = false;
            grounded = false;
        }
        else
        {
            Vector2 boxCenter = (Vector2)transform.position + (Vector2.down * playerSize.y * 0.5f);
            onGround = Physics2D.OverlapBox(boxCenter, boxSize, 0, groundLayer) != null;
            onOtherObject = Physics2D.OverlapBox(boxCenter, boxSize, 0, otherObjectLayer) != null;
            grounded = onGround || onOtherObject;
        }
        if (onGround)
        {
            Player.playerObject.bouncedByMushroom = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = grounded ? Color.red : Color.green;
        Vector2 boxCenter = (Vector2)transform.position + (Vector2.down * playerSize.y * 0.5f);
        Gizmos.DrawWireCube(boxCenter, boxSize);

    }
}
