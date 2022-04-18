using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public GameObject goWardrobe;
    public Animator animatorRoom;
    private Animation animWardrobe;
    private AudioSource audioSourceWardrobe;
    public AudioClip[] arrAudioWardrobeDoor;
    public SpriteRenderer srBotDrawer;
    public SpriteRenderer srTopDrawer;
    public SpriteRenderer srLeftDoor;
    public SpriteRenderer srRightDoor;


    // Var for animations
    public GameObject goMainFurniture;
    public GameObject goRestFurniture;

    public AudioClip[] arrClipsRoomBuild;
    private AudioSource audioSourceRoom;
    private enum RoomBuildClips {Wall, Main, Rest};


    private void OnEnable() {
        EventManager.OnAddMonsterInRoom_MM += OnAddMonsterInRoom_MM;
        EventManager.OnMonstersShiftedBack_MM += OnMonstersShiftedBack_MM;

        EventManager.OnInitRoom_GM += OnInitRoom_GM;
        EventManager.OnNeedPaintErrorIndicator_GM += OnNeedPaintErrorIndicator_GM;

        EventManager.OnStartAnimationBuildWall_RMAnim += OnStartAnimationBuildWall_RMAnim;
        EventManager.OnEndAnimationBuildWall_RMAnim += OnEndAnimationBuildWall_RMAnim;
        EventManager.OnStartAnimationBuildMainFurniture_RMAnim += OnStartAnimationBuildMainFurniture_RMAnim;
        EventManager.OnEndAnimationBuildMainFurniture_RMAnim += OnEndAnimationBuildMainFurniture_RMAnim;
        EventManager.OnStartAnimationBuildRestFurniture_RMAnim += OnStartAnimationBuildRestFurniture_RMAnim;
        EventManager.OnEndAnimationBuildRestFurniture_RMAnim += OnEndAnimationBuildRestFurniture_RMAnim;     

        
    }

    private void OnDisable() {
        EventManager.OnAddMonsterInRoom_MM -= OnAddMonsterInRoom_MM;
        EventManager.OnMonstersShiftedBack_MM -= OnMonstersShiftedBack_MM;

        EventManager.OnInitRoom_GM -= OnInitRoom_GM;
        EventManager.OnNeedPaintErrorIndicator_GM -= OnNeedPaintErrorIndicator_GM;

        EventManager.OnStartAnimationBuildWall_RMAnim -= OnStartAnimationBuildWall_RMAnim;
        EventManager.OnEndAnimationBuildWall_RMAnim -= OnEndAnimationBuildWall_RMAnim;        
        EventManager.OnStartAnimationBuildMainFurniture_RMAnim -= OnStartAnimationBuildMainFurniture_RMAnim;
        EventManager.OnEndAnimationBuildMainFurniture_RMAnim -= OnEndAnimationBuildMainFurniture_RMAnim;
        EventManager.OnStartAnimationBuildRestFurniture_RMAnim -= OnStartAnimationBuildRestFurniture_RMAnim;
        EventManager.OnEndAnimationBuildRestFurniture_RMAnim -= OnEndAnimationBuildRestFurniture_RMAnim;         
    }

    

    // Start is called before the first frame update    
    void Start()
    {
        animWardrobe = goWardrobe.GetComponent<Animation>();
        audioSourceWardrobe = goWardrobe.GetComponent<AudioSource>();
        audioSourceRoom = gameObject.GetComponent<AudioSource>();
        Debug.Log("audioSourceRoom = "+audioSourceRoom);
        SetColorWardrobe(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnAddMonsterInRoom_MM(){       
        SetColorWardrobe(0);        
        animWardrobe.Play("OpenDoors");
        audioSourceWardrobe.PlayOneShot(arrAudioWardrobeDoor[UnityEngine.Random.Range(0, arrAudioWardrobeDoor.Length)], 0.7f);
    }
    private void OnMonstersShiftedBack_MM() {
        SetColorWardrobe(0);
    }

    private void SetColorWardrobe (int iStep){
        switch (iStep)
        {
            case 0:
                srLeftDoor.color = Color.white;
                srRightDoor.color = Color.white;
                srTopDrawer.color = Color.white;
                srBotDrawer.color = Color.white;
                break;
            case 1:
                srLeftDoor.color = Color.white;
                srRightDoor.color = Color.white;
                srTopDrawer.color = Color.white;
                srBotDrawer.color = Color.red;            
                break;
            case 2:
                srLeftDoor.color = Color.white;
                srRightDoor.color = Color.white;
                srTopDrawer.color = Color.red;
                srBotDrawer.color = Color.red;                        
                break;
            case 3:
                srLeftDoor.color = Color.red;
                srRightDoor.color = Color.red;
                srTopDrawer.color = Color.red;
                srBotDrawer.color = Color.red;            
                break;                                                                     
        }
    }

    void OnNeedPaintErrorIndicator_GM(int nError) {
        SetColorWardrobe(nError);
    }
    void OnStartAnimationBuildWall_RMAnim() {
        audioSourceRoom.PlayOneShot(arrClipsRoomBuild[(int)RoomBuildClips.Wall]);
    }
    void OnEndAnimationBuildWall_RMAnim() {

    }
    void OnStartAnimationBuildMainFurniture_RMAnim () {
        goMainFurniture.SetActive (true);
        audioSourceRoom.PlayOneShot(arrClipsRoomBuild[(int)RoomBuildClips.Main]);
    }
    void OnEndAnimationBuildMainFurniture_RMAnim () {    
    }

    void OnStartAnimationBuildRestFurniture_RMAnim () {
        goRestFurniture.SetActive (true);        
        audioSourceRoom.PlayOneShot(arrClipsRoomBuild[(int)RoomBuildClips.Rest]);
    }
    void OnEndAnimationBuildRestFurniture_RMAnim () {    
        EventManager.mOnInitRoomEnd_RM();
    }    

    private void OnInitRoom_GM(int maxSelectedCards) {
        animatorRoom.enabled = true;
    }

/*
    void BuildMainFurniture (Transform [] tArr){        
        for (int i = 0; i < tArr.Length; i++){
            
        }
    }

    IEnumerator LinearChangePositionCoroutine(Transform t, Vector3 startVector, Vector3 finishVector, float animTime) {
        float time = 0;
        while (time < animTime){
            t.localPosition = Vector3.Lerp (startVector, finishVector, time / animTime);
            //Debug.Log("y = "+t.localPosition.y+" ( % = "+ time / animTime);
            //Debug.Log("time = "+time+" < "+ animTime);
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        t.localPosition = finishVector;
    }
*/


}