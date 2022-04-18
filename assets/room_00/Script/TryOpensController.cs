using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TryOpensController : MonoBehaviour
{
    public GameObject goListCards;
    public Text txtBottomText;
    
    public Sprite[] arrCardSprite;

    public GameObject prefabListItem;
    public Text tBottomText;

    private Animator animListCards;
    private int nCards = 3;

    private Vector3 extTopPosition = new Vector3 (-70.0f, -50.0f, 0.0f);
    private Vector3 extBotPosition = new Vector3 (-70.0f, -225.0f, 0.0f);
    private ListItem[] arrTryOpens;
    private class ListItem{
        public Transform transform;
        public Image imgCard;
        public GameObject goHighlighting;        
        public ListItem (Transform t, Image img, GameObject go){            
            transform = t;
            imgCard = img;
            goHighlighting = go;
        }        

        public void DisableHighlighting(){
            goHighlighting.SetActive(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        animListCards = gameObject.GetComponent<Animator>();
    }
    private void OnEnable() {
        EventManager.OnInitRoom_GM += OnInitRoom_GM;
        EventManager.OnDoTierActions_GM += OnDoTierActions_GM;
        EventManager.OnTierActionsEnd_GM += OnTierActionsEnd_GM;

        EventManager.OnInitCardsEnd_CM += OnInitCardsEnd_CM;

        EventManager.OnToyNotKillSomeMonster_MM += OnToyNotKillSomeMonster_MM;        
    }
    private void OnDisable() {
        EventManager.OnInitRoom_GM -= OnInitRoom_GM;
        EventManager.OnDoTierActions_GM -= OnDoTierActions_GM;
        EventManager.OnTierActionsEnd_GM -= OnTierActionsEnd_GM;

        EventManager.OnInitCardsEnd_CM -= OnInitCardsEnd_CM;

        EventManager.OnToyNotKillSomeMonster_MM -= OnToyNotKillSomeMonster_MM;
    }

    public void OnInitRoom_GM(int maxSelectedCards){
        Text _t;
        Image _i;
        float cardStepRangeY = (extTopPosition.y + extBotPosition.y)/(maxSelectedCards);
        nCards = maxSelectedCards;
        arrTryOpens = new ListItem[maxSelectedCards];
        for (int i = 0; i < maxSelectedCards; i++) {
            GameObject go = Instantiate (prefabListItem) as GameObject;
            Transform t = go.transform;
            t.SetParent (goListCards.transform);
            t.localScale = new Vector3(0.2f, 0.2f, 0.2f);            
            switch (maxSelectedCards)
            {
                case 1:
                    cardStepRangeY = (extTopPosition.y - extBotPosition.y)/2;                    
                    t.localPosition = new Vector3(extTopPosition.x, (extTopPosition.y - cardStepRangeY + 137.5f), extTopPosition.z);                
                    break;
                case 2:
                    cardStepRangeY = (goListCards.GetComponent<RectTransform>().sizeDelta.y - (t.GetComponent<RectTransform>().sizeDelta.y * t.localScale.y * maxSelectedCards))/3;
                    Debug.Log("cardStepRangeY = "+cardStepRangeY);
                    t.localPosition = new Vector3(extTopPosition.x, (-32-(t.GetComponent<RectTransform>().sizeDelta.y * t.localScale.y) * i - cardStepRangeY * (i+1) + 137.5f), extTopPosition.z);
                    break;
                default:
                    cardStepRangeY = (extTopPosition.y - extBotPosition.y)/(maxSelectedCards-1);
                    t.localPosition = new Vector3(extTopPosition.x, (extTopPosition.y - cardStepRangeY * i)+137.5f, extTopPosition.z);
                    break;
            }
            Debug.Log("---------->"+t.localPosition);
            t.name = "TryOpen_"+i.ToString();
            _t = t.GetChild(0).GetComponent<Text>();
            _i = t.GetComponent<Image>();

            arrTryOpens[i] = new ListItem(t, _i, t.GetChild(1).gameObject);

            _t.text = " - " + (i+1).ToString() + " CARD";
            txtBottomText.text = nCards.ToString()+" CARDS";
        }
    }

    private void OnInitMonstersEnd_MM(int nMonstersInGame){
        //animListCards.SetBool("TryOpensOpen", true);
    }
    private void OnInitCardsEnd_CM(int iSetCardsPrefabs, int iSetToysPrefabs, int[] poolToys){        
        foreach (ListItem item in arrTryOpens) {
            item.imgCard.sprite = arrCardSprite[iSetCardsPrefabs];
        }
    }
    private void OnToyNotKillSomeMonster_MM (int cardNumber, int toyNumber){        
        foreach (ListItem item in arrTryOpens) {            
            if (!item.goHighlighting.activeSelf){
                item.goHighlighting.SetActive(true);
                break;
            }
        }
    }
    private void OnDoTierActions_GM(){
        animListCards.SetBool("TryOpensOpen", false);
    }

    private void DisableAllTryOpens(){
        foreach (ListItem item in arrTryOpens) {
            item.DisableHighlighting();
        }
    }
    private void OnTierActionsEnd_GM(){
        DisableAllTryOpens();
        animListCards.SetBool("TryOpensOpen", true);        
    }    
}
