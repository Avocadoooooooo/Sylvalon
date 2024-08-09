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
    private bool[] mousePosUpdatedList = new bool[4];//[Down, Up, Right, Left]
    private Vector3 originalMousePos;


    void Start()
    {
        ableToCast = false;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        ground = LayerMask.GetMask("Ground");
    }
    // Update is called once per frame
    void Update()
    {
        checkAbleToCast();
        moveObject();
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
        if (isHeld && ableToCast)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 newPos = new Vector3(mousePos.x - startPosX, mousePos.y - startPosY, 0);


            newPos.y = CheckAndRestrictMovement(newPos, Vector2.down, 0, newPos.y, transform.position.y);
            newPos.y = CheckAndRestrictMovement(newPos, Vector2.up, 1, newPos.y, transform.position.y);
            newPos.x = CheckAndRestrictMovement(newPos, Vector2.right, 2, newPos.x, transform.position.x);
            newPos.x = CheckAndRestrictMovement(newPos, Vector2.left, 3, newPos.x, transform.position.x);

            gameObject.transform.localPosition = newPos;

            Player.playerObject.casting = true;
            Player.playerObject.castingObject = gameObject;
        }
    }
    private float GetHitDistance(Vector2 position, Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(position, direction, maxRaycastDistance, ground);
        Debug.DrawRay(position, direction * hit.distance, Color.red);
        if (hit.collider != null)
        {
            return hit.distance;
        }
        return 99999f;
    }
    float CheckAndRestrictMovement(Vector3 newPos, Vector2 direction, int index, float axisValue, float originalValue)
    {
        float hitDistance = GetHitDistance(newPos, direction);
        Vector3 newMousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);

        if (hitDistance < 0.01f)
        {
            axisValue = originalValue;

            if (!mousePosUpdatedList[index])
            {
                // Save the original mouse position the first time we hit the object
                originalMousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
                mousePosUpdatedList[index] = true;
            }

            // Warp the cursor to its original position to prevent further movement
            Mouse.current.WarpCursorPosition(originalMousePos);
        }

        if (mousePosUpdatedList[index] && hitDistance > 0.1f)
        {
            mousePosUpdatedList[index] = false;
        }

        return axisValue;
    }
}
