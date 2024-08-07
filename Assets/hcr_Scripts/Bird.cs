using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    public GameObject followItem;
    private float movespeed = 10f;
    private Vector3 diff;

    private Rigidbody2D rigidBody;
    // Start is called before the first frame update
    void Start()
    {
        diff = gameObject.transform.position - followItem.transform.position;
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(followItem.activeSelf == false)
        {
            followItem = Player.playerObject.gameObject;
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        birdMove();
    }
    

    void birdMove()
    {
        Vector3 destination = followItem.transform.position + diff;
        Vector3 direction = destination - gameObject.transform.position;

        rigidBody.velocity = direction.normalized * movespeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "DangerCreature")
        {
            Player.playerObject.casting = false;
            gameObject.SetActive(false);
        }
    }
}
