using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugScreenController : MonoBehaviour
{

    public GameManager GM;
    public MonstersManager MM;
    public CardsManager CM;
    public Text tGM;    
    public Text tMM;
    public Text tCM;

    bool flagGameStarted = false;
    
    private void OnEnable() {
        EventManager.OnInitRoom_GM += OnInitRoom_GM;

        EventManager.OnInitMonstersEnd_MM += OnInitMonstersEnd_MM;
    }
    private void OnDisable() {
        EventManager.OnInitRoom_GM -= OnInitRoom_GM;

        EventManager.OnInitMonstersEnd_MM -= OnInitMonstersEnd_MM;
    }    

    // Start is called before the first frame update
    void Start()
    {
        flagGameStarted = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (flagGameStarted){
            tGM.text = "GM SelectedCards = "+GM.selectedCards.ToString()+" MonstersInRoom = "+GM.monstersInRoom.ToString()+"  MonstersKilled = "+GM.monstersKilled.ToString();

            string s="";
            for (int i = 0; i < MM.poolMonsters.Length; i++) {
                if (MM.poolMonsters[i].isEmptyPosition){
                    if (MM.poolMonsters[i].monster != null){
                        s+= "M["+i+"](empty: "+MM.poolMonsters[i].isEmptyPosition+"; monster: [killed: "+MM.poolMonsters[i].monster.IsKilled()+"; toy: "+MM.poolMonsters[i].monster.toy+"]) \n";
                    } else {
                        s+= "M["+i+"](empty: "+MM.poolMonsters[i].isEmptyPosition+"; monster: null) \n";
                    }
                } else {
                    s+= "M["+i+"](empty: "+MM.poolMonsters[i].isEmptyPosition+"; monster: [killed: "+MM.poolMonsters[i].monster.IsKilled()+"; toy: "+MM.poolMonsters[i].monster.toy+"]) \n";
                }
            }
            tMM.text = s;

            s="";
            for (int i = 0; i < CM.poolCards.Length; i++) {
                s += "C["+i+"](Number: "+CM.poolCards[i].number+"; Toy: "+CM.poolCards[i].toy+"; Open: "+CM.poolCards[i].IsOpen+"; DefOpen: "+CM.poolCards[i].IsDefaultOpen+"; Clicked: "+CM.poolCards[i].IsClicked+"; Moving: "+CM.poolCards[i].IsMoving+") \n";
            }
            tCM.text = s;
        }
    }

    private void OnInitRoom_GM(int maxSelectedCards){
        flagGameStarted = false;
    }

    private void OnInitMonstersEnd_MM(int nMonstersInGame){
        flagGameStarted = true;
    }


}
