using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{
    private CardsManager cardsManager;
    public int number;

    // Start is called before the first frame update
    void Start()
    {
        cardsManager = transform.parent.GetComponent<CardsManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnMouseDown() {        
        cardsManager.CardClicked(number);
    }    
}
