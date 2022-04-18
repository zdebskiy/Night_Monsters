using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowController : MonoBehaviour
{

    public Transform tHouse;
    public float X_CENTER;
    public float Y_BOT;
        

    private Vector3 screenPosArrow;
    private Vector3 worldPosArrow;
    private Vector3 bufPos;

    private Animator animArrow;
    private Camera mainCam;

    private Vector3 startPos;
    

    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
        animArrow = gameObject.GetComponent<Animator>();
        worldPosArrow = transform.position;                
        startPos = mainCam.ScreenToWorldPoint(new Vector3(Screen.width/2, Screen.height * 0, 0.0f));
    }

    // Update is called once per frame
    void Update()
    {
        screenPosArrow = Vector3.zero;

        //Debug.Log(GetAngleArrow().ToString()); 

        transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f + GetAngleArrow());

        if (tHouse.position.x <= X_CENTER) {
            screenPosArrow.x = Screen.width * 0.02f;
        } else {            
            screenPosArrow.x = Screen.width * 0.98f;
        }
        bufPos = mainCam.ScreenToWorldPoint(screenPosArrow);
        worldPosArrow.x = bufPos.x;
        transform.position = worldPosArrow;

        if (tHouse.position.y < Y_BOT) {
            animArrow.SetBool("IsDisable", false);
        } else {
            animArrow.SetBool("IsDisable", true);
        }        
    }

    public void ShowAngle () {
        Debug.Log(GetAngleArrow());
    }

    private float GetAngleArrow (){
        float dX=0;
        float dY=0;
        float deg=0;

            //dX = tHouse.position.x - transform.position.x;
            //dY = tHouse.position.y - transform.position.y;
        dX = tHouse.position.x - startPos.x;
        dY = tHouse.position.y - startPos.y;
        if (dX != 0){
            deg = Mathf.Atan(dY /dX) * Mathf.Rad2Deg;
            if (deg < 0) {
                deg = 180.0f + deg;
            }
            return deg;
        } else {
            return 0.0f;
        }
    }


}
