using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowsController : MonoBehaviour
{
    public GameObject[] arrowPrefabs; // Prefab i from DIFFICULTY
    public float X_CENTER;
    public float Y_BOT;

    private Camera mainCam;

    private void OnEnable() {
        MenuEventManager.OnInitMission_MenuCont += OnInitMission_MenuCont;        
    }    

    private void OnDisable() {
        MenuEventManager.OnInitMission_MenuCont -= OnInitMission_MenuCont;
    }

    void Start()
    {
        mainCam = Camera.main;
    }

    void Update()
    {
        
    }

    void OnInitMission_MenuCont (int iArrow, Transform tHouse, int iDifficulty) {
        //Debug.Log("ArrowsController -> i: " + iArrow + "; iDif: " + iDifficulty);

        int iArrowPrefab = 0;
        GameObject goNewArrow;
        Vector3 screenPosArrow = Vector3.zero;

        iArrowPrefab = UnityEngine.Random.Range(0, arrowPrefabs.Length/3);
        //Debug.Log("iArrowPrefab ===> " + iArrowPrefab);
        //Debug.Log("GetPrefab N = "+ ((iArrowPrefab * 3) + iDifficulty/3));
        iArrowPrefab = (iArrowPrefab * 3) + OfsetFromDifficulty(iDifficulty);
        goNewArrow = Instantiate(arrowPrefabs[iArrowPrefab], transform);
        
        if (tHouse.position.x <= X_CENTER) {
            goNewArrow.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            screenPosArrow.x = Screen.width * 0.02f;
        } else {
            goNewArrow.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 180.0f);
            screenPosArrow.x = Screen.width * 0.98f;
        }
        screenPosArrow.y = Screen.height * ((2.0f + iArrow) / 6.0f);
        screenPosArrow.z = mainCam.nearClipPlane;
        goNewArrow.transform.position = mainCam.ScreenToWorldPoint(screenPosArrow);

        if (tHouse.position.y < Y_BOT) {
            goNewArrow.GetComponent<Animator>().SetBool("IsDisable", false);
        } else {
            goNewArrow.GetComponent<Animator>().SetBool("IsDisable", true);
        }

        ArrowController arrowController = goNewArrow.GetComponent<ArrowController>();
        arrowController.tHouse = tHouse;
        arrowController.X_CENTER = X_CENTER;
        arrowController.Y_BOT = Y_BOT;
    }

    private int OfsetFromDifficulty(int iDifficulty){
        int retvalue = 0;
        if (iDifficulty <= 2) {
            retvalue = 0;
        } else if (iDifficulty <= 5) {
            retvalue = 1;
        } else {
            retvalue = 2;
        }
        return retvalue;        
    }
}
