using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bear : MonoBehaviour
{

    private Animator animator;

    private int eatNum;
    private int fullNum = 1;
    private bool isFull;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        eatNum = 0;
    }

    private void FixedUpdate()
    {
        if(eatNum == fullNum)
        {
            gameObject.tag = "Creature";
            isFull = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if((collision.gameObject.tag == "Player" || collision.gameObject.tag == "Daikon") && !isFull)
        {
            animator.SetBool("eat",true);
            StartCoroutine(StopEatingAfterAnimation());
        }

        if (collision.gameObject.tag == "Daikon" && gameObject.tag == "DangerCreature")
        {
            eatNum += 1;
        }

    }
    private IEnumerator StopEatingAfterAnimation()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length); // Waits for the current animation to complete
        animator.SetBool("eat", false);
    }


}

