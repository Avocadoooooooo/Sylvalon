using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterJump : MonoBehaviour
{
    public float fallMultiplier = 3f;
    public float lowJumpMultiplier = 5.2f;

    private Rigidbody2D _rigidbody2D;
    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!Player.playerObject.bouncedByMushroom)
        {
            if (_rigidbody2D.velocity.y < -0.01f)
            {
                _rigidbody2D.gravityScale = fallMultiplier;
            }
            else if (_rigidbody2D.velocity.y > 0.01f && !Input.GetButton("Jump"))
            {
                _rigidbody2D.gravityScale = lowJumpMultiplier;
            }
            else
            {
                _rigidbody2D.gravityScale = 1.2f;
            }
        }
        else
        {
            if (_rigidbody2D.velocity.y < -0.01f)
            {
                _rigidbody2D.gravityScale = fallMultiplier;
            }
            else
            {
                _rigidbody2D.gravityScale = 1.5f;
            }
        }

    }
}
