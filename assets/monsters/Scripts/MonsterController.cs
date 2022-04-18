using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    public Animator animDialog;
	public AudioClip[] clipsJump;
	public AudioClip[] clipsWait;
    public AudioClip[] clipsBorn;
    public AudioClip[] clipsDie;
	public AudioSource audioSource;

    private void OnEnable() {
        EventManager.OnBulletHitMonster_TBC += OnBulletHitMonster_TBC;
        EventManager.OnStartAnimationAddMonster_MMAnim += OnStartAnimationAddMonster_MMAnim;
        EventManager.OnStartAnimationDieMonster_MMAnim += OnStartAnimationDieMonster_MMAnim;
        EventManager.OnStartAnimationJumpMonster_MMAnim += OnStartAnimationJumpMonster_MMAnim;
        //EventManager.OnEndAnimationAddMonster_MMAnim += OnEndAnimationAddMonster_MMAnim;
        //EventManager.OnEndAnimationJumpMonster_MMAnim += OnEndAnimationJumpMonster_MMAnim;
        EventManager.OnStartAnimationWaitMonster_MMAnim += OnStartAnimationWaitMonster_MMAnim;
        //EventManager.OnEndAnimationWaitMonster_MMAnim += OnEndAnimationWaitMonster_MMAnim;
        EventManager.OnEndAnimationDieMonster_MMAnim += OnEndAnimationDieMonster_MMAnim;                
    }

    private void OnDisable() {
        EventManager.OnBulletHitMonster_TBC -= OnBulletHitMonster_TBC;
        EventManager.OnStartAnimationAddMonster_MMAnim -= OnStartAnimationAddMonster_MMAnim;
        EventManager.OnStartAnimationDieMonster_MMAnim -= OnStartAnimationDieMonster_MMAnim;
        EventManager.OnStartAnimationJumpMonster_MMAnim -= OnStartAnimationJumpMonster_MMAnim;
        //EventManager.OnEndAnimationAddMonster_MMAnim -= OnEndAnimationAddMonster_MMAnim;
        //EventManager.OnEndAnimationJumpMonster_MMAnim -= OnEndAnimationJumpMonster_MMAnim;  
        EventManager.OnStartAnimationWaitMonster_MMAnim -= OnStartAnimationWaitMonster_MMAnim;            
        //EventManager.OnEndAnimationWaitMonster_MMAnim -= OnEndAnimationWaitMonster_MMAnim;
        EventManager.OnEndAnimationDieMonster_MMAnim -= OnEndAnimationDieMonster_MMAnim;        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnBulletHitMonster_TBC (string targetName) {
        if (gameObject.name == targetName) {
            EventManager.mOnBulletHitMonster_MC();
            animDialog.SetTrigger("BulletHitMonster");
        }
    }

    void OnStartAnimationAddMonster_MMAnim(string sGoName){
        if (gameObject.name == sGoName) {
            audioSource.loop = false;
            audioSource.PlayOneShot(clipsBorn[Random.Range(0, clipsBorn.Length)]);
        }
    }
    void OnStartAnimationDieMonster_MMAnim(GameObject go){
        if (gameObject.name == go.name) {
            audioSource.loop = false;
            audioSource.PlayOneShot(clipsDie[Random.Range(0, clipsDie.Length)]);
        }
    }
    void OnStartAnimationJumpMonster_MMAnim(string sGoName){
        if (gameObject.name == sGoName) {
            audioSource.loop = false;
            audioSource.PlayOneShot(clipsJump[Random.Range(0, clipsJump.Length)]);
        }
    }

    void OnStartAnimationWaitMonster_MMAnim(string sGoName){
        if (gameObject.name == sGoName){
            audioSource.loop = true;
            audioSource.clip = clipsWait[Random.Range(0, clipsWait.Length)];
            audioSource.Play();
        }
    }

    void OnEndAnimationDieMonster_MMAnim(GameObject go){
        if (gameObject.name == go.name) {
            audioSource.loop = false;
            audioSource.Stop();
        }
    }

    /*
    void OnTriggerEnter2D(Collider2D col) {		
		if (col.gameObject.tag == "ToyBullet") {
            EventManager.mOnBulletHitMonster_MC();
            animDialog.SetTrigger("BulletHitMonster");
        }
    }*/
}
