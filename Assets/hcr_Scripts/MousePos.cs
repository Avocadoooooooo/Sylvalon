using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePos : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
