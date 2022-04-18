using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapDangersController : MonoBehaviour
{
    public AudioClip[] arrAudioClipsPoster;
    private Animator[] arrDangersAnimator; 
    private AudioSource[] arrDangersAudioSource; 

    // Start is called before the first frame update
    void Start()
    {
        InitArrDangers();
    }

    private void OnEnable() {       
        EventManager.OnFirstOpenTrapCard_GM += OnFirstOpenTrapCard_GM;
        EventManager.OnTrapCardMixSelected_GM += OnTrapCardMixSelected_GM;
        //EventManager.OnTrapCardAddMonsterSelected_GM += OnTrapCardAddMonsterSelected_GM;
    }

    private void OnDisable() {
        EventManager.OnFirstOpenTrapCard_GM -= OnFirstOpenTrapCard_GM;
        EventManager.OnTrapCardMixSelected_GM -= OnTrapCardMixSelected_GM;
        //EventManager.OnTrapCardAddMonsterSelected_GM -= OnTrapCardAddMonsterSelected_GM;
    }

    private void InitArrDangers(){
        arrDangersAnimator = new Animator[transform.childCount];
        arrDangersAudioSource = new AudioSource[transform.childCount];
        for (int i=0; i < transform.childCount; i++) {
            Transform tChild = transform.GetChild(i);
            Animator animChild = null;
            AudioSource audioSourceChild = null;
            if (tChild.name.Contains("Danger")){
                animChild = tChild.GetComponent<Animator>();
                if (animChild != null){
                    arrDangersAnimator[i] = animChild;
                }
                audioSourceChild = tChild.GetComponent<AudioSource>();
                if (audioSourceChild != null){
                    arrDangersAudioSource[i] = audioSourceChild;
                }                
            }
        }
        Debug.Log("Init Audio: "+arrDangersAnimator.Length+" + "+arrDangersAudioSource.Length);
    }

    private void OnFirstOpenTrapCard_GM(int numberCard, int toyCard){
        arrDangersAnimator[toyCard-50].SetBool("isDangerOn", true);
        arrDangersAudioSource[toyCard-50].PlayOneShot(arrAudioClipsPoster[UnityEngine.Random.Range(0, arrAudioClipsPoster.Length)]);        
    }    

    private void OnTrapCardMixSelected_GM(int card){
        arrDangersAnimator[0].SetBool("isDangerOn", false);
        arrDangersAnimator[1].SetBool("isDangerOn", false);
        arrDangersAudioSource[0].PlayOneShot(arrAudioClipsPoster[UnityEngine.Random.Range(0, arrAudioClipsPoster.Length)]);
        arrDangersAudioSource[1].PlayOneShot(arrAudioClipsPoster[UnityEngine.Random.Range(0, arrAudioClipsPoster.Length)]);
    }
}
