using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public float offset = 2.5f;
    public GameObject pairHole;
    private Vector3 teleportOffset;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.transform.position.x - gameObject.transform.position.x > 0.01f)
        {
            teleportOffset = new Vector3(-offset, 0, 0);
        }
        else
        {
            teleportOffset = new Vector3(offset, 0, 0);
        }
        teleport(collision.gameObject);
    }
    private void teleport(GameObject item)
    {
        Vector3 teleportPosition = pairHole.transform.position; /*new Vector3(pairHole.transform.position.x, Player.playerObject.transform.position.y, 0f);*/
        item.transform.position = teleportPosition + teleportOffset;
    }
}
