using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5;
    private Rigidbody2D rigidBody;
    private Animator animator;
    public bool casting;
    public bool followed;
    public bool bouncedByMushroom;
    public GameObject castingObject;
    public float castingRadius = 16f;

    private float x;
    private float y;
    public float maxVelocity = 16f;
    public float posMaxVelocity = 20f;
    public float negMaxVelocity = 20f;//adding -ve sign causes problem

    private Vector3 rebirthPos;

    public static Player playerObject;
    // Start is called before the first frame update
    void Start()
    {
        casting = false;
        bouncedByMushroom = false;
        playerObject = this;

        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        rebirthPos = gameObject.transform.position;
        rigidBody.freezeRotation = true;
        rigidBody.isKinematic = false;

    }

    // Update is called once per frame
    private void Update()
    {
        SetAnimator();
    }

    private void FixedUpdate()
    {
        Run();
        ClampVelocity();
        CheckCastingObject();
        Physics2D.IgnoreLayerCollision(7, 8, casting);
        Physics2D.IgnoreLayerCollision(10, 8, casting);
    }

    private void SetAnimator()
    {
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");
        if (x > 0.01f && casting == false)
        {
            rigidBody.transform.eulerAngles = new Vector3(0f, 0f, 0f);
            animator.SetBool("run", true);
        }
        else if (x < -0.01f && casting == false)
        {
            rigidBody.transform.eulerAngles = new Vector3(0f, 180f, 0f);
            animator.SetBool("run", true);
        }
        if (x > -0.01f && x < 0.01f)
        {
            animator.SetBool("run", false);
        }
        if(casting == true)
        {
            animator.SetBool("run", false);
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (mousePos.x - gameObject.transform.position.x > 0)
            {
                rigidBody.transform.eulerAngles = new Vector3(0f, 0f, 0f);
            }
            else
            {
                rigidBody.transform.eulerAngles = new Vector3(0f, 180f, 0f);
            }
        }
        animator.SetFloat("velocityY", rigidBody.velocity.y);
        animator.SetBool("grounded", JumpBox.jumpBox.grounded);
    }

    private void Run()
    {
        if(casting == false)
        {
            Vector3 movement = new Vector3(x, y, 0);
            rigidBody.transform.position += movement * speed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "DangerCreature")
        {
            gameObject.SetActive(false);

            Invoke("Rebirth", 1f);
        }
        if(other.gameObject.tag == "CheckPoint")
        {
            rebirthPos = other.transform.position;
        }
    }
    void CheckCastingObject()
    {
        if(casting && Vector3.Distance(gameObject.transform.position, castingObject.transform.position) > castingRadius)
        {
            casting = false;//if casting object moves out of range while casting then set casting to false;
        }
    }
    private void Rebirth()
    {
        gameObject.transform.position = rebirthPos;
        gameObject.SetActive(true);
    }

    void ClampVelocity()
    {
        if (!bouncedByMushroom)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, Mathf.Clamp(rigidBody.velocity.y, -negMaxVelocity, posMaxVelocity));
        }
        else
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, Mathf.Clamp(rigidBody.velocity.y, -negMaxVelocity, posMaxVelocity*1.5f));
        }

    }
}
