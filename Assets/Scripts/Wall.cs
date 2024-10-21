using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    
    void Start()
    {
        
    }

    
    void Update()
    {
        // Nesneye tıklandığında fare ile hareket ettirilebilir hale getir

        if (Input.GetMouseButton(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            if (GetComponent<Collider2D>() == Physics2D.OverlapPoint(mousePos))
            {
                transform.position = mousePos;
            }
        }
    }
}
