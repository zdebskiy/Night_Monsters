using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContolBar_HandleController : MonoBehaviour
{
    public Joystick jControl;
    public float maxDistansX;

    private Vector3 startPos = Vector3.zero;
    private Vector3 movePos = Vector3.zero;
    private float moveX = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moveX = jControl.Horizontal;
        if (moveX >=0){
            movePos = new Vector3(Mathf.Lerp(0.0f, maxDistansX, Mathf.Abs(moveX)), startPos.y, startPos.z);
        } else {
            movePos = new Vector3(Mathf.Lerp(0.0f, maxDistansX, Mathf.Abs(moveX)) * -1.0f, startPos.y, startPos.z);
        }

        transform.localPosition = movePos;
    }
}
