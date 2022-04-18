using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{

/*
private void FixedUpdate() {    
    Debug.Log(OnInitCards_GM.GetInvocationList().Length);    
    for (int i = 0; i < OnInitCards_GM.GetInvocationList().Length; i++) {
        Debug.Log("Metod: " + OnInitCards_GM.GetInvocationList()[i].Method);
        Debug.Log("Target: " + OnInitCards_GM.GetInvocationList()[i].Target);
    }
    
}
*/      

    // RoomManager - delegates and events
    public delegate void InitRoomEnd_RM_Delegate();
    public static event InitRoomEnd_RM_Delegate OnInitRoomEnd_RM;

    // RoomManager Animation Control - delegates and events
    public delegate void StartAnimationBuildWall_RMAnim_Delegate();
    public static event StartAnimationBuildWall_RMAnim_Delegate OnStartAnimationBuildWall_RMAnim;
    public delegate void StartAnimationBuildMainFurniture_RMAnim_Delegate();
    public static event StartAnimationBuildMainFurniture_RMAnim_Delegate OnStartAnimationBuildMainFurniture_RMAnim;
    public delegate void StartAnimationBuildRestFurniture_RMAnim_Delegate();
    public static event StartAnimationBuildRestFurniture_RMAnim_Delegate OnStartAnimationBuildRestFurniture_RMAnim;
    public delegate void EndAnimationBuildWall_RMAnim_Delegate();
    public static event EndAnimationBuildWall_RMAnim_Delegate OnEndAnimationBuildWall_RMAnim;
    public delegate void EndAnimationBuildMainFurniture_RMAnim_Delegate();
    public static event EndAnimationBuildMainFurniture_RMAnim_Delegate OnEndAnimationBuildMainFurniture_RMAnim;
    public delegate void EndAnimationBuildRestFurniture_RMAnim_Delegate();
    public static event EndAnimationBuildRestFurniture_RMAnim_Delegate OnEndAnimationBuildRestFurniture_RMAnim;


    // CardsManager - delegates and events
    public delegate void CardClicked_CM_Delegate(int cardNumber); 
    public static event CardClicked_CM_Delegate OnCardClicked_CM;   // When user clicked card
    public delegate void CardSelected_CM_Delegate(int cardNumber, int toyNumber); 
    public static event CardSelected_CM_Delegate OnCardSelected_CM;   // When user selected(open card) or open card(see toy)
    public delegate void InitCardsEnd_CM_Delegate(int iSetCardsPrefabs, int iSetToysPrefabs, int[] poolToys); 
    public static event InitCardsEnd_CM_Delegate OnInitCardsEnd_CM;
    public delegate void EndAnimationMixCards_CM_Delegate();
    public static event EndAnimationMixCards_CM_Delegate OnEndAnimationMixCards_CM;


    //CardManager Animation Control - delegates and events
    public delegate void StartAnimationCard_CMAnim_Delegate(int number);
    public static event StartAnimationCard_CMAnim_Delegate OnStartAnimationOpenCard_CMAnim;
    public static event StartAnimationCard_CMAnim_Delegate OnStartAnimationCloseCard_CMAnim;
    public delegate void EndAnimationCard_CMAnim_Delegate(int number);
    public static event EndAnimationCard_CMAnim_Delegate OnEndAnimationOpenCard_CMAnim;
    public static event EndAnimationCard_CMAnim_Delegate OnEndAnimationCloseCard_CMAnim;
    public static event EndAnimationCard_CMAnim_Delegate OnEndAnimationTrapCard_CMAnim;



    // GameManager - delegates and events
    public delegate void OpenCard_GM_Delegate(int cardNumber); 
    public static event OpenCard_GM_Delegate OnEnableToOpenCard_GM; 
    public static event OpenCard_GM_Delegate OnDisableToOpenCard_GM; 
    public delegate void CheckKillMonster_GM_Delegate(int numberCard, int toyNumber);
    public static event CheckKillMonster_GM_Delegate OnCheckKillMonster_GM;
    public delegate void NeedShootTheMonster_GM_Delegate(int cardNumber, int toyNumber, Transform tMonster);
    public static event NeedShootTheMonster_GM_Delegate OnNeedShootTheMonster_GM;
    public delegate void NeedPaintWrongCard_GM_Delegate(int cardNumber, int toyNumber);
    public static event NeedPaintWrongCard_GM_Delegate OnNeedPaintWrongCard_GM;    
    public delegate void NeedPaintCorrectCard_GM_Delegate(int cardNumber, int toyNumber);
    public static event NeedPaintCorrectCard_GM_Delegate OnNeedPaintCorrectCard_GM;    
    public delegate void MaxSelectedCardsDone_GM_Delegate ();
    public static event MaxSelectedCardsDone_GM_Delegate OnMaxSelectedCardsDone_GM;
    public delegate void NeedAddMonsterInRoom_GM_Delegate ();
    public static event NeedAddMonsterInRoom_GM_Delegate OnNeedAddMonsterInRoom_GM;
    public delegate void NeedShiftMonstersBack_GM_Delegate ();
    public static event NeedShiftMonstersBack_GM_Delegate OnNeedShiftMonstersBack_GM;
    public delegate void GameProcess_GM_Delegate ();
    public static event GameProcess_GM_Delegate OnGameLost_GM;
    public static event GameProcess_GM_Delegate OnGameWon_GM;
    public delegate void GamePause_GM_Delegate (bool state);
    public static event GamePause_GM_Delegate OnGamePause_GM;
    public delegate void NeedPaintErrorIndicator_GM_Delegate (int nError);
    public static event NeedPaintErrorIndicator_GM_Delegate OnNeedPaintErrorIndicator_GM;
    public delegate void TrapCardMixSelected_GM_Delegate(int cardNumber);
    public static event TrapCardMixSelected_GM_Delegate OnTrapCardMixSelected_GM;
    public delegate void TrapCardAddMonsterSelected_GM_Delegate(int cardNumber);
    public static event TrapCardAddMonsterSelected_GM_Delegate OnTrapCardAddMonsterSelected_GM;
    public delegate void InitRoom_GM_Delegate(int maxSelectedCards);
    public static event InitRoom_GM_Delegate OnInitRoom_GM;
    public delegate void InitCards_GM_Delegate(int iGameDifficulty);
    public static event InitCards_GM_Delegate OnInitCards_GM;
    public delegate void InitMonsters_GM_Delegate(int iGameDifficulty, int iSetToysPrefabs, int[] poolToys); 
    public static event InitMonsters_GM_Delegate OnInitMonsters_GM;
    public delegate void FirstOpenTrapCard_GM_Delegate(int cardNumber, int toyNumber);
    public static event FirstOpenTrapCard_GM_Delegate OnFirstOpenTrapCard_GM;
    public delegate void DoTierActions_GM_Delegate();
    public static event DoTierActions_GM_Delegate OnDoTierActions_GM;
    public static event DoTierActions_GM_Delegate OnTierActionsEnd_GM;
    


    // MonstersManager - delegates and events
    public delegate void MonsterKilled_MM_Delegate(); 
    public static event MonsterKilled_MM_Delegate OnMonsterKilled_MM; 
    public delegate void MonsterNotKilled_MM_Delegate(int numberCard, int toyNumber); 
    public static event MonsterNotKilled_MM_Delegate OnMonsterNotKilled_MM; 
    public delegate void MonstersShiftedBack_MM_Delegate ();
    public static event MonstersShiftedBack_MM_Delegate OnMonstersShiftedBack_MM;
    public delegate void ToyKillSomeMonster_MM_Delegate(int numberCard, int toyNumber, Transform tMonster);
    public static event ToyKillSomeMonster_MM_Delegate OnToyKillSomeMonster_MM;
    public delegate void ToyNotKillSomeMonster_MM_Delegate(int numberCard, int toyNumber);
    public static event ToyNotKillSomeMonster_MM_Delegate OnToyNotKillSomeMonster_MM;
    public delegate void AddMonsterInRoom_MM_Delegate ();
    public static event AddMonsterInRoom_MM_Delegate OnAddMonsterInRoom_MM;
    public delegate void InitMonstersEnd_MM_Delegate(int nMonstersInGame);
    public static event InitMonstersEnd_MM_Delegate OnInitMonstersEnd_MM;



    //MonsterManager Animation Control - delegates and events
    public delegate void AnimationAddMonster_MMAnim_Delegate(string sGoName);
    public static event AnimationAddMonster_MMAnim_Delegate OnStartAnimationAddMonster_MMAnim;
    public static event AnimationAddMonster_MMAnim_Delegate OnEndAnimationAddMonster_MMAnim;    
    public delegate void StartAnimationMonster_MMAnim_Delegate(GameObject goMonster);
    public static event StartAnimationMonster_MMAnim_Delegate OnStartAnimationDieMonster_MMAnim;
    public delegate void EndAnimationMonster_MMAnim_Delegate(GameObject goMonster);
    public static event EndAnimationMonster_MMAnim_Delegate OnEndAnimationDieMonster_MMAnim;   
    public delegate void AnimationJumpMonster_MMAnim_Delegate(string sGoName);
    public static event AnimationJumpMonster_MMAnim_Delegate OnStartAnimationJumpMonster_MMAnim;
    public static event AnimationJumpMonster_MMAnim_Delegate OnEndAnimationJumpMonster_MMAnim; 
    public delegate void AnimationWaitMonster_MMAnim_Delegate(string sGoName);
    public static event AnimationWaitMonster_MMAnim_Delegate OnStartAnimationWaitMonster_MMAnim;
    public static event AnimationWaitMonster_MMAnim_Delegate OnEndAnimationWaitMonster_MMAnim;


    // MonsterController - delegates and events
    public delegate void OnBulletHitMonster_MC_Delegate(); 
    public static event OnBulletHitMonster_MC_Delegate OnBulletHitMonster_MC; 


    // ToyBulletController - delegates and events
    public delegate void OnBulletHitMonster_TBC_Delegate(string targetName); 
    public static event OnBulletHitMonster_TBC_Delegate OnBulletHitMonster_TBC; 
    

    // --- ROOM MANAGER ---
    // RoomManager - public static metods
    public static void mOnInitRoomEnd_RM () {
        Debug.Log("EVENT Anim ---> EM.mOnInitRoomEnd_RM");
        var handler = OnInitRoomEnd_RM;
        if (handler != null) handler();
    }    

    // RoomManager Animation Control - public static metods
    public static void mOnStartAnimationBuildWall_RMAnim () {
        Debug.Log("EVENT Anim ---> EM.mOnStartAnimationBuildWall_RMAnim");
        var handler = OnStartAnimationBuildWall_RMAnim;
        if (handler != null) handler();
    }
    public static void mOnStartAnimationBuildMainFurniture_RMAnim () {
        Debug.Log("EVENT Anim ---> EM.mOnStartAnimationBuildMainFurniture_RMAnim");
        var handler = OnStartAnimationBuildMainFurniture_RMAnim;
        if (handler != null) handler();
    }
    public static void mOnStartAnimationBuildRestFurniture_RMAnim () {
        Debug.Log("EVENT Anim ---> EM.mOnStartAnimationBuildRestFurniture_RMAnim");
        var handler = OnStartAnimationBuildRestFurniture_RMAnim;
        if (handler != null) handler();
    }    
    public static void mOnEndAnimationBuildWall_RMAnim () {
        Debug.Log("EVENT Anim ---> EM.mOnEndAnimationBuildWall_RMAnim");
        var handler = OnEndAnimationBuildWall_RMAnim;
        if (handler != null) handler();
    }
    public static void mOnEndAnimationBuildMainFurniture_RMAnim () {
        Debug.Log("EVENT Anim ---> EM.mOnEndAnimationBuildMainFurniture_RMAnim");
        var handler = OnEndAnimationBuildMainFurniture_RMAnim;
        if (handler != null) handler();
    }
    public static void mOnEndAnimationBuildRestFurniture_RMAnim () {
        Debug.Log("EVENT Anim ---> EM.mOnEndAnimationBuildRestFurniture_RMAnim");
        var handler = OnEndAnimationBuildRestFurniture_RMAnim;
        if (handler != null) handler();
    }    


    // --- CARDS MANAGER ---
    // CardsManager - public static metods
    public static void mOnCardClicked_CM(int cardNumber) {
        Debug.Log("EVENT ---> EM.mOnCardClicked_CM Card="+cardNumber.ToString());
        var handler = OnCardClicked_CM;
        if (handler != null) handler(cardNumber);
    }
    public static void mOnCardSelected_CM(int cardNumber, int toyNumber) {
        Debug.Log("EVENT ---> EM.mOnCardSelected_CM Card="+cardNumber.ToString()+" Toy="+toyNumber.ToString());
        var handler = OnCardSelected_CM;
        if (handler != null) handler(cardNumber, toyNumber);
    }
    public static void mOnInitCardsEnd_CM(int iSetCardsPrefabs, int iSetToysPrefabs, int[] poolToys) {
        Debug.Log("EVENT ---> EM.mOnInitCardsEnd_CM iSetCards=" + iSetCardsPrefabs + " iSetToys=" + iSetToysPrefabs.ToString() + " Toys=" + poolToys);
        var handler = OnInitCardsEnd_CM;
        if (handler != null) handler(iSetCardsPrefabs, iSetToysPrefabs, poolToys);
    }
    public static void mOnEndAnimationMixCards_CM() {
        Debug.Log("EVENT ---> EM.mOnEndAnimationMixCards_CM");
        var handler = OnEndAnimationMixCards_CM;
        if (handler != null) handler();
    }    
    

    // CardsManager Animation Control - public static metods
    public static void mOnStartAnimationOpenCard_CMAnim(int number) {
        Debug.Log("EVENT Anim ---> EM.mOnStartAnimationOpenCard_CMAnim");
        var handler = OnStartAnimationOpenCard_CMAnim;
        if (handler != null) handler(number);
    }
    public static void mOnStartAnimationCloseCard_CMAnim(int number) {
        Debug.Log("EVENT Anim ---> EM.mOnStartAnimationCloseCard_CMAnim");
        var handler = OnStartAnimationCloseCard_CMAnim;
        if (handler != null) handler(number);
    }
    public static void mOnEndAnimationOpenCard_CMAnim(int number) {
        Debug.Log("EVENT Anim ---> EM.mOnEndAnimationOpenCard_CMAnim");
        var handler = OnEndAnimationOpenCard_CMAnim;
        if (handler != null) handler(number);
    }
    public static void mOnEndAnimationCloseCard_CMAnim(int number) {
        Debug.Log("EVENT Anim ---> EM.mOnEndAnimationCloseCard_CMAnim");
        var handler = OnEndAnimationCloseCard_CMAnim;
        if (handler != null) handler(number);
    }
    public static void mOnEndAnimationTrapCard_CMAnim(int number) {
        Debug.Log("EVENT Anim ---> EM.mOnEndAnimationTrapCard_CMAnim Card N="+number);
        var handler = OnEndAnimationTrapCard_CMAnim;
        if (handler != null) handler(number);
    }    




    // --- GAME MANAGER ---
    // GameManager - public static metods
    public static void mOnEnableToOpenCard_GM(int cardNumber) {
        Debug.Log("EVENT ---> EM.mOnEnableToOpenCard_GM = "+cardNumber.ToString());
        var handler = OnEnableToOpenCard_GM;
        if (handler != null) handler(cardNumber);
    }    
    public static void mOnDisableToOpenCard_GM(int cardNumber) {
        Debug.Log("EVENT ---> EM.mOnDisableToOpenCard_GM = "+cardNumber.ToString());
        var handler = OnDisableToOpenCard_GM;
        if (handler != null) handler(cardNumber);
    }        
    public static void mOnCheckKillMonster_GM(int cardNumber, int toyNumber) {
        Debug.Log("EVENT ---> EM.mOnCheckKillMonster_GM Card="+cardNumber+" Toy="+toyNumber.ToString());
        var handler = OnCheckKillMonster_GM;
        if (handler != null) handler(cardNumber, toyNumber);
    }            
    public static void mOnNeedShootTheMonster_GM(int cardNumber, int toyNumber, Transform tMonster) {
        Debug.Log("EVENT ---> EM.mOnNeedShootTheMonster_GM Card="+cardNumber+" Toy="+toyNumber.ToString()+" Monster="+tMonster.gameObject.name);
        var handler = OnNeedShootTheMonster_GM;
        if (handler != null) handler(cardNumber, toyNumber, tMonster);
    }    
    public static void mOnNeedPaintWrongCard_GM(int cardNumber, int toyNumber) {
        Debug.Log("EVENT ---> EM.mOnNeedPaintWrongCard_GM Card="+cardNumber+" Toy="+toyNumber.ToString());
        var handler = OnNeedPaintWrongCard_GM;
        if (handler != null) handler(cardNumber, toyNumber);
    }        
    public static void mOnNeedPaintCorrectCard_GM(int cardNumber, int toyNumber) {
        Debug.Log("EVENT ---> EM.mOnNeedPaintCorrectCard_GM Card="+cardNumber+" Toy="+toyNumber.ToString());
        var handler = OnNeedPaintCorrectCard_GM;
        if (handler != null) handler(cardNumber, toyNumber);
    }            
    public static void mOnMaxSelectedCardsDone_GM() {
        Debug.Log("EVENT ---> EM.mOnMaxSelectedCardsDone_GM");
        var handler = OnMaxSelectedCardsDone_GM;
        if (handler != null) handler();
    }           

    public static void mOnNeedAddMonsterInRoom_GM() {
        Debug.Log("EVENT ---> EM.mOnNeedAddMonsterInRoom_GM");
        var handler = OnNeedAddMonsterInRoom_GM;
        if (handler != null) handler();
    }
    public static void mOnNeedShiftMonstersBack_GM() {
        Debug.Log("EVENT ---> EM.mOnNeedShiftMonstersBack_GM");
        var handler = OnNeedShiftMonstersBack_GM;
        if (handler != null) handler();
    }    
    
    public static void mOnGameLost_GM() {
        Debug.Log("EVENT ---> EM.mOnGameLost_GM");
        var handler = OnGameLost_GM;
        if (handler != null) handler();
    }
    public static void mOnGameWon_GM() {
        Debug.Log("EVENT ---> EM.mOnGameWon_GM");
        var handler = OnGameWon_GM;
        if (handler != null) handler();
    }    
    public static void mOnGamePause_GM(bool state) {
        Debug.Log("EVENT ---> EM.mOnGamePause_GM");
        var handler = OnGamePause_GM;
        if (handler != null) handler(state);
    }        
    public static void mOnNeedPaintErrorIndicator_GM(int nError) {
        Debug.Log("EVENT ---> EM.mOnNeedPaintErrorIndicator_GM = "+nError.ToString());
        var handler = OnNeedPaintErrorIndicator_GM;
        if (handler != null) handler(nError);
    }   
    public static void mOnTrapCardMixSelected_GM(int cardNumber) {
        Debug.Log("EVENT ---> EM.mOnTrapCardMixSelected_GM Card="+cardNumber);
        var handler = OnTrapCardMixSelected_GM;
        if (handler != null) handler(cardNumber);
    }
    public static void mOnTrapCardAddMonsterSelected_GM(int cardNumber) {
        Debug.Log("EVENT ---> EM.mOnTrapCardAddMonsterSelected_GM Card="+cardNumber);
        var handler = OnTrapCardAddMonsterSelected_GM;
        if (handler != null) handler(cardNumber);
    }    
    public static void mOnInitRoom_GM(int maxSelectedCards) {
        Debug.Log("EVENT ---> EM.mOnInitRoom_GM maxSelectedCards="+maxSelectedCards);
        var handler = OnInitRoom_GM;
        if (handler != null) handler(maxSelectedCards);
    }
    public static void mOnInitCards_GM(int iGameDifficulty) {
        Debug.Log("EVENT ---> EM. mOnInitCards_GM iGameDifficulty="+iGameDifficulty);
        var handler =  OnInitCards_GM;
        if (handler != null) handler(iGameDifficulty);
    }    
    public static void mOnInitMonsters_GM(int iGameDifficulty, int iSetToysPrefabs, int[] poolToys) {
        Debug.Log("EVENT ---> EM.mOnInitMonsters_GM iGameDifficulty="+iGameDifficulty+" iSet="+iSetToysPrefabs.ToString()+" Toys="+poolToys);
        var handler = OnInitMonsters_GM;
        if (handler != null) handler(iGameDifficulty, iSetToysPrefabs, poolToys);
    }
    public static void mOnFirstOpenTrapCard_GM(int cardNumber, int toyNumber) {
        Debug.Log("EVENT ---> EM.mOnFirstOpenTrapCard_GM Card="+cardNumber+" Toy="+toyNumber.ToString());
        var handler = OnFirstOpenTrapCard_GM;
        if (handler != null) handler(cardNumber, toyNumber);
    }                
    public static void mOnDoTierActions_GM() {
        Debug.Log("EVENT ---> EM.mOnDoTierActions_GM");
        var handler = OnDoTierActions_GM;
        if (handler != null) handler();
    }
    public static void mOnTierActionsEnd_GM() {
        Debug.Log("EVENT ---> EM.mOnTierActionsEnd_GM");
        var handler = OnTierActionsEnd_GM;
        if (handler != null) handler();
    }    


    // --- MONSTERS MANAGER ---
    // MonstersManager - public static metods
    public static void mOnMonsterKilled_MM() {
        Debug.Log("EVENT ---> EM.mOnMonsterKilled_MM");
        var handler = OnMonsterKilled_MM;
        if (handler != null) handler();
    }
    public static void mOnMonsterNotKilled_MM(int cardNumber, int toyNumber) {
        Debug.Log("EVENT ---> EM.mOnMonsterNotKilled_MM Card="+cardNumber.ToString()+" Toy="+toyNumber.ToString());
        var handler = OnMonsterNotKilled_MM;
        if (handler != null) handler(cardNumber, toyNumber);
    }            
    public static void mOnMonstersShiftedBack_MM() {
        Debug.Log("EVENT ---> EM.mOnMonstersShiftedBack_MM");
        var handler = OnMonstersShiftedBack_MM;
        if (handler != null) handler();
    }
    public static void mOnToyKillSomeMonster_MM(int cardNumber, int toyNumber, Transform tMonster) {
        Debug.Log("EVENT ---> EM.mOnToyKillSomeMonster_MM Card="+cardNumber.ToString()+" Toy="+toyNumber.ToString());
        var handler = OnToyKillSomeMonster_MM;
        if (handler != null) handler(cardNumber, toyNumber, tMonster);
    }
    public static void mOnToyNotKillSomeMonster_MM(int cardNumber, int toyNumber) {
        Debug.Log("EVENT ---> EM.mOnToyNotKillSomeMonster_MM Card="+cardNumber.ToString()+" Toy="+toyNumber.ToString());
        var handler = OnToyNotKillSomeMonster_MM;
        if (handler != null) handler(cardNumber, toyNumber);
    }    
    public static void mOnAddMonsterInRoom_MM() {
        Debug.Log("EVENT ---> EM.mOnAddMonsterInRoom_MM");
        var handler = OnAddMonsterInRoom_MM;
        if (handler != null) handler();
    }    
    public static void mOnInitMonstersEnd_MM(int nMonstersInGame) {
        Debug.Log("EVENT ---> EM.mOnInitMonstersEnd_MM nMonstersInGame="+nMonstersInGame.ToString());
        var handler = OnInitMonstersEnd_MM;
        if (handler != null) handler(nMonstersInGame);
    }    
    

    // MonstersManager Animation Control - public static metods
    public static void mOnStartAnimationDieMonster_MMAnim(GameObject goMonster) {
        Debug.Log("EVENT Anim ---> EM.mOnStartAnimationDieMonster_MMAnim");
        var handler = OnStartAnimationDieMonster_MMAnim;
        if (handler != null) handler(goMonster);
    }
    public static void mOnEndAnimationDieMonster_MMAnim(GameObject goMonster) {
        Debug.Log("EVENT Anim ---> EM.mOnEndAnimationDieMonster_MMAnim");
        var handler = OnEndAnimationDieMonster_MMAnim;
        if (handler != null) handler(goMonster);
    }
    public static void mOnStartAnimationAddMonster_MMAnim(string sGoName) {
        Debug.Log("EVENT Anim ---> EM.mOnStartAnimationAddMonster_MMAnim");
        var handler = OnStartAnimationAddMonster_MMAnim;
        if (handler != null) handler(sGoName);
    }
    public static void mOnEndAnimationAddMonster_MMAnim(string sGoName) {
        Debug.Log("EVENT Anim ---> EM.mOnEndAnimationAddMonster_MMAnim");
        var handler = OnEndAnimationAddMonster_MMAnim;
        if (handler != null) handler(sGoName);
    }

    public static void mOnStartAnimationJumpMonster_MMAnim(string sGoName) {
        Debug.Log("EVENT Anim ---> EM.mOnStartAnimationJumpMonster_MMAnim");
        var handler = OnStartAnimationJumpMonster_MMAnim;
        if (handler != null) handler(sGoName);
    }
    public static void mOnEndAnimationJumpMonster_MMAnim(string sGoName) {
        Debug.Log("EVENT Anim ---> EM.mOnEndAnimationJumpMonster_MMAnim");
        var handler = OnEndAnimationJumpMonster_MMAnim;
        if (handler != null) handler(sGoName);
    }    
    public static void mOnStartAnimationWaitMonster_MMAnim(string sGoName) {
        Debug.Log("EVENT Anim ---> EM.mOnStartAnimationWaitMonster_MMAnim");
        var handler = OnStartAnimationWaitMonster_MMAnim;
        if (handler != null) handler(sGoName);
    }
    public static void mOnEndAnimationWaitMonster_MMAnim(string sGoName) {
        Debug.Log("EVENT Anim ---> EM.mOnEndAnimationWaitMonster_MMAnim");
        var handler = OnEndAnimationWaitMonster_MMAnim;
        if (handler != null) handler(sGoName);
    }    


    // MonsterController - public static metods
    public static void mOnBulletHitMonster_MC () {
        Debug.Log("EVENT ---> EM.mOnBulletHitMonster_MC");
        var handler = OnBulletHitMonster_MC;
        if (handler != null) handler();
    }

    // ToyBulletController - public static metods
    public static void mOnBulletHitMonster_TBC (string targetName) {
        Debug.Log("EVENT ---> EM.mOnBulletHitMonster_TBC");
        var handler = OnBulletHitMonster_TBC;
        if (handler != null) handler(targetName);
    }


}
