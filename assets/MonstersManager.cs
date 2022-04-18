using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MonstersManager : MonoBehaviour
{
    public GameObject[] goMonsterPrefabs;
    public GameObject[] goSetToysPrefabs;    
    public poolMonstersElem[] poolMonsters;
    public Sprite [] sprDialogsKill;
    public Vector3 extremeRightPos;
    public Vector3 extremeLeftPos;

    public GameObject goDialogSleep;
    public Sprite [] sprDialogSleepArr;
    public GameObject goVectorToyInDialogSleep;
    public AudioClip[] arrAudioClipDialogSleep;

    private float shiftSize; // diametr Jump Circle

    private int iSetToysPrefabsInGame = 0;
    private int[] poolToysInGame;

    private int nMonstersInGame = 3;

    private Animation animDialogSleep;
    private SpriteRenderer sprToyDialogSleep;
    private SpriteRenderer sprDialogSleep;
    private Transform tToyDialogSleep;
    private Vector3 v3StartPosDialogSleep;
    private AudioSource audioSourceDialogSleep;    


    // Timer DialogSleep Update
    private const float TimerPeriod_DS = 4.0f;
    private float m_TimerPeriod_DS = TimerPeriod_DS;
    // ------------------------



    public struct poolMonstersElem {
        public bool isEmptyPosition;
        public Monster monster;

    }

    public class Monster {
        public GameObject go;
        public Animator animator;
        public AudioSource audioSource;
        public int iPrefab;
        public int toy;
        public bool isKilled;
        bool isClicked;

        public Monster (GameObject newGo, Animator newAnimator, AudioSource newAudioSource, int newIPreafab, int newToy, bool newIsKilled, bool newIsClicked){
            go = newGo;
            animator = newAnimator;
            audioSource = newAudioSource;
            iPrefab = newIPreafab;
            //number = newNumber;
            toy = newToy;
            isKilled = newIsKilled;
            isClicked = newIsClicked;
        }


        public void Shift(Vector3 shiftVector){
            go.transform.position +=  shiftVector;
        }

        public void Killed (){
            isKilled = true;
            animator.SetBool("isKilledMonster", true);
        }
        public bool IsKilled () {
            return isKilled;
        }                   
        public bool IsClicked () {
            return isClicked;
        }     

        public void Mute(bool status){
            audioSource.mute = status;
        }      

    }

    // Start is called before the first frame update
    void Start()
    {        
        
    }
    private void OnEnable() {
        EventManager.OnCheckKillMonster_GM += OnCheckKillMonster_GM;
        EventManager.OnNeedAddMonsterInRoom_GM += OnNeedAddMonsterInRoom_GM;
        EventManager.OnNeedShiftMonstersBack_GM += OnNeedShiftMonstersBack_GM;
        EventManager.OnInitMonsters_GM += OnInitMonsters_GM;

        EventManager.OnBulletHitMonster_TBC += OnBulletHitMonster_TBC;

        //EventManager.OnStartAnimationDieMonster_MMAnim += OnStartAnimationDieMonster_MMAnim;
        EventManager.OnEndAnimationDieMonster_MMAnim += OnEndAnimationDieMonster_MMAnim;

        EventManager.OnGamePause_GM += OnGamePause_GM;     
        EventManager.OnGameWon_GM += OnGameWon_GM;     
        EventManager.OnGameLost_GM += OnGameLost_GM;     
    }
    private void OnDisable() {
        EventManager.OnCheckKillMonster_GM -= OnCheckKillMonster_GM;
        EventManager.OnNeedAddMonsterInRoom_GM -= OnNeedAddMonsterInRoom_GM;
        EventManager.OnNeedShiftMonstersBack_GM -= OnNeedShiftMonstersBack_GM;        
        EventManager.OnInitMonsters_GM -= OnInitMonsters_GM;

        EventManager.OnBulletHitMonster_TBC -= OnBulletHitMonster_TBC;

        //EventManager.OnStartAnimationDieMonster_MMAnim -= OnStartAnimationDieMonster_MMAnim;
        EventManager.OnEndAnimationDieMonster_MMAnim -= OnEndAnimationDieMonster_MMAnim;        

        EventManager.OnGamePause_GM -= OnGamePause_GM;
        EventManager.OnGameWon_GM -= OnGameWon_GM;     
        EventManager.OnGameLost_GM -= OnGameLost_GM;           
    }
    // Update is called once per frame
    void Update(){
        
    }

    private void FixedUpdate() {

        // Timer Dialog Sleep
        if (poolMonsters != null){
            m_TimerPeriod_DS -= Time.deltaTime;
            if (m_TimerPeriod_DS < 0) {
                UpdateDialogSleep ();
                m_TimerPeriod_DS = TimerPeriod_DS;
            }        
        } else {
            //Debug.Log("poolMonsters - Not INIT!!!");
        }
        // ------------------        
    }

    private void OnInitMonsters_GM (int iGameDifficulty, int iSetToysPrefabs, int[] poolToys){
        iSetToysPrefabsInGame = iSetToysPrefabs;
        poolToysInGame = poolToys;
        SetVarFromDifficulty(iGameDifficulty);        
        InitPoolMonsters(nMonstersInGame);
        InitDialogSleep();
        AddMonster();
        EventManager.mOnInitMonstersEnd_MM(nMonstersInGame);
    }

    private void InitPoolMonsters (int n) {
        poolMonsters = new poolMonstersElem[n];
        for (int i = 0; i < poolMonsters.Length; i++) {
            poolMonsters[i].isEmptyPosition = true;
            poolMonsters[i].monster = new Monster(null, null, null, 0, -1, false, false);
        }
        //Debug.Log("Init Pool Monsters. Length = "+poolMonsters.Length);        
    }

    public IEnumerator RushMonsters(){
        if (poolMonsters[poolMonsters.Length-1].isEmptyPosition) {
            //Debug.Log("Have empty position!");            
            for (int i = poolMonsters.Length-1; i > 0; i--) {
                //Debug.Log("Monster [ "+(i-1)+" ] go to pos [ "+i+" ]");
                if ((poolMonsters[i].isEmptyPosition) && (!poolMonsters[i-1].isEmptyPosition)) {
                    //Debug.Log("++++++++++++++++++ > I = "+i);
                    yield return StartCoroutine(JumpCoroutine(poolMonsters[i-1].monster, true));
                    poolMonsters[i].isEmptyPosition = false;
                    poolMonsters[i].monster = poolMonsters[i-1].monster;                    
                    poolMonsters[i].monster.go.name = "Monster_"+i.ToString();
                    poolMonsters[i-1].isEmptyPosition = true;
                    //poolMonsters[i].monster.Shift(Vector3.left*4);                    
                } else {
                    //Debug.Log("------------------ > I = "+i);
                }
            }
            AddMonster();                        
        } else {
            Debug.Log("Monster EAT YOU BRAIN!!!");
        }
    }

/*
    IEnumerable coroShift(){
        Debug.Log("Start Cooooooooooooooooooooooooooooooooooooooooorrrrrooooo!!!");
        //go.transform.position += shiftVector * Time.deltaTime;

        yield return new WaitForSeconds(0.1f);
    }    
*/
    public void ShiftMonstersBack() {
        for (int i = 0; i < poolMonsters.Length; i++) {
            //Debug.Log("Position ["+i+"] = "+poolMonsters[i].isEmptyPosition);
            if (poolMonsters[i].monster.IsKilled()) {
                //Debug.Log("Killed!");
                StartCoroutine(ShiftMonstersBackAnimation(i));
                break;
            } else {
                //Debug.Log("Monster ["+i+"] - Not Killed!");
            }
        }
        //StartCoroutine(ShiftMonstersBackAnimation(i));
        //Debug.Log("YoHoo SHIFTED!!!!");
        //EventManager.mOnMonstersShiftedBack_MM();
    }



    private void AddMonster(){
        EventManager.mOnAddMonsterInRoom_MM(); // send to WardrobeController

        int newIPreafb = 0;
        bool flagNoRepeat = true;
        // Check repeat monsters in room
        do {
            flagNoRepeat = true;
            newIPreafb = UnityEngine.Random.Range(0, goMonsterPrefabs.Length);
            for (int i=0; i < poolMonsters.Length; i++){
                if (!poolMonsters[i].isEmptyPosition) {
                    if (poolMonsters[i].monster.iPrefab == newIPreafb){
                        flagNoRepeat = false;
                        break;
                    }
                }
            }
        } while(!flagNoRepeat);


        GameObject go = Instantiate (goMonsterPrefabs[newIPreafb]) as GameObject;
        Transform t = go.transform;
        Transform tTrapMsg = transform;// = new GameObject().transform;
        // find go "TrapMsg" in monster prefab
        for (int i = 0; i < t.childCount; i++) {
            if (t.GetChild(i).transform.name == "TrapMsg"){
                tTrapMsg = t.GetChild(i).transform;
                break;
            }
        }                
        SpriteRenderer sprDialog = tTrapMsg.GetChild(0).GetComponent<SpriteRenderer>();        
        sprDialog.sprite = sprDialogsKill[UnityEngine.Random.Range(0, sprDialogsKill.Length)];
        SpriteRenderer sprToy = tTrapMsg.GetChild(1).GetComponent<SpriteRenderer>();
        t.SetParent (transform);
        t.localPosition = new Vector3(4.0f, 8.0f, 0.0f);
        t.name = "Monster_0";
     
        Animator animator = go.GetComponent<Animator>();
        Animator animatorTrapMsg = tTrapMsg.GetComponent<Animator>();
        AudioSource audioSource = go.GetComponent<MonsterController>().audioSource;
        //CardController cc = go.GetComponent<CardController>();
        //cc.number = i;
        //int number = i;

        //int toy = 0; // line for debuging
        int toy = poolToysInGame[UnityEngine.Random.Range(0, poolToysInGame.Length)];

        sprToy.sprite = goSetToysPrefabs[iSetToysPrefabsInGame].transform.GetChild(toy).GetComponent<SpriteRenderer>().sprite;

        bool isKilled = false;
        bool isClicked = false;
        poolMonsters[0].isEmptyPosition = false;
        poolMonsters[0].monster = new Monster(go, animator, audioSource, newIPreafb, toy, isKilled, isClicked);
        StartCoroutine(BornAnimation(poolMonsters[0].monster));
        //StartCoroutine(RunDialogSleep());
    }

    private void OnCheckKillMonster_GM(int numberCard, int toyNumber){
        bool flagKilled = false;   
        for (int i = 0; i < poolMonsters.Length; i++) {
            if (!poolMonsters[i].isEmptyPosition) {
                //Debug.Log("Monster "+i+" = NotEmpty");
                //Debug.Log(toyNumber+" == "+poolMonsters[i].monster.toy);
                if (toyNumber == poolMonsters[i].monster.toy){                    
                    poolMonsters[i].monster.isKilled = true;
                    Debug.Log("MONSTER ["+i+"] - flag killed"+poolMonsters[i].monster.IsKilled());
                    EventManager.mOnToyKillSomeMonster_MM(numberCard, toyNumber, poolMonsters[i].monster.go.transform);                    
                    //EventManager.mOnMonsterKilled_MM(numberCard, toyNumber, poolMonsters[i].monster.go.transform);                    
                    flagKilled = true;
                    break;
                }
            }
        }    
        if (!flagKilled){
            EventManager.mOnToyNotKillSomeMonster_MM(numberCard, toyNumber);
        }        
    }
    
    private void OnNeedAddMonsterInRoom_GM () {
        StartCoroutine(RushMonsters());        
    }
    private void OnNeedShiftMonstersBack_GM () {
        ShiftMonstersBack();        
    }
    private void OnBulletHitMonster_TBC (string targetName) {
        for (int i = 0; i < poolMonsters.Length; i++)
        {
            if (!poolMonsters[i].isEmptyPosition){
                if (poolMonsters[i].monster.IsKilled()){
                    poolMonsters[i].monster.Killed();    
                }
            }
        }        
    }
    private void OnEndAnimationDieMonster_MMAnim(GameObject goMonster){
        EventManager.mOnMonsterKilled_MM();
        goMonster.GetComponent<Animator>().SetBool("isKilledMonster", false);
        Destroy(goMonster);
    }

    IEnumerator JumpOneAfterOther() {
        for (int i = poolMonsters.Length-1; i >= 0; i--) {
            if (!poolMonsters[i].isEmptyPosition){
                //Debug.Log("---> Jump Monster["+i+"]");                
                yield return StartCoroutine(JumpCoroutine(poolMonsters[i].monster, true));
            }
        }
        AddMonster();
    }

    IEnumerator ShiftMonstersBackAnimation(int startPos) {
        bool flagShifted = false;
        for (int j = startPos; j < poolMonsters.Length-1; j++) {
            if (!poolMonsters[j+1].isEmptyPosition){
                flagShifted =true;
                yield return StartCoroutine(JumpCoroutine(poolMonsters[j+1].monster, false));                
                poolMonsters[j].monster=poolMonsters[j+1].monster;                        
                poolMonsters[j].monster.go.name = "Monster_"+j.ToString();                        
                poolMonsters[j].isEmptyPosition = false;
                poolMonsters[j+1].isEmptyPosition = true;                
            } else {
                break;
            }
        }
        if (!flagShifted){
            poolMonsters[startPos].isEmptyPosition = true;
        }
        //Debug.Log("YoHoo SHIFTED!!!!");
        EventManager.mOnMonstersShiftedBack_MM();        
    }

    public IEnumerator JumpCoroutine(Monster monster, bool directionForward) {
        float jumpTime = 1.0f;
        monster.animator.Play("Base Layer.Jump");
        //jumpTime = monster.animator.GetCurrentAnimatorStateInfo(0).length;
        //Debug.Log("Jump TIME --> "+jumpTime);
        //Debug.Log("Jump Animation Time: "+jumpTime);
        if (directionForward){
            StartCoroutine(JumpForwardCoroutine(monster, jumpTime));
        } else {
            StartCoroutine(JumpBackCoroutine(monster, jumpTime));
        }        
        yield return new WaitForSeconds(jumpTime/2);
    }

    IEnumerator JumpBackCoroutine(Monster monster, float jumpTime)
    {   
        //float shiftSize = 5.0f;
        Vector3 startPosition = monster.go.transform.position;
        float radiusCircle = shiftSize/2.0f;
        Vector3 centerCircle = new Vector3 (startPosition.x + radiusCircle, startPosition.y, 0);

        int nFrame = 60;
        float delta = shiftSize / nFrame;

        //Debug.Log("Delta = "+delta);
        for (float x = 0; x <= shiftSize; x+=delta) {
            double y=0;
            y=Math.Sqrt(Math.Pow(radiusCircle, 2) - Math.Pow((startPosition.x+x - centerCircle.x), 2)) + centerCircle.y;
            Vector3 targetPosition = new Vector3(startPosition.x+x, Convert.ToSingle(y), 0.0f);
            monster.go.transform.Translate(targetPosition - monster.go.transform.position);
            yield return new WaitForSeconds(jumpTime/nFrame/2);
        }
    }    
    IEnumerator JumpForwardCoroutine(Monster monster, float jumpTime)
    {   
        //float shiftSize = 5.0f;
        Vector3 startPosition = monster.go.transform.position;
        float radiusCircle = shiftSize/2.0f;
        Vector3 centerCircle = new Vector3 (startPosition.x - radiusCircle, startPosition.y, 0);
        //Debug.Log("Radius = "+radiusCircle);
        //Debug.Log("Center = "+centerCircle);

        int nFrame = 60;
        float delta = shiftSize / nFrame;

        for (float x = 0; x <= shiftSize; x+=delta) {
            double y=0;
            y=Math.Sqrt(Math.Pow(radiusCircle, 2) - Math.Pow((startPosition.x-x - centerCircle.x), 2)) + centerCircle.y;
            Vector3 targetPosition = new Vector3(startPosition.x-x, Convert.ToSingle(y), 0.0f);
            //Debug.Log("JUMP Target -> "+x+" } "+targetPosition);
            monster.go.transform.Translate(targetPosition - monster.go.transform.position);
            yield return new WaitForSeconds(jumpTime/nFrame/2);
        }
    }    

    IEnumerator BornAnimation(Monster monster){
        //monster.animator.SetBool("isBorn", true);
        float animTime = monster.animator.GetCurrentAnimatorStateInfo(0).length;
        float flyTime = 1.0f;
        //Debug.Log("TIME Born --->>> "+animTime);
        StartCoroutine(LinearMoveCoroutine(monster, extremeRightPos, new Vector3((extremeRightPos.x-shiftSize/2), -3.0f, 0.0f), flyTime));
        yield return new WaitForSeconds(animTime);
        //monster.animator.SetBool("isBorn", false);
    }

    IEnumerator LinearMoveCoroutine(Monster monster, Vector3 startVector, Vector3 finishVector, float animTime){
        Transform t = monster.go.transform;
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

    private void SetVarFromDifficulty (int iGameDifficulty){
        switch (iGameDifficulty) {
            case 0:
                nMonstersInGame = 3;
                break;
            case 10:
                nMonstersInGame = 3;
                break;
            case 20:
                nMonstersInGame = 3;
                break;                
            default:
                nMonstersInGame = 3;
                break;
        }
        shiftSize = (Math.Abs(extremeLeftPos.x)+Math.Abs(extremeRightPos.x))/nMonstersInGame;
    }

    private void InitDialogSleep () {
        sprDialogSleep = goDialogSleep.transform.GetComponent<SpriteRenderer>();
        animDialogSleep = goDialogSleep.transform.GetComponent<Animation>() ;
        sprToyDialogSleep = goDialogSleep.transform.GetChild(0).transform.GetComponent<SpriteRenderer>();
        tToyDialogSleep = goDialogSleep.transform.GetChild(0).transform;
        v3StartPosDialogSleep = goDialogSleep.transform.localPosition;
        audioSourceDialogSleep = goDialogSleep.transform.GetComponent<AudioSource>();
    }
    private IEnumerator RunDialogSleep () {
            int nLiveMonsters = GetNMonstersInPool();
            int iDialogSleep =0;
        while (true){
            if (nLiveMonsters > 0){
                for (int i = 0; i < poolMonsters.Length; i++) {
                    if (!poolMonsters[i].isEmptyPosition){
                        iDialogSleep = UnityEngine.Random.Range(0, sprDialogSleepArr.Length);
                        sprDialogSleep.sprite = sprDialogSleepArr[iDialogSleep];                        
                        tToyDialogSleep.localPosition = goVectorToyInDialogSleep.transform.GetChild(iDialogSleep).transform.GetChild(0).transform.localPosition;
                        sprToyDialogSleep.sprite = goSetToysPrefabs[iSetToysPrefabsInGame].transform.GetChild(poolMonsters[i].monster.toy).GetComponent<SpriteRenderer>().sprite;
                        animDialogSleep.Play("SleepDialog");
                        yield return new WaitForSeconds(5.0f);
                    }                                        
                }
            }        
        }
    }

    private void UpdateDialogSleep () {
        int nLiveMonsters = GetNMonstersInPool();
        int iPoolMonsters = 0;
        int iDialogSleep = 0;
        int iAudioClipDialogSleep = 0;
        if (nLiveMonsters > 0){
            iPoolMonsters = UnityEngine.Random.Range(0, nLiveMonsters);               
            iDialogSleep = UnityEngine.Random.Range(0, sprDialogSleepArr.Length);
            iAudioClipDialogSleep = UnityEngine.Random.Range(0, arrAudioClipDialogSleep.Length);
            sprDialogSleep.sprite = sprDialogSleepArr[iDialogSleep];
            sprDialogSleep.transform.localPosition = v3StartPosDialogSleep;
            tToyDialogSleep.localPosition = goVectorToyInDialogSleep.transform.GetChild(iDialogSleep).transform.GetChild(0).transform.localPosition;
            sprToyDialogSleep.sprite = goSetToysPrefabs[iSetToysPrefabsInGame].transform.GetChild(poolMonsters[iPoolMonsters].monster.toy).GetComponent<SpriteRenderer>().sprite;
            if (nLiveMonsters != nMonstersInGame) {                
                sprDialogSleep.flipX = false;
                //sprDialogSleep.transform.localPosition = new Vector3(sprDialogSleep.transform.localPosition.x, sprDialogSleep.transform.localPosition.y, sprDialogSleep.transform.localPosition.z);
                //tToyDialogSleep.localPosition = new Vector3(-tToyDialogSleep.localPosition.x, tToyDialogSleep.localPosition.y, tToyDialogSleep.localPosition.z);
            } else {
                sprDialogSleep.flipX = true;
                sprDialogSleep.transform.localPosition = new Vector3(sprDialogSleep.transform.localPosition.x-1.0f, sprDialogSleep.transform.localPosition.y, sprDialogSleep.transform.localPosition.z);
                tToyDialogSleep.localPosition = new Vector3(-tToyDialogSleep.localPosition.x, tToyDialogSleep.localPosition.y, tToyDialogSleep.localPosition.z);
            }             
            animDialogSleep.Play("SleepDialog");
            audioSourceDialogSleep.PlayOneShot(arrAudioClipDialogSleep[iAudioClipDialogSleep]);
        }        
    }    

    private int GetNMonstersInPool (){
        int count=0;
        for (int i = 0; i < poolMonsters.Length; i++){
            if (!poolMonsters[i].isEmptyPosition){
                count++;
            }
        }
        return count;
    }

    private void OnGamePause_GM (bool status){
        MuteMonstersInRoom(status);
    }

    private void OnGameWon_GM(){
        MuteMonstersInRoom(true);
    }
    private void OnGameLost_GM(){
        MuteMonstersInRoom(true);
    }    

    private void MuteMonstersInRoom (bool status){
        // Mute monsters in Room
        foreach(poolMonstersElem pMonster in poolMonsters){
            if (!pMonster.isEmptyPosition){
                pMonster.monster.Mute(status);
            }
        }

        // Mute Dialog Sleep in Room
        audioSourceDialogSleep.mute = status;                
    }

}
