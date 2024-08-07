using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DragItem : MonoBehaviour
{
    public bool ableToCast;
    public LayerMask ground;
    private float maxRaycastDistance = 100f;

    private float startPosX;
    private float startPosY;
    private bool isHeld;
    private SpriteRenderer spriteRenderer;

    private float rangeRadius = 16f;

    void Start()
    {
        ableToCast = false;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        ground = 1 << LayerMask.NameToLayer("Ground");
    }
    // Update is called once per frame
    void Update()
    {
        checkAbleToCast();
        if (ableToCast)
        {
            moveObject();
        }
    }
    private void OnMouseOver()//Change color when mouse over
    {
        if (ableToCast)
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

        startPosX = mousePos.x - transform.localPosition.x;
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
        if (isHeld)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 newPos = new Vector3(mousePos.x - startPosX, mousePos.y - startPosY, 0);

            float hitDistance = GetHitDistance(newPos);
            float groundLevel;

            if (hitDistance < 0.01f)
            {
                groundLevel = gameObject.transform.position.y;
                newPos.y = groundLevel;
            }

            Vector4 hitVector = GetHitVector(newPos);  

            gameObject.transform.localPosition = newPos;

            Player.playerObject.casting = true;
            Player.playerObject.castingObject = gameObject;
        }
    }
    private float GetHitDistance(Vector2 position)
    {
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.down, maxRaycastDistance, ground);
        Debug.DrawRay(position, Vector2.down * hit.distance, Color.red);
        if (hit.collider != null)
        {
            return hit.distance;
        }
        return 0f;
    }

    private Vector4 GetHitVector(Vector2 position)
    {
        RaycastHit2D hitLeft = Physics2D.Raycast(position, Vector2.left, maxRaycastDistance, ground);
        RaycastHit2D hitRight = Physics2D.Raycast(position, Vector2.right, maxRaycastDistance, ground);
        RaycastHit2D hitUp = Physics2D.Raycast(position, Vector2.up, maxRaycastDistance, ground);
        RaycastHit2D hitDown = Physics2D.Raycast(position, Vector2.down, maxRaycastDistance, ground);
        if (hitDown.collider != null)
        {
            return new Vector4(hitLeft.distance, hitRight.distance, hitUp.distance, hitDown.distance);
        }
        return Vector4.zero;
    }

/*    private void ClampMousePosition()
    {
        var mouse = Mouse.current;
        if () ;
        Mouse.current.WarpCursorPosition(new Vector2(123, 234));
    }*/

}