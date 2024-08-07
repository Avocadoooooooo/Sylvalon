using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
    public float bounceHeight;

    private Rigidbody2D _rigidbody;
    private Vector3 initialPos;
    // Start is called before the first frame update
    private void Start()
    {
        initialPos = gameObject.transform.position;
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.freezeRotation = true;
    }
    private void Update()
    {
        dragged();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Creature" || collision.gameObject.tag == "Object")
        {
            Vector2 bounceVeclocity = new Vector2(collision.gameObject.GetComponent<Rigidbody2D>().velocity.x, Mathf.Sqrt(bounceHeight * 2 * 9.81f));
            //v = sqrt(2gh)
            Player.playerObject.bouncedByMushroom = true;
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = bounceVeclocity;
        }

    }

    private void dragged()
    {
        if ((gameObject.transform.position - initialPos).x > 0.01f || (gameObject.transform.position - initialPos).y > 0.01f)
        {
            _rigidbody.isKinematic = false;
        }
    }
}
