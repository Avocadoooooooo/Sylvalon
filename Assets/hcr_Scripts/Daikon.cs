using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Daikon : MonoBehaviour
{
    public float speed = 8;
    public float forceMagnitude = 8;

    public Transform rightUp;
    public Transform rightDown;
    public LayerMask layer;

    private bool collided;

    private Rigidbody2D rigidBody;
    private CapsuleCollider2D daikonCollider;

    private float daikonOffset = 2.7f;
    private bool pulledOut = false;
    private Vector2 initialPos;
    private Vector3 movement;
    private float maxVelocity = 15f;

    public bool ableToCast;

    private float startPosY;
    private bool isHeld;
    private SpriteRenderer spriteRenderer;

    private float rangeRadius = 16f;

    // Start is called before the first frame update
    void Start()
    {
        ableToCast = false;
        rigidBody = GetComponent<Rigidbody2D>();
        daikonCollider = GetComponent<CapsuleCollider2D>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        rigidBody.isKinematic = true;
        rigidBody.freezeRotation = true;
        initialPos = transform.position;
    }
    private void Update()
    {
        checkAbleToCast();
        moveObject();
    }
    private void FixedUpdate()
    {
        CheckPulledOut();
        ClampVelocity();
        ChangeGravityScale();
        collided = Physics2D.Linecast(rightUp.position, rightDown.position, layer);
        if (pulledOut)
        {
            Run();
        }
    }
    private void Run()
    {
        if (collided)
        {
            Debug.DrawLine(rightUp.position, rightDown.position, Color.red);
            transform.localScale = new Vector3(transform.localScale.x * -1f, transform.localScale.y, 0);
            speed *= -1f;
        }
        else
        {
            Debug.DrawLine(rightUp.position, rightDown.position, Color.green);
        }
        movement = new Vector3(speed, 0, 0);
        rigidBody.transform.position += movement * Time.deltaTime;
    }

    void CheckPulledOut()
    {
        if (transform.position.y - initialPos.y > daikonOffset && (Input.GetMouseButton(0) == false) && pulledOut == false)
        {
            rigidBody.isKinematic = false;
            rigidBody.AddForce(Vector2.up * forceMagnitude, ForceMode2D.Impulse);
            pulledOut = true;
            daikonCollider.isTrigger = false;
        }
    }

    void ChangeGravityScale()
    {
        if (rigidBody.velocity.y < -0.1f)
        {
            rigidBody.gravityScale = 2f;
        }
    }
    void ClampVelocity()
    {
        rigidBody.velocity = new Vector2(rigidBody.velocity.x, Mathf.Clamp(rigidBody.velocity.y, -maxVelocity, maxVelocity));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "DangerCreature")
        {
            Player.playerObject.casting = false;
            gameObject.SetActive(false);
        }
    }

    private void OnMouseOver()//Change color when mouse over
    {
        if (ableToCast&&!pulledOut)
        {
            spriteRenderer.color = new Color32(222, 152, 160, 255);
        }
        else
        {
            spriteRenderer.color = new Color32(255, 255, 255, 255);
        }
    }

    private void OnMouseExit()
    {
        spriteRenderer.color = new Color32(255, 255, 255, 255);
    }
    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            startPosY = mousePos.y - transform.localPosition.y;
            isHeld = true;
        }
    }
    private void OnMouseUp()
    {
        isHeld = false;
        Player.playerObject.casting = false;
    }
    void checkAbleToCast()// If the object is too far away from the player then not allowed to be moved
    {
        float distToPlayer = Vector3.Distance(transform.position, Player.playerObject.transform.position);
        ableToCast = (distToPlayer < rangeRadius);
    }
    void moveObject()
    {
        if (isHeld&&ableToCast&&!pulledOut)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 newPos = new Vector3(initialPos.x, Mathf.Clamp((mousePos.y - startPosY), initialPos.y, initialPos.y + 10f), 0);
            gameObject.transform.localPosition = newPos;

            Player.playerObject.casting = true;
            Player.playerObject.castingObject = gameObject;
        }
    }
}

