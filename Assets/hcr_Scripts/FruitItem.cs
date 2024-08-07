using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitItem : MonoBehaviour
{
    public GameObject CollectedEffect;

    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody;
    private CircleCollider2D _circleCollider;

    private Vector3 initialPos;

    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _circleCollider = GetComponent<CircleCollider2D>();
        _rigidbody.isKinematic = true;
        _rigidbody.freezeRotation = true;
        initialPos = gameObject.transform.position;
    }

    private void Update()
    {
        dragged();
    }
    
    private void dragged()
    {
        if((gameObject.transform.position - initialPos).x > 0.01f || (gameObject.transform.position - initialPos).y > 0.01f)
        {
            _circleCollider.isTrigger = false;
            _rigidbody.isKinematic = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _spriteRenderer.enabled = false;
            _circleCollider.enabled = false;

            CollectedEffect.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
