using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject goGameOverPage;
    public GameObject goWinPage;
    public GameObject goPausePage;
    public GameObject goInterface;    

    // Game Settings
    //private int nCards=5;
    private int maxSelectedCards=3;
    private int nMonstersOnRound=5;
    private int nMonstersToFailure=3;
    private int iGameDifficulty=10;

    // Game Process
    public bool flagClickableCards = false; // permission to open cards
    public int clickedCards = 0; // User just clicked cards
    public int selectedCards = 0; // Open and give Toy
    public int monstersInRoom = 1;
    public int monstersKilled = 0;
    public bool [] trapFlags;
    public GameObject goProgress;
    
    // Tier Kills info
    public struct KilledInfo {
        public bool isKilled;
        public  int card;
        public  int toy;
        public Transform transform;
    }

    public KilledInfo killedMonsterInThisTier;

    // Start is called before the first frame update
    void Start()    
    {
        //InitRoom();
        //InitMonster();
        //InitCards();

        iGameDifficulty = GameDirectorController.curDifficulty;
        FirebaseManagerController.FBA_EventScreenView("Game_Screen");
        FirebaseManagerController.FBA_EventLevelStart("Room_00", iGameDifficulty);

        Debug.Log("Game Manager ++++++ iGameDifficulty = " + iGameDifficulty);

        InitGameSettings(iGameDifficulty);
        InitGameProcess();

        goGameOverPage.SetActive(false);
        goWinPage.SetActive(false);
        goPausePage.SetActive(false);
        goInterface.SetActive(false);

        EventManager.mOnInitRoom_GM(maxSelectedCards);
    }

    private void OnEnable() {
        EventManager.OnInitRoomEnd_RM += OnInitRoomEnd_RM;

        EventManager.OnCardClicked_CM += OnCardClicked_CM;
        EventManager.OnCardSelected_CM += OnCardSelected_CM;
        EventManager.OnInitCardsEnd_CM += OnInitCardsEnd_CM;
        EventManager.OnEndAnimationMixCards_CM += OnEndAnimationMixCards_CM;

        EventManager.OnInitMonstersEnd_MM += OnInitMonstersEnd_MM;
        EventManager.OnMonsterKilled_MM += OnMonsterKilled_MM;
        //EventManager.OnMonsterNotKilled_MM += OnMonsterNotKilled_MM;
        EventManager.OnToyKillSomeMonster_MM += OnToyKillSomeMonster_MM;
        EventManager.OnToyNotKillSomeMonster_MM += OnToyNotKillSomeMonster_MM;
        EventManager.OnMonstersShiftedBack_MM += OnMonstersShiftedBack_MM;

        EventManager.OnStartAnimationAddMonster_MMAnim += OnStartAnimationAddMonster_MMAnim;
        EventManager.OnEndAnimationAddMonster_MMAnim += OnEndAnimationAddMonster_MMAnim;
        EventManager.OnStartAnimationDieMonster_MMAnim += OnStartAnimationDieMonster_MMAnim;
        EventManager.OnEndAnimationDieMonster_MMAnim += OnEndAnimationDieMonster_MMAnim;    

    }

    private void OnDisable() {
        EventManager.OnInitRoomEnd_RM -= OnInitRoomEnd_RM;

        EventManager.OnCardClicked_CM -= OnCardClicked_CM;        
        EventManager.OnCardSelected_CM -= OnCardSelected_CM;
        EventManager.OnInitCardsEnd_CM -= OnInitCardsEnd_CM;
        EventManager.OnEndAnimationMixCards_CM += OnEndAnimationMixCards_CM;        

        EventManager.OnInitMonstersEnd_MM -= OnInitMonstersEnd_MM;
        EventManager.OnMonsterKilled_MM -= OnMonsterKilled_MM;
        //EventManager.OnMonsterNotKilled_MM -= OnMonsterNotKilled_MM;        
        EventManager.OnToyKillSomeMonster_MM -= OnToyKillSomeMonster_MM;
        EventManager.OnToyNotKillSomeMonster_MM -= OnToyNotKillSomeMonster_MM;        
        EventManager.OnMonstersShiftedBack_MM -= OnMonstersShiftedBack_MM;

        EventManager.OnStartAnimationAddMonster_MMAnim -= OnStartAnimationAddMonster_MMAnim;
        EventManager.OnEndAnimationAddMonster_MMAnim -= OnEndAnimationAddMonster_MMAnim;
        EventManager.OnStartAnimationDieMonster_MMAnim -= OnStartAnimationDieMonster_MMAnim;
        EventManager.OnEndAnimationDieMonster_MMAnim -= OnEndAnimationDieMonster_MMAnim;        

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitGameSettings(int iGameDifficulty){
        //Debug.Log("===> "+ iGameDifficulty / 10);
        switch (iGameDifficulty)
        {
            case 0:     // train lvl 3 Card
            case 1: 
            case 2: 
            case 3: 
                nMonstersOnRound = 3;
                maxSelectedCards=2;
                break;
            case 4:     // train lvl 5 Card
            case 5: 
            case 6: 
            case 7: 
            case 8: 
            case 9: 
                nMonstersOnRound = 4;
                maxSelectedCards=3;
                break;                
            case 10:    // iGameDifficulty 10...19 - 5 Card
            case 11:
            case 12:
            case 13:
            case 14:
            case 15:
            case 16:
            case 17:
            case 18:
            case 19:
                nMonstersOnRound = 5;
                maxSelectedCards=3;
                break;
            case 20:    // iGameDifficulty 20...29 - 7 Card
            case 21:
            case 22:
            case 23:
            case 24:
            case 25:
            case 26:
            case 27:
            case 28:
            case 29:
                nMonstersOnRound = 7;
                maxSelectedCards=3;
                break;
            default:
                nMonstersOnRound = 7;
                maxSelectedCards=3;
                break;
        }
        //nMonstersOnRound = 10; // Debug line
        //maxSelectedCards=3; // Debug line
    }

    private void InitGameProcess(){
        trapFlags = new bool[2];
        trapFlags[0] = false;
        trapFlags[1] = false;

        killedMonsterInThisTier.isKilled = false;
        SetTextProgress(monstersKilled, nMonstersOnRound); 
    }

    private void OnInitMonstersEnd_MM(int nMonstersInGame){
        nMonstersToFailure = nMonstersInGame;
    }


    private void OnInitCardsEnd_CM (int iSetCardsPrefabs, int iSetToysPrefabs, int[] poolToys){
        EventManager.mOnInitMonsters_GM(iGameDifficulty, iSetToysPrefabs, poolToys);
    }

    private void OnInitRoomEnd_RM() {
        goInterface.SetActive(true);
        EventManager.mOnInitCards_GM(iGameDifficulty);
    }

    public void OnCardClicked_CM (int numberCard) {
        if (flagClickableCards && (clickedCards < maxSelectedCards)) {
            clickedCards++;
            EventManager.mOnEnableToOpenCard_GM(numberCard);
        } else {
            EventManager.mOnDisableToOpenCard_GM(numberCard);
        }
    }

    public void OnCardSelected_CM (int numberCard, int toyCard) {
        selectedCards++;        
        switch (toyCard)
        {
            case 50:
                // MIX Cards
                Debug.Log("MIX Cards!");
                if (trapFlags[0]) {
                    trapFlags[0] = false;
                    trapFlags[1] = false;
                    if (!killedMonsterInThisTier.isKilled){
                        EventManager.mOnNeedPaintWrongCard_GM(numberCard, toyCard);
                        killedMonsterInThisTier.isKilled = true;
                        killedMonsterInThisTier.card = numberCard;
                        killedMonsterInThisTier.toy = toyCard;
                    }                            
                    if (selectedCards == clickedCards) {
                        DoTierAction();
                    } else {                
                        //Debug.Log("GM - Wait for open all clicked cards!");
                    }                          
                    
                    //EventManager.mOnCheckKillMonster_GM(numberCard, -1);                    
                } else {
                    Debug.Log("TrapFlag[0] = true");
                    trapFlags[0] = true;
                    EventManager.mOnCheckKillMonster_GM(numberCard, -1);
                    EventManager.mOnFirstOpenTrapCard_GM(numberCard, toyCard);
                }
                break;

            case 51:
                // ADD Monster
                Debug.Log("ADD Monster!");
                if (trapFlags[1]){
                    //trapFlags[1] = false;
                    if (!killedMonsterInThisTier.isKilled){
                        EventManager.mOnNeedPaintWrongCard_GM(numberCard, toyCard);
                        killedMonsterInThisTier.isKilled = true;
                        killedMonsterInThisTier.card = numberCard;
                        killedMonsterInThisTier.toy = toyCard;
                    }                            
                    if (selectedCards == clickedCards) {
                        DoTierAction();
                    } else {                
                        //Debug.Log("GM - Wait for open all clicked cards!");
                    }                                        
                } else {
                    Debug.Log("TrapFlag[1] = true");
                    trapFlags[1] = true;
                    EventManager.mOnCheckKillMonster_GM(numberCard, -1);
                    EventManager.mOnFirstOpenTrapCard_GM(numberCard, toyCard);
                }
                break;                           
            
            default:
                if (!killedMonsterInThisTier.isKilled) {
                    EventManager.mOnCheckKillMonster_GM(numberCard, toyCard);
                } else {
                    EventManager.mOnCheckKillMonster_GM(numberCard, -1);
                }
                break;
        }
    }

    public void OnToyKillSomeMonster_MM(int cardNumber, int toyNumber, Transform tMonster) {
        if (!killedMonsterInThisTier.isKilled){
            EventManager.mOnNeedPaintCorrectCard_GM(cardNumber, toyNumber);
            SetTextProgress(monstersKilled+1, nMonstersOnRound);
            killedMonsterInThisTier.isKilled = true;
            killedMonsterInThisTier.card = cardNumber;
            killedMonsterInThisTier.toy = toyNumber;
            killedMonsterInThisTier.transform = tMonster;            
        }
        if (selectedCards == clickedCards) {
            DoTierAction();
        } else {                
            //Debug.Log("GM - Wait for open all clicked cards!");
        }
    }

    public void OnToyNotKillSomeMonster_MM(int cardNumber, int toyNumber) {
        EventManager.mOnNeedPaintWrongCard_GM(cardNumber, toyNumber);
        if (!killedMonsterInThisTier.isKilled) {
            EventManager.mOnNeedPaintErrorIndicator_GM(selectedCards);
        }
        //Debug.Log("selectedCards "+selectedCards+" == "+maxSelectedCards+" maxSelectedCards");        
        if (selectedCards == clickedCards) {
            if (killedMonsterInThisTier.isKilled){
                DoTierAction();
            } else {
                if (selectedCards == maxSelectedCards) {
                    //Close all selected card
                    EventManager.mOnMaxSelectedCardsDone_GM();
                    EventManager.mOnDoTierActions_GM();
                    ResetTierVariable();
                    //AddMonster
                    if (monstersInRoom < nMonstersToFailure) {
                        monstersInRoom ++;
                        EventManager.mOnNeedAddMonsterInRoom_GM();
                    } else {
                        Debug.Log("GAME OVER!!!");
                        goGameOverPage.SetActive(true);
                        EventManager.mOnGameLost_GM();
                    }                        
                } else {
                    Debug.Log("Wait next cklicked Card!");
                }
            }
        } else {
            //Debug.Log("GM - Wait for open all clicked cards!");
        }

    }

    private void OnMonsterKilled_MM(){                 
        EventManager.mOnNeedPaintErrorIndicator_GM(selectedCards);        
        ResetTierVariable();
        monstersInRoom--;
        monstersKilled++;        
        FirebaseManagerController.FBA_EventMonsterDie(monstersKilled, nMonstersOnRound);        
        //Debug.Log("monstersKilled "+monstersKilled+" == "+nMonstersOnRound+" nMonstersOnRound");
        if (monstersKilled == nMonstersOnRound) {
            Debug.Log("All monsters ADDed from ClotheBoard!");
            Debug.Log("Congretulation!!!");                
            EventManager.mOnGameWon_GM();
            goWinPage.SetActive(true);
        } else {
            Debug.Log("Need Shift mobs back!!!");
            EventManager.mOnNeedShiftMonstersBack_GM();        
        }        
    }

    private void OnTakeFearItem(int iFearItem){
        //OnCardOpen(iFearItem); // Send to MonstersManager number FearItem to try kill some monster
    }

    private void OnMonstersShiftedBack_MM() {
        selectedCards = 0;
        if (monstersInRoom == 0){
            EventManager.mOnNeedAddMonsterInRoom_GM();
            monstersInRoom++;
        } else {
            EventManager.mOnTierActionsEnd_GM();
        }
    }

    private void ResetTierVariable(){
        clickedCards=0;
        selectedCards = 0;
        killedMonsterInThisTier.isKilled = false;        
    }
    
    private void DoTierAction() {
        flagClickableCards = false;
        EventManager.mOnDoTierActions_GM();        
        if (killedMonsterInThisTier.toy < 50){
            EventManager.mOnNeedShootTheMonster_GM(killedMonsterInThisTier.card, killedMonsterInThisTier.toy, killedMonsterInThisTier.transform);
            EventManager.mOnMaxSelectedCardsDone_GM();
        } else {
            // Trap Cards
            switch (killedMonsterInThisTier.toy){
                case 50:
                    EventManager.mOnTrapCardMixSelected_GM(killedMonsterInThisTier.card);
                    ResetTierVariable();
                    break;
                case 51:
                    EventManager.mOnTrapCardAddMonsterSelected_GM(killedMonsterInThisTier.card);
                    if (monstersInRoom < nMonstersToFailure) {
                        monstersInRoom ++;
                        EventManager.mOnNeedAddMonsterInRoom_GM();
                    } else {
                        Debug.Log("GAME OVER!!!");
                        goGameOverPage.SetActive(true);
                        EventManager.mOnGameLost_GM();
                    }                                        
                    ResetTierVariable();
                    break;                        
            }                    
        }        
    }

    private void OnStartAnimationAddMonster_MMAnim(string sGoName){
        flagClickableCards = false;
        FirebaseManagerController.FBA_EventMonsterAdd();
    }

    private void OnEndAnimationAddMonster_MMAnim(string sGoName){
        flagClickableCards = true;
        EventManager.mOnTierActionsEnd_GM();
    }
    private void OnStartAnimationDieMonster_MMAnim(GameObject goMonster){
        flagClickableCards = false;
    }
    private void OnEndAnimationDieMonster_MMAnim(GameObject goMonster){
        flagClickableCards = true;
    }    

    public void PauseGame (bool state){
        goPausePage.SetActive(state);
        EventManager.mOnGamePause_GM(state);
    }

    private void SetTextProgress(int nKiled, int nMonsters){
        Text _t = goProgress.GetComponent<Text>();
        GameObject goEffect = goProgress.transform.GetChild(0).gameObject;

        _t.text = "Scared: " + nKiled + " of " + nMonsters;
        if (nKiled !=0){
            goEffect.SetActive(true);
        }
    }

    private void OnEndAnimationMixCards_CM (){
        flagClickableCards = true;
        EventManager.mOnTierActionsEnd_GM();
    }

}
