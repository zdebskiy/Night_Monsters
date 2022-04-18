using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommentatorController : MonoBehaviour
{
    public AudioClip[] arrAudioClipsTrapWarning;
    public AudioClip[] arrAudioClipsTrapAction;
    public AudioClip[] arrAudioClipsCorrectCard;
    public AudioClip[] arrAudioClipsWrongCard;
    public AudioClip[] arrAudioClipsWin;
    public AudioClip[] arrAudioClipsGameOver;
    public AudioClip[] arrAudioClipsPause;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = transform.GetComponent<AudioSource>();
    }

    private void OnEnable() {
        EventManager.OnFirstOpenTrapCard_GM += OnFirstOpenTrapCard_GM;
        EventManager.OnTrapCardMixSelected_GM += OnTrapCardMixSelected_GM;
        EventManager.OnTrapCardAddMonsterSelected_GM += OnTrapCardAddMonsterSelected_GM;      
        EventManager.OnToyKillSomeMonster_MM += OnToyKillSomeMonster_MM;
        EventManager.OnToyNotKillSomeMonster_MM += OnToyNotKillSomeMonster_MM;
        EventManager.OnGameLost_GM += OnGameLost_GM;
        EventManager.OnGameWon_GM += OnGameWon_GM;
        EventManager.OnGamePause_GM += OnGamePause_GM;
    }

    private void OnDisable() {
        EventManager.OnFirstOpenTrapCard_GM -= OnFirstOpenTrapCard_GM;
        EventManager.OnTrapCardMixSelected_GM -= OnTrapCardMixSelected_GM;
        EventManager.OnTrapCardAddMonsterSelected_GM -= OnTrapCardAddMonsterSelected_GM;        
        EventManager.OnToyKillSomeMonster_MM -= OnToyKillSomeMonster_MM;
        EventManager.OnToyNotKillSomeMonster_MM -= OnToyNotKillSomeMonster_MM;
        EventManager.OnGameLost_GM -= OnGameLost_GM;
        EventManager.OnGameWon_GM -= OnGameWon_GM;
        EventManager.OnGamePause_GM -= OnGamePause_GM;
    }

    private void OnFirstOpenTrapCard_GM(int numberCard, int toyCard){
        audioSource.PlayOneShot(RandomClip(arrAudioClipsTrapWarning));
    }    
    private void OnTrapCardMixSelected_GM(int card){
        audioSource.PlayOneShot(RandomClip(arrAudioClipsTrapAction));
    }    
    private void OnTrapCardAddMonsterSelected_GM(int card){
        audioSource.PlayOneShot(RandomClip(arrAudioClipsTrapAction));
    }
    private void OnToyKillSomeMonster_MM (int numberCard, int toyNumber, Transform tMonster) {
        audioSource.PlayOneShot(RandomClip(arrAudioClipsCorrectCard));
    }
    private void OnToyNotKillSomeMonster_MM (int numberCard, int toyNumber) {
        if (toyNumber != -1){
            audioSource.PlayOneShot(RandomClip(arrAudioClipsWrongCard));
        }
    }    
    private void OnGameLost_GM () {
        audioSource.Stop();
        audioSource.PlayOneShot(RandomClip(arrAudioClipsGameOver));
    }
    private void OnGameWon_GM () {
        audioSource.Stop();
        audioSource.PlayOneShot(RandomClip(arrAudioClipsWin));
    }    
    private void OnGamePause_GM (bool state) {
        audioSource.Stop();
        audioSource.PlayOneShot(RandomClip(arrAudioClipsPause));
    }        
    private AudioClip RandomClip( AudioClip[] clips){
        return clips[UnityEngine.Random.Range(0, clips.Length)];
    }

}
