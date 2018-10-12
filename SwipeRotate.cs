using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeRotate : MonoBehaviour {

    void OnEnable()
    {
        transform.rotation = new Quaternion(0, 0, 0, 0);
    }

    void Update()
    {
        if (Input.touchCount == 1)
        {
            Touch touch0 = Input.GetTouch(0);
            if (touch0.phase == TouchPhase.Moved)
            {
                transform.Rotate(touch0.deltaPosition.y * .1f, touch0.deltaPosition.x * -.1f, 0f);
            }            
        }
        else
        {
            transform.Rotate(Vector3.up * Time.deltaTime * 4);
        }
    }
}