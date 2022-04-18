using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{

    // Debug
    public Text textDebug;

    public GameObject goLoadScreen;
    public Image imgCurtain;

    // Missions
    int MAX_MISSIONS = 3;
    int N_HOUSES = 22;
    int N_DIFFICULTY = 10;
    public Transform tHouses;
    MissionStruct [] arrMissions;

    public struct MissionStruct {
        public int iHouse;
        public int iDifficulty;
    }

    // Scroll town
    public float scrolingSpeed = 1.0f;
    public Transform tTownCircle;
    public AudioClip[] arrClipsHouseUp;
    public AudioClip[] arrClipsBackTown;
    private float moveX; // -1..+1 
    private Vector3 axis = Vector3.back;


    // Control panel
    public GameObject goControlBar;
    public Animator animControlBar; 
    public AudioSource audioSourceControlBar;
    public AudioClip[] arrClipsRollControlBar;
    public GameObject goButtons;
    public Joystick jControl;

    public GameObject goHandPointer;

    private bool isControlAvailable = false;


    private void OnEnable() {
        MenuEventManager.OnEnterTownBorn_MenuCont += OnEnterTownBorn_MenuCont;
        MenuEventManager.OnExitTownBorn_MenuCont += OnExitTownBorn_MenuCont;
        MenuEventManager.OnExitAppNameStart_AnimAppNameController += OnExitAppNameStart_AnimAppNameController;
    }

    private void OnDisable() {
        MenuEventManager.OnEnterTownBorn_MenuCont -= OnEnterTownBorn_MenuCont;
        MenuEventManager.OnExitTownBorn_MenuCont -= OnExitTownBorn_MenuCont;
        MenuEventManager.OnExitAppNameStart_AnimAppNameController -= OnExitAppNameStart_AnimAppNameController;
    }

    // Start is called before the first frame update
    void Start()
    {
        FirebaseManagerController.FBA_EventScreenView("Menu_Screen");
        isControlAvailable = false;
        MissionGenerator();

        HousesInit();

        AudioInit();
    }

    // Update is called once per frame
    void Update()
    {

        //ShowDebugInfo();
        if (isControlAvailable){
            moveX = jControl.Horizontal;
        } else {
            moveX = 0.0f;
        }

        if (goControlBar.activeSelf) {
            if (moveX == 0){
                animControlBar.SetBool("MoveRight", false);
                animControlBar.SetBool("MoveLeft", false);
            } else if (moveX > 0) {
                animControlBar.SetBool("MoveRight", true);
                animControlBar.SetBool("MoveLeft", false);
            } else {
                animControlBar.SetBool("MoveRight", false);
                animControlBar.SetBool("MoveLeft", true);            
            }
        }

        //moveX = Input.GetAxis ("Horizontal");
        //print ("moveX = " + moveX);
        axis = Vector3.forward;
/*        
        if (moveX < 0) {
            axis = Vector3.forward;
        } else {
            axis = Vector3.back;
        }
*/        
        //print ("axis = " + axis);
        tTownCircle.RotateAround(tTownCircle.position, axis, scrolingSpeed*moveX*Time.deltaTime);

        audioSourceControlBar.pitch = Mathf.Abs(moveX);

    }

    void MissionGenerator () {
        bool [] arrEmptyPos = new bool[N_HOUSES];
        for (int j = 0; j < arrEmptyPos.Length; j++) {
            arrEmptyPos[j] = true;
        }

        arrMissions = new MissionStruct[MAX_MISSIONS];

        int i=0; 
        while (i < MAX_MISSIONS) {
            int newITarget = UnityEngine.Random.Range(0, N_HOUSES);
            //int newITarget = i*2; // Debug line
            if (arrEmptyPos[newITarget]) {
                arrMissions[i].iHouse = newITarget;
                arrMissions[i].iDifficulty = UnityEngine.Random.Range(0, N_DIFFICULTY);
                SetFlagsInArr(arrEmptyPos, newITarget);
                i++;
            }
        }
    }

    void SetFlagsInArr(bool [] arr, int iFlag){
        if (iFlag == 0){
            arr[N_HOUSES-1] = false;
            arr[iFlag] = false;
            arr[iFlag+1] = false;
        } else {
            if (iFlag == N_HOUSES-1) {
                arr[iFlag-1] = false;
                arr[iFlag] = false;
                arr[0] = false;
            } else {
                arr[iFlag-1] = false;
                arr[iFlag] = false;
                arr[iFlag+1] = false;
            }
        }
    }

    void HousesInit() {
        Transform tHouse;
        Button btnHouse;
        Image imgHouse;
        Animation animHouse;

        GameObject goNewHandPointer;

        int h=0;
        int iAnimClip=0;
        for (int i=0; i < arrMissions.Length; i++) {
            tHouse = tHouses.GetChild(arrMissions[i].iHouse);
            btnHouse = tHouse.gameObject.GetComponent<Button>();
            imgHouse = tHouse.gameObject.GetComponent<Image>();
            animHouse = tHouse.gameObject.GetComponent<Animation>();

            btnHouse.interactable = true;
            imgHouse.raycastTarget = true;

            iAnimClip = OfsetFromDifficulty(arrMissions[i].iDifficulty);
            Debug.Log(">>>> mission_0"+iAnimClip.ToString());
            animHouse.PlayQueued("mission_0"+iAnimClip.ToString(), QueueMode.PlayNow);                
            animHouse["mission_0"+ iAnimClip.ToString() +" - Queued Clone"].speed = UnityEngine.Random.Range(0.6f, 1.2f);

            goNewHandPointer = Instantiate(goHandPointer, tHouse);
            
            /*
            foreach (AnimationState state in animHouse)
            {          
                Debug.Log(h+") "+state.clip);
                Debug.Log(h+") "+state.length);
                Debug.Log(h+") "+state.name);
                if (h==0) {
                    state.speed = -1.0f;  
                } else {
                    if (h == 1) {
                        state.speed = 2.0f;                          
                    } else {
                        state.speed = 0.2f;
                    }
                }                              
            } 

            //animHouse["mission_00 - Queued Clone"].speed = 4.0f;
            */
            h++;            
        }        
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

    public void GoToScene(int iSelectedHouse) {

        GameDirectorController.curDifficulty = SetLevelDifficulty(iSelectedHouse);

        Debug.Log("Cur Diff: " + GameDirectorController.curDifficulty);

        string sceneName = "Room00Scene";
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
        goLoadScreen.SetActive(true);
        gameObject.SetActive(false);
    }

    public void ShowPos() {
        // --------------------------------------
        Debug.Log("position: " + tHouses.GetChild(0).transform.position);
        //Debug.Log("localPosition: " + tHouses.GetChild(0).transform.localPosition); 
        // --------------------------------------

    }

    private int SetLevelDifficulty (int iSelectedHouse) {
        int levelDifficulty = 0;
        foreach(MissionStruct mission in arrMissions){
            if (mission.iHouse == iSelectedHouse){                    
                if (GameDirectorController.savedWinStreak <= 9){
                    levelDifficulty = GameDirectorController.savedWinStreak;
                } else if (GameDirectorController.savedWinStreak <= 10) {
                    levelDifficulty = 10 + mission.iDifficulty;
                } else {
                    levelDifficulty = 20 + mission.iDifficulty;
                }
                break;
            }
        }
        return levelDifficulty;
    }

    //Debug
    private void ShowDebugInfo(){
        string debugInfo = "Debug Info: \n";
        debugInfo += "Win Streak = " + GameDirectorController.savedWinStreak + ";\n";
        foreach(MissionStruct mission in arrMissions){
            debugInfo += "mission[]: {iHouse = " + mission.iHouse + "; iDifficulty = "+ mission.iDifficulty +";}\n";
        }
        textDebug.text = debugInfo;
    }

    private void OnExitTownBorn_MenuCont() {
        tTownCircle.GetComponent<Animator>().enabled = false;
        goControlBar.SetActive(true);
        isControlAvailable = true;
        int i=0;
        foreach(MissionStruct mission in arrMissions) {
            MenuEventManager.mOnInitMission_MenuCont(i, tHouses.GetChild(mission.iHouse), mission.iDifficulty);          
            i++;
        }        
    }

    private void OnExitAppNameStart_AnimAppNameController() {
        goButtons.SetActive(true);        
    }

    public void HouseClicked(int iHouse){
        
        if (isControlAvailable) {
            isControlAvailable = false;

            Transform _tHouse = tHouses.GetChild(iHouse);

            Vector2 _v2CenterPoint = tHouses.position;
            Vector2 _v2TargetPoint = _tHouse.position;

            Vector2 _v2Target = _v2TargetPoint - _v2CenterPoint;

            float _angleY = 0.0f;
            float _angleX = 0.0f;

            _angleY = Vector2.Angle(Vector2.up, _v2Target);
            _angleX = Vector2.Angle(Vector2.right, _v2Target);

            Debug.Log("Angle Y ------------ " + _angleY);
            Debug.Log("Angle X ------------ " + _angleX);

            if (_angleX < 90){
                _angleY *= -1.0f;
            }
            StartCoroutine(RouteTownMissionToCenter(_angleY, iHouse));        
        } else {
            GoToScene(iHouse);
        }
    }
    
    IEnumerator RouteTownMissionToCenter(float angle, int iSelectedHouse) {
        float moveX = 1.0f;
        moveX = Mathf.Lerp(0.5f, 1.0f, Mathf.Abs(angle)/80.0f);

        Debug.Log("moveX = " + moveX);

        if (angle > 0){
            moveX *= -1.0f;
        }            
        float curAngelMove = 0.0f;
        while (curAngelMove < Mathf.Abs(angle)){            

            curAngelMove += Mathf.Abs(scrolingSpeed*moveX*Time.deltaTime);
            tTownCircle.RotateAround(tTownCircle.position, axis, scrolingSpeed*moveX*Time.deltaTime);
            audioSourceControlBar.pitch = Mathf.Abs(moveX);
            yield return new WaitForEndOfFrame();
        }
        GoToScene(iSelectedHouse);
    }
    
    private void AudioInit(){

        // Init Control Bar
        audioSourceControlBar.pitch = 0.0f;
        audioSourceControlBar.clip = arrClipsRollControlBar[UnityEngine.Random.Range(0, arrClipsRollControlBar.Length)];

        // Init House Up Clip
        AudioSource audioSourseHouse;
        foreach(Transform tHouse in tHouses){
            //Debug.Log(tHouse.name);
            audioSourseHouse = tHouse.GetComponent<AudioSource>();
            audioSourseHouse.clip = arrClipsHouseUp[UnityEngine.Random.Range(0, arrClipsHouseUp.Length)];            
        }
    }

    private void OnEnterTownBorn_MenuCont (){
        AudioSource _audioSource;
        _audioSource = tTownCircle.GetComponent<AudioSource>();
        _audioSource.clip = arrClipsBackTown[UnityEngine.Random.Range(0, arrClipsBackTown.Length)];
        _audioSource.Play();
    }

    public void RateApp (){
		Application.OpenURL("market://details?id=" + Application.identifier);
	}
	public void InformApp (){
		Application.OpenURL("https://docs.google.com/document/d/1_dRrvCCmwFYhq7mWTdZ-Sztf7G4iypxd/edit?usp=sharing&ouid=106630332952662586191&rtpof=true&sd=true");
	}
}
