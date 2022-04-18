using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Analytics;
using Firebase.Extensions;

public class GameDirectorController : MonoBehaviour
{
    public static GameDirectorController instance = null;
    
    // APP Info
    public static int savedWinStreak = 0;

    // GAME process
    public static int curDifficulty = 0;

    private void Awake() {
        InitSingleton();        
    }

    // Start is called before the first frame update
    void Start()
    {
        //InitDirector();
    }

    private void OnEnable() {
        EventManager.OnGameWon_GM += OnGameWon_GM;        
        InitDirector();
    }

    private void OnDisable() {
        EventManager.OnGameWon_GM -= OnGameWon_GM;
    }

    private void InitSingleton(){
        if (instance == null) {
	        instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            if(instance == this) {
	            Destroy(gameObject);
            }
        }        
    }

    private void InitDirector(){
        Debug.Log("Inicialize Director!!!");

        if (PlayerPrefs.HasKey ("WinStreak")) {
        // Debug line
		//if (false){    
			//print ("Has NumSession");			
			savedWinStreak = PlayerPrefs.GetInt ("WinStreak");
		} else {
			//print ("Dont has NumSession");
			//savedNumSession = 1;
			PlayerPrefs.SetInt ("WinStreak", savedWinStreak);
			PlayerPrefs.Save();
		}
        Debug.Log("savedWinStreak = " + savedWinStreak);        
        Debug.Log("curDifficulty = " + curDifficulty);
    }

    private void OnGameWon_GM (){
        savedWinStreak++;
	    PlayerPrefs.SetInt ("WinStreak", savedWinStreak);
		PlayerPrefs.Save();        
    }
    
    
    public static void SetLevelDifficultyForNextRoom () {
        int levelDifficulty = curDifficulty;
        int iDifficulty = curDifficulty % 10;
        if (savedWinStreak <= 9){
            levelDifficulty = savedWinStreak;
        } else if (savedWinStreak <= 10) {
            levelDifficulty = 10 + iDifficulty;
        } else {
            levelDifficulty = 20 + iDifficulty;
        }        
        
        curDifficulty = levelDifficulty;
    }    
}
