using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsManager : MonoBehaviour
{    
    public GameObject[] goCardPrefabs;
    public GameObject[] goSetToysPrefabs;
    public GameObject[] goSetTrapPrefabs;
    public GameObject[] goToyBulletPrefabs;

    public Vector3 extremeRightPos;
    public Vector3 extremeLeftPos;
    public int nCardsInLevel;
    public int nOpenCardsInLevel;

    public AudioClip[] clipsCardUp;
    public AudioClip[] clipsCardDown;
    private string flagsOpenCardsInLevel = "";
    private string flagsTrapCardsInLevel = "";

    public Card[] poolCards;

    public class Card {
        public GameObject go;
        Animator animator;
        AudioSource audioSource;
        public int number;
        public int toy;
        bool isOpen;
        bool isDefaultOpen;
        bool isClicked;
        bool isMoving;

        public Card (GameObject newGo, Animator newAnimator, AudioSource newAudioSource, int newNumber, int newToy, bool newIsOpen, bool newIsDefaultOpen, bool newIsClicked, bool newIsMoving){
            go = newGo;
            animator = newAnimator;
            audioSource = newAudioSource;
            number = newNumber;
            toy = newToy;
            isOpen = newIsOpen;
            isDefaultOpen = newIsDefaultOpen;
            isClicked = newIsClicked;
            isMoving = newIsMoving;
        }
        public void Open (){
            animator.SetBool("isOpenCard", true);
        }
        public void Close (){            
            animator.SetBool("isOpenCard", false);
            animator.SetBool("isTrapCard", false);
        }     

        public void CardKillMonster (){
            animator.SetBool("isCorrectCard", true);
        }
        public void CardNotKillMonster (){
            animator.SetBool("isCorrectCard", false);
        }

        public void TrapCardOpen (){
            animator.SetBool("isTrapCard", true);
        }

        public void InitGrate(){
            if (toy < 50){
                // Disable GRATE to toy card
                go.transform.GetChild(0).GetChild(1).GetChild(0).gameObject.SetActive(false);   // BackSide - Open - Grate
                go.transform.GetChild(1).GetChild(0).GetChild(0).gameObject.SetActive(false);   // FrontSide - Front - Grate
            } else {
                // Activate GRATE to Trap card
                go.transform.GetChild(0).GetChild(1).GetChild(0).gameObject.SetActive(true);    // BackSide - Open - Grate
                go.transform.GetChild(1).GetChild(0).GetChild(0).gameObject.SetActive(true);    // FrontSide - Front - Grate
            }
        }
        public void DisableGrate(){
            go.transform.GetChild(0).GetChild(1).GetChild(0).gameObject.SetActive(false);   // BackSide - Open - Grate
            go.transform.GetChild(1).GetChild(0).GetChild(0).gameObject.SetActive(false);   // FrontSide - Front - Grate
        }
        public void ActiveGrate(){
            go.transform.GetChild(0).GetChild(1).GetChild(0).gameObject.SetActive(true);    // BackSide - Open - Grate
            go.transform.GetChild(1).GetChild(0).GetChild(0).gameObject.SetActive(true);    // FrontSide - Front - Grate
        }

        public void DisableBlockRotationIcon(){
            go.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(false); // BackSide - Close - BlockRotationIcon
            go.transform.GetChild(0).GetChild(1).GetChild(1).gameObject.SetActive(false); // BackSide - Open - BlockRotationIcon
            go.transform.GetChild(1).GetChild(0).GetChild(1).gameObject.SetActive(false); // FrontSide - Front - BlockRotationIcon            
        }
        public void ActivateBlockRotationIcon(){
            go.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(true); // BackSide - Close - BlockRotationIcon
            go.transform.GetChild(0).GetChild(1).GetChild(1).gameObject.SetActive(true); // BackSide - Open - BlockRotationIcon
            go.transform.GetChild(1).GetChild(0).GetChild(1).gameObject.SetActive(true); // FrontSide - Front - BlockRotationIcon                     
        }        

        public bool IsOpen {
            get { return isOpen; }
            set {isOpen = value; }
        }   
        public bool IsDefaultOpen {
            get { return isDefaultOpen; }
            set {isDefaultOpen = value; }
        }           
        public bool IsClicked {
            get { return isClicked; }
            set {isClicked = value;}
        }
        public bool IsMoving {
            get { return isMoving; }
            set {isMoving = value;}
        } 

        public void PlayRandomClipOnce(AudioClip[] clips){
            audioSource.PlayOneShot(clips[UnityEngine.Random.Range(0, clips.Length)]);
        }

    }

    private int iCardPrefabs = 0;
    private int iSetToysPrefabs = 0;

    private Vector3 cardStepRange;

    // Start is called before the first frame update
    void Start()
    {
        //InitCards(nCardsInLevel);
    }

    private void OnEnable() {
        EventManager.OnEnableToOpenCard_GM += OnEnableToOpenCard_GM;
        EventManager.OnDisableToOpenCard_GM += OnDisableToOpenCard_GM;
        EventManager.OnMaxSelectedCardsDone_GM += OnMaxSelectedCardsDone_GM;
        EventManager.OnNeedShootTheMonster_GM += OnNeedShootTheMonster_GM;
        EventManager.OnNeedPaintWrongCard_GM += OnNeedPaintWrongCard_GM;
        EventManager.OnNeedPaintCorrectCard_GM += OnNeedPaintCorrectCard_GM;
        EventManager.OnTrapCardMixSelected_GM += OnTrapCardMixSelected_GM;
        EventManager.OnTrapCardAddMonsterSelected_GM += OnTrapCardAddMonsterSelected_GM;        
        EventManager.OnInitCards_GM += OnInitCards_GM;
        EventManager.OnFirstOpenTrapCard_GM += OnFirstOpenTrapCard_GM;
        EventManager.OnDoTierActions_GM += OnDoTierActions_GM;
        EventManager.OnTierActionsEnd_GM += OnTierActionsEnd_GM;

    
        EventManager.OnStartAnimationOpenCard_CMAnim += OnStartAnimationOpenCard_CMAnim;  
        EventManager.OnEndAnimationOpenCard_CMAnim += OnEndAnimationOpenCard_CMAnim;  
        EventManager.OnStartAnimationCloseCard_CMAnim += OnStartAnimationCloseCard_CMAnim;  
        EventManager.OnEndAnimationCloseCard_CMAnim += OnEndAnimationCloseCard_CMAnim;          
        EventManager.OnEndAnimationTrapCard_CMAnim += OnEndAnimationTrapCard_CMAnim;
    }

    private void OnDisable() {
        EventManager.OnEnableToOpenCard_GM -= OnEnableToOpenCard_GM;
        EventManager.OnDisableToOpenCard_GM -= OnDisableToOpenCard_GM;
        EventManager.OnMaxSelectedCardsDone_GM -= OnMaxSelectedCardsDone_GM;
        EventManager.OnNeedShootTheMonster_GM -= OnNeedShootTheMonster_GM;
        EventManager.OnNeedPaintWrongCard_GM -= OnNeedPaintWrongCard_GM;
        EventManager.OnNeedPaintCorrectCard_GM -= OnNeedPaintCorrectCard_GM;
        EventManager.OnTrapCardMixSelected_GM -= OnTrapCardMixSelected_GM;
        EventManager.OnTrapCardAddMonsterSelected_GM -= OnTrapCardAddMonsterSelected_GM;
        EventManager.OnInitCards_GM -= OnInitCards_GM;
        EventManager.OnFirstOpenTrapCard_GM -= OnFirstOpenTrapCard_GM;
        EventManager.OnDoTierActions_GM -= OnDoTierActions_GM;
        EventManager.OnTierActionsEnd_GM -= OnTierActionsEnd_GM;

        EventManager.OnStartAnimationOpenCard_CMAnim -= OnStartAnimationOpenCard_CMAnim;
        EventManager.OnEndAnimationOpenCard_CMAnim -= OnEndAnimationOpenCard_CMAnim;  
        EventManager.OnStartAnimationCloseCard_CMAnim -= OnStartAnimationCloseCard_CMAnim;  
        EventManager.OnEndAnimationCloseCard_CMAnim -= OnEndAnimationCloseCard_CMAnim;  
        EventManager.OnEndAnimationTrapCard_CMAnim -= OnEndAnimationTrapCard_CMAnim;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnInitCards_GM(int iGameDifficulty) {
        Debug.Log("OnInitCards_GM = " + iGameDifficulty);
        //iGameDifficulty = 100; //debug
        //Debug.Log("---------------------------------- Inin CARDS!");
        SetVarFromDifficulty(iGameDifficulty);
        StartCoroutine(InitCards (nCardsInLevel, nOpenCardsInLevel));
    }

    private IEnumerator InitCards (int nCards, int nOpenCards) {

        iCardPrefabs = UnityEngine.Random.Range(0, goCardPrefabs.Length);
        iSetToysPrefabs = UnityEngine.Random.Range(0, goSetToysPrefabs.Length);
        //iSetToysPrefabs = 1; // debug

        int[] randomToyIndexArr = new int[nCards];
        int n=0;
        int newToyIndex = 0;
        bool flagNoRepeat = true;

        cardStepRange = (extremeRightPos - extremeLeftPos) / (nCards-1);

        //Debug.Log("cardStepRange = "+cardStepRange.ToString());

        // sorted array
        /*
        for (int i = 0; i < randomToyIndexArr.Length; i++) {
            randomToyIndexArr[i]=i;
        }
        */

        // generate Array without repeats
        //Debug.Log("N in Toys = "+goSetToysPrefabs[iSetToysPrefabs].transform.childCount);        

        for (int i = 0; i < randomToyIndexArr.Length; i++) {            
            do {
                flagNoRepeat = true;
                newToyIndex = UnityEngine.Random.Range(0, goSetToysPrefabs[iSetToysPrefabs].transform.childCount);                
                for (int j = 0; j < n; j++) {
                    //Debug.Log(">>> arr["+j+"] ="+randomToyIndexArr[j] +" == "+ newToyIndex);
                    if (randomToyIndexArr[j] == newToyIndex) {
                        flagNoRepeat = false;
                        break;
                    }
                }
            } while (!flagNoRepeat);    
            randomToyIndexArr[n] = newToyIndex;
            //Debug.Log("randomToyIndexArr["+n+"]="+randomToyIndexArr[n]);
            n++;                        
        }        

        // Trap Toy index generator zone (50 - mix card, 51 - add monster)
        switch (flagsTrapCardsInLevel) {
            case "00":
                break;
            case "10":
                randomToyIndexArr[nCards-1] = 50;
                break;                
            case "01":
                randomToyIndexArr[nCards-1] = 51;            
                break;
            case "11":
                randomToyIndexArr[nCards-1] = 50;
                randomToyIndexArr[nCards-2] = 51;
                break;                                                
            default:
                randomToyIndexArr[nCards-1] = 50;
                randomToyIndexArr[nCards-2] = 51;
                break;
        }
        
        // Random Shake Array
        for (int i = 0; i < randomToyIndexArr.Length; i++) {
            int randomSwapPozition = UnityEngine.Random.Range(0, randomToyIndexArr.Length);
            int buf = randomToyIndexArr[i];
            randomToyIndexArr[i] = randomToyIndexArr[randomSwapPozition];
            randomToyIndexArr[randomSwapPozition]=buf;
        }        

        poolCards = new Card[nCards];
        for (int i = 0; i < nCards; i++) {            

            int number = i;
            int toy = randomToyIndexArr[i];            
            bool isOpen = false;
            bool isDefaultOpen = false;            
            if (flagsOpenCardsInLevel[i] == '1'){
                isDefaultOpen = true;
            }
            bool isClicked = false;
            bool isMoving = false;

            GameObject go = Instantiate (goCardPrefabs[iCardPrefabs]) as GameObject;
            Transform t = go.transform;
            t.SetParent (transform);
            //t.localPosition = extremeLeftPos + (cardStepRange * i); //new Vector3(-4.0f+1.75f*i, -4.2f, 0);
            t.localPosition = Vector3.zero;
            t.name = "Card_"+i.ToString();

            Animator animator = go.GetComponent<Animator>();
            AudioSource audioSource = go.GetComponent<AudioSource>();
            CardController cc = go.GetComponent<CardController>();            
            cc.number = i;

            Transform tBackSide = t.GetChild(0);
            GameObject goToy;
            if (toy < 50) {
                goToy = Instantiate (goSetToysPrefabs[iSetToysPrefabs].transform.GetChild(toy).gameObject) as GameObject;                
            } else {
                goToy = Instantiate (goSetTrapPrefabs[iSetToysPrefabs].transform.GetChild(toy-50).gameObject) as GameObject;
            }
            Transform tToy = goToy.transform;
            tToy.SetParent (tBackSide, false);

            Transform tFrontSide = t.GetChild(1);
            goToy = Instantiate (goToy) as GameObject;
            tToy = goToy.transform;
            tToy.SetParent (tFrontSide, false);

            poolCards[i] = new Card(go, animator, audioSource, number, toy, isOpen, isDefaultOpen, isClicked, isMoving);

            poolCards[i].InitGrate();
            poolCards[i].ActivateBlockRotationIcon();

            if (poolCards[i].IsDefaultOpen){
                tBackSide.GetChild(0).gameObject.SetActive(false);
            } else {
                tBackSide.GetChild(1).gameObject.SetActive(false);
            }   
        }

        for (int i = 0; i < poolCards.Length; i++) {
            //Debug.Log("START GO BOT");
            StartCoroutine(LinearMoveCoroutine(poolCards[i], poolCards[i].go.transform.localPosition, extremeLeftPos + (cardStepRange * i), 1));
            yield return new WaitForSeconds(UnityEngine.Random.Range(0.005f, 0.01f));
        }      

        while (AreCardsMoving()){
            //Debug.Log("Waiting Card go BOT...");
            yield return new WaitForSeconds(0.2f);
            //yield return new WaitForEndOfFrame();
        }          
        EventManager.mOnInitCardsEnd_CM(iCardPrefabs, iSetToysPrefabs, GetPoolToys());
    }

    public void CardClicked(int cardNumber){        
        if (poolCards[cardNumber].IsOpen) {
            poolCards[cardNumber].IsClicked = false;
        } else {
            poolCards[cardNumber].IsClicked = true;
            EventManager.mOnCardClicked_CM(cardNumber);
        }
    }

    private void OnEnableToOpenCard_GM(int cardNumber){      
        poolCards[cardNumber].Open();        
    }    

    private void OnStartAnimationOpenCard_CMAnim(int cardNumber) {
        poolCards[cardNumber].IsMoving = true;
        poolCards[cardNumber].IsOpen = true;
    }
    private void OnEndAnimationOpenCard_CMAnim(int cardNumber) {        
        poolCards[cardNumber].IsMoving = false;
        poolCards[cardNumber].IsClicked = false;
        EventManager.mOnCardSelected_CM(poolCards[cardNumber].number, poolCards[cardNumber].toy);
            
    }    

    private void OnStartAnimationCloseCard_CMAnim(int cardNumber) {
        poolCards[cardNumber].IsMoving = true;
        //poolCards[cardNumber].IsOpen = false;
    }    
    private void OnEndAnimationCloseCard_CMAnim(int cardNumber) {
        poolCards[cardNumber].IsMoving = false;
        poolCards[cardNumber].IsOpen = false;
        /*
        if (IsAllCardsClose()){
            UnblockClickingCards();
        }
        */
    }        

    private void OnDisableToOpenCard_GM(int cardNumber){
        //Debug.Log("Animation disable to open Card!");                
    }        

    private void OnNeedShootTheMonster_GM(int numberCard, int toyNumber, Transform killedMonster){
        //Debug.Log("Some monster was KILLED!!!");
        poolCards[numberCard].CardKillMonster();
        GenerateToyBullet(numberCard, toyNumber, killedMonster);
    }

    private void OnNeedPaintWrongCard_GM(int numberCard, int toyNumber){
        //Debug.Log("No one KILL!!!");
        poolCards[numberCard].CardNotKillMonster();
    }

    private void OnNeedPaintCorrectCard_GM(int numberCard, int toyNumber){        
        poolCards[numberCard].CardKillMonster();
    }    

    private void OnMaxSelectedCardsDone_GM(){
        //close all opened cards
        StartCoroutine(CloseAllCards());
    }
    
    IEnumerator CloseAllCards() {
        BlockClickingCards();
        for (int i = 0; i < poolCards.Length; i++) {
            if (poolCards[i].IsOpen) {
                poolCards[i].Close();
            }
        }
        while (!IsAllCardsClose()){
            //Debug.Log("Waiting before cloce cards...");
            //yield return new WaitForSeconds(0.2f);
            yield return new WaitForEndOfFrame();
        }    
        UnblockClickingCards();
    }

    private void GenerateToyBullet (int numberCard, int toyNumber, Transform target) {
        Transform parentBullet = poolCards[numberCard].go.transform;
        GameObject go = Instantiate (goToyBulletPrefabs[iSetToysPrefabs]) as GameObject;
        Transform t = go.transform;
        t.SetParent (parentBullet, false);
        t.name = "ToyBullet";
        ToyBulletController tbc = go.GetComponent<ToyBulletController>();
        tbc.goTarget = target.gameObject;
        tbc.numToy = toyNumber;
    }

    private void BlockClickingCards(){
        for (int i = 0; i < poolCards.Length; i++) {
            DisableCardCollider(i);
        }
    }
    private void UnblockClickingCards(){
        for (int i = 0; i < poolCards.Length; i++) {
            EnableCardCollider(i);
        }
    }    
    private void EnableCardCollider(int numCard){
        poolCards[numCard].go.GetComponent<BoxCollider2D>().enabled = true;
    }    

    private void DisableCardCollider(int numCard){
        poolCards[numCard].go.GetComponent<BoxCollider2D>().enabled = false;
    }        

    private bool IsAllCardsClose() {
        bool flag = true;
        for (int i = 0; i < poolCards.Length; i++) {
            if (poolCards[i].IsOpen){
                flag = false;
                break;
            }
        }
        return flag;
    }

    private void OnTrapCardMixSelected_GM (int numCard) {
        StartCoroutine(CardsMix());
    }
    private void OnTrapCardAddMonsterSelected_GM (int numCard) {
        StartCoroutine(CloseAllCards());
    }    

    IEnumerator CardsMix() {        
        BlockClickingCards();
        //SetBlockRotationIconAllCards(false);

        // Close al open cards
        for (int i = 0; i < poolCards.Length; i++) {
            if (poolCards[i].IsOpen) {
                poolCards[i].Close();
            }
        }
        
        // Card Go Center
        for (int i = 0; i < poolCards.Length; i++) {
            //Debug.Log("START GO CENTER");
            StartCoroutine(LinearMoveCoroutine(poolCards[i], poolCards[i].go.transform.localPosition, new Vector3(0,0,0), 1.0f));
            yield return new WaitForSeconds(0.2f);
        }                
        
        // Card MIXing
        //Debug.Log("Waiting...");
        while (AreCardsMoving()){
            //Debug.Log("Waiting before MIX...");
            yield return new WaitForSeconds(0.2f);
            //yield return new WaitForEndOfFrame();
        }

        for (int i = 0; i < poolCards.Length; i++) {
            Card bufCard;
            CardController cc;
            int randI = UnityEngine.Random.Range(0, poolCards.Length);

            bufCard = poolCards[i];
            bufCard.number = randI;
            cc = bufCard.go.GetComponent<CardController>();   // Change number CardController for send correct number after Click;            
            cc.number = randI;

            poolCards[i] = poolCards[randI];
            poolCards[i].number = i;
            cc = poolCards[i].go.GetComponent<CardController>(); // Change number CardController for send correct number after Click;
            cc.number = i;

            poolCards[randI] = bufCard;
        }

        float shiftAngle = 0;
        float shiftRadius = 0;
        Vector3[] shiftVector = new Vector3[poolCards.Length];
        float shiftX = 0;
        float shiftY = 0;

        for (int i = 0; i < poolCards.Length; i++) {
            //Debug.Log("START MIX");
            shiftAngle = DegreesInRadian(UnityEngine.Random.Range(0.0f, 360.0f));
            shiftRadius = UnityEngine.Random.Range(2.0f, 3.0f);            
            shiftX = shiftRadius * Mathf.Cos(shiftAngle);
            shiftY = shiftRadius * Mathf.Sin(shiftAngle);
            shiftVector[i] = new Vector3(shiftX, shiftY, 0);
            StartCoroutine(LinearMoveCoroutine(poolCards[i], poolCards[i].go.transform.localPosition, shiftVector[i], 0.5f));
            yield return new WaitForEndOfFrame();
        }

        //Debug.Log("Waiting...");
        while (AreCardsMoving()){
            //Debug.Log("Waiting before ...");
            //yield return new WaitForSeconds(0.2f);
            yield return new WaitForEndOfFrame();
        }

        for (int i = 0; i < poolCards.Length; i++) {
            StartCoroutine(LinearMoveCoroutine(poolCards[i], poolCards[i].go.transform.localPosition, shiftVector[i] * -1.0f, 1f));
            yield return new WaitForEndOfFrame();
        }

        // Card GO Center
        //Debug.Log("Waiting...");
        while (AreCardsMoving()){
            //Debug.Log("Waiting...");
            yield return new WaitForEndOfFrame();
        }

        for (int i = 0; i < poolCards.Length; i++) {
            //Debug.Log("START GO CENTER");
            StartCoroutine(LinearMoveCoroutine(poolCards[i], poolCards[i].go.transform.localPosition, new Vector3(0,0,0), 0.5f));
            yield return new WaitForEndOfFrame();
        }

        // Cards GO BOT
        //Debug.Log("Waiting...");
        while (AreCardsMoving()){
            //Debug.Log("Waiting...");
            //yield return new WaitForSeconds(0.2f);
            yield return new WaitForEndOfFrame();
        }

        for (int i = 0; i < poolCards.Length; i++) {
            if (poolCards[i].toy >= 50){
                poolCards[i].ActiveGrate();
            }
            //Debug.Log("START GO BOT");
            StartCoroutine(LinearMoveCoroutine(poolCards[i], poolCards[i].go.transform.localPosition, extremeLeftPos + (cardStepRange * i), 1));
            yield return new WaitForSeconds(UnityEngine.Random.Range(0.005f, 0.01f));
        }

        //Debug.Log("Waiting...");
        while (AreCardsMoving()){
            //Debug.Log("Waiting...");
            //yield return new WaitForSeconds(0.2f);
            yield return new WaitForEndOfFrame();
        }
        //SetBlockRotationIconAllCards(true);        
        EventManager.mOnEndAnimationMixCards_CM();                
        UnblockClickingCards();        
    }

    IEnumerator LinearMoveCoroutine(Card card, Vector3 startVector, Vector3 finishVector, float animTime){                
        Transform t = card.go.transform;
        bool flagMovingDirection = false; // false - Move UP, true - Move Dovn
        if (startVector.y > finishVector.y){
            flagMovingDirection = true;
        }
        if (flagMovingDirection) {
            card.PlayRandomClipOnce(clipsCardUp);
        } else { 
            card.PlayRandomClipOnce(clipsCardDown);
        }
        card.IsMoving = true;
        float time = 0;
        while (time < animTime){            
            t.localPosition = Vector3.Lerp (startVector, finishVector, time / animTime);            
            //Debug.Log("y = "+t.localPosition.y+" ( % = "+ time / animTime);
            //Debug.Log("time = "+time+" < "+ animTime);
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        if (flagMovingDirection) {
            card.PlayRandomClipOnce(clipsCardDown);
        } else {             
            card.PlayRandomClipOnce(clipsCardUp); 
        }        
        t.localPosition = finishVector;
        card.IsMoving = false;
    }

    private bool AreCardsMoving(){
        bool flag = false;
        for (int i = 0; i < poolCards.Length; i++) {
            if (poolCards[i].IsMoving) {
                flag = true;
                break;
            } 
        }
        return flag;
    }

    private float DegreesInRadian (float degrees){
        return degrees * Mathf.PI / 180.0f;
    }

    private void SetVarFromDifficulty (int iGameDifficulty){

        Debug.Log("SetVarFromDifficulty = " + iGameDifficulty);
        //iGameDifficulty = 99;  // Debug line
        switch (iGameDifficulty) {
            case 0:
                nCardsInLevel = 3;
                nOpenCardsInLevel = 3;
                flagsOpenCardsInLevel = "111";
                flagsTrapCardsInLevel = "00";
                break;            
            case 1:
                nCardsInLevel = 3;
                nOpenCardsInLevel = 2;
                flagsOpenCardsInLevel = "101";
                flagsTrapCardsInLevel = "00";
                break;
            case 2:
                nCardsInLevel = 3;
                nOpenCardsInLevel = 1;
                flagsOpenCardsInLevel = "010";
                flagsTrapCardsInLevel = "00";
                break;            
            case 3:
                nCardsInLevel = 3;
                nOpenCardsInLevel = 0;
                flagsOpenCardsInLevel = "000";
                flagsTrapCardsInLevel = "00";
                break;
            case 4:
                nCardsInLevel = 5;
                nOpenCardsInLevel = 3;
                flagsOpenCardsInLevel = "10101";
                flagsTrapCardsInLevel = "00";
                break;            
            case 5:            
                nCardsInLevel = 5;
                nOpenCardsInLevel = 3;
                flagsOpenCardsInLevel = "10101";
                flagsTrapCardsInLevel = "01";
                break;                     
            case 6:
                nCardsInLevel = 5;
                nOpenCardsInLevel = 3;
                flagsOpenCardsInLevel = "10101";
                flagsTrapCardsInLevel = "10";
                break;
            case 7:
                nCardsInLevel = 5;
                nOpenCardsInLevel = 2;
                flagsOpenCardsInLevel = "01010";
                flagsTrapCardsInLevel = "01";
                break;
            case 8:
                nCardsInLevel = 5;
                nOpenCardsInLevel = 2;
                flagsOpenCardsInLevel = "01010";
                flagsTrapCardsInLevel = "10";
                break;            
            case 9:     
                nCardsInLevel = 5;
                nOpenCardsInLevel = 0;
                flagsOpenCardsInLevel = "00000";
                flagsTrapCardsInLevel = "00";
                break;                
            case 10:
            case 11:
            case 12:                                    
                nCardsInLevel = 5;
                nOpenCardsInLevel = 2;
                flagsOpenCardsInLevel = "01010";
                flagsTrapCardsInLevel = "01";
                break;
            case 13:
            case 14:
            case 15:                        
                nCardsInLevel = 5;
                nOpenCardsInLevel = 1;
                flagsOpenCardsInLevel = "00100";
                flagsTrapCardsInLevel = "01";
                break;                
            case 16:
            case 17:
            case 18:
            case 19:
                nCardsInLevel = 5;
                nOpenCardsInLevel = 0;
                flagsOpenCardsInLevel = "00000";
                flagsTrapCardsInLevel = "01";
                break;                                
            case 20:
            case 21:
                nCardsInLevel = 7;
                nOpenCardsInLevel = 2;
                flagsOpenCardsInLevel = "0100010";
                flagsTrapCardsInLevel = "11";
                break;
            case 22:
            case 23:
                nCardsInLevel = 7;
                nOpenCardsInLevel = 1;
                flagsOpenCardsInLevel = "0001000";
                flagsTrapCardsInLevel = "11";
                break;
            case 24:
            case 25:
            case 26:
            case 27:
            case 28:
            case 29:
                nCardsInLevel = 7;
                nOpenCardsInLevel = 0;
                flagsOpenCardsInLevel = "0000000";
                flagsTrapCardsInLevel = "11";
                break;
            case 99:
                nCardsInLevel = 7;
                nOpenCardsInLevel = 7;
                flagsOpenCardsInLevel = "1111111";
                flagsTrapCardsInLevel = "11";
                break;                                                              
            default:
                nCardsInLevel = 5;
                nOpenCardsInLevel = 3;
                flagsOpenCardsInLevel = "10101";
                flagsTrapCardsInLevel = "11";
                break;
        }
    }

    private int[] GetPoolToys(){
        List<int> poolToys = new List<int>();
        for (int i = 0; i < poolCards.Length; i++) {
            if (poolCards[i].toy < 50){
                poolToys.Add(poolCards[i].toy);
            }
        }
        return poolToys.ToArray();
    }
    private void OnFirstOpenTrapCard_GM (int cardNumber, int toyNumber) {
        //Debug.Log("TRAP CARD FIRST OPEN!!!");
        poolCards[cardNumber].TrapCardOpen();
    }
    private void OnEndAnimationTrapCard_CMAnim (int cardNumber){
        //Debug.Log("TRAP CARD Anim END!!!");
        poolCards[cardNumber].DisableGrate();
    }
    
    private void OnDoTierActions_GM() {
        SetBlockRotationIconAllCards(true);
    }
    private void OnTierActionsEnd_GM() {
        SetBlockRotationIconAllCards(false);
    }

    private void SetBlockRotationIconAllCards(bool state){
        foreach (Card card in poolCards) {
            if (state){
                card.ActivateBlockRotationIcon();
            } else {
                card.DisableBlockRotationIcon();
            }
        }

    }
}

