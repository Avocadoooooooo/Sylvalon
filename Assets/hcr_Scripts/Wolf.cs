using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : MonoBehaviour
{
    public Sprite redWolf;
    public Sprite fullWolf;

    private SpriteRenderer spriteRenderer;

    private int eatNum;
    private int fullNum = 1;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        eatNum = 0;
    }

    private void FixedUpdate()
    {
        if(eatNum == fullNum)
        {
            gameObject.tag = "Creature";
            spriteRenderer.sprite = fullWolf;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player"&& eatNum!= fullNum)
        {
            spriteRenderer.sprite = redWolf;
            gameObject.tag = "DangerCreature";
        }
        if(collision.gameObject.tag == "Creature" && gameObject.tag == "DangerCreature")
        {
            eatNum += 1;
        }

    }
}
