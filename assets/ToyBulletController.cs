using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToyBulletController : MonoBehaviour
{
    public GameObject goTarget = null;
    public float speed = 2.0f;
    public Sprite[] bulletSprites;
    public int numToy;

    private bool isHitMonster = false;
    private Vector3 targetPosition = Vector3.zero;

    private SpriteRenderer spriteRenderer;
    private void OnEnable() {
        //Debug.Log("TOY BULLET CONTROLLER --->>> Enable!!!");
    }
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("SpriteRenderer START!!!");
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer.sprite == null) {
            spriteRenderer.sprite = bulletSprites[numToy];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (targetPosition == Vector3.zero){
            targetPosition = goTarget.transform.position;
        }
        
        if(!isHitMonster) {
            transform.Translate((targetPosition - transform.position) * speed * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D col) {		
        Debug.Log(col.gameObject.name+" == "+goTarget.name);
		if (col.gameObject.name == goTarget.name) {
            Debug.Log(col.gameObject.name +" <--- I am hit the Monster!!! -----------------------");
            isHitMonster = true;
            EventManager.mOnBulletHitMonster_TBC(goTarget.name);
            Destroy(gameObject);
        }
    }
}
