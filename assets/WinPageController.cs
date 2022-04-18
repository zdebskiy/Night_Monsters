using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinPageController : MonoBehaviour
{
    public GameObject goLoadScreen;
    public Image imgCurtain;

    private void OnEnable() {
        SetAnalitics();
    }

    public void GoToScene(string sceneName) {

        //string sceneName = "MenuScene";
        //random room
        
        imgCurtain.gameObject.SetActive(true);
        goLoadScreen.GetComponent<MenuLoadController>().sceneName = sceneName;
        StartCoroutine(AnimNextScreen());

    }

    IEnumerator AnimNextScreen (){
        Vector3 v3Scale = transform.localScale;
        Color color = imgCurtain.color; 
        float animSpeed = 10.0f;
        while (transform.localScale.x < 12.0f) {
            v3Scale +=  Vector3.one * animSpeed * Time.deltaTime;
            transform.localScale = v3Scale;
            if (transform.localScale.x > 6.0f) {
                color[3] += animSpeed/5 * Time.deltaTime;
                imgCurtain.color = color; 
            }
            yield return true;
        }
        color[3] = 0.0f;
        imgCurtain.color = color;
        goLoadScreen.SetActive(true);
        gameObject.SetActive(false);    
    }    

    public void GoToNextRoomScene(string sceneName) {

        //string sceneName = "MenuScene";
        //random room
        GameDirectorController.SetLevelDifficultyForNextRoom();
        GoToScene(sceneName);
    }

    private void SetAnalitics () {
        Debug.Log("NAME: "+gameObject.name);
        switch (gameObject.name)
        {
            case "GameOverPage":
                FirebaseManagerController.FBA_EventScreenView("Game_Over_Screen");
                FirebaseManagerController.FBA_EventLevelEnd("Room_00", GameDirectorController.curDifficulty, 0);
                break;
            case "WinPage":
                FirebaseManagerController.FBA_EventScreenView("Win_Screen");
                FirebaseManagerController.FBA_EventLevelEnd("Room_00", GameDirectorController.curDifficulty, 1);
                break;
            case "PausePage":
                FirebaseManagerController.FBA_EventScreenView("Pause_Screen");
                break;
        }

    }
}

