using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuEventManager : MonoBehaviour
{

    // MenuController - delegates and events
    public delegate void InitMission_MenuCont_Delegate(int iArrow, Transform tHouse, int iDifficulty);
    public static event InitMission_MenuCont_Delegate OnInitMission_MenuCont;

    // TownBornController - delegates and events
    public delegate void TownBorn_MenuCont_Delegate();
    public static event TownBorn_MenuCont_Delegate OnEnterTownBorn_MenuCont;
    public static event TownBorn_MenuCont_Delegate OnExitTownBorn_MenuCont;

    // AnimAppNameStartController - delegates and events
    public delegate void AnimAppName_Delegate();    
    public static event AnimAppName_Delegate OnExitAppNameStart_AnimAppNameController;    


    // MenuController - public static metods
    public static void mOnInitMission_MenuCont (int iArrow, Transform tHouse, int iDifficulty) {
        Debug.Log("EVENT ---> MEM.mOnInitMission_MenuCont");
        var handler = OnInitMission_MenuCont;
        if (handler != null) handler(iArrow, tHouse, iDifficulty);
    }    

    // TownBornController - public static metods
    public static void mOnEnterTownBorn_MenuCont () {
        Debug.Log("EVENT ---> MEM.mOnEnterTownBorn_MenuCont");
        var handler = OnEnterTownBorn_MenuCont;
        if (handler != null) handler();
    }    
    public static void mOnExitTownBorn_MenuCont () {
        Debug.Log("EVENT ---> MEM.mOnExitTownBorn_MenuCont");
        var handler = OnExitTownBorn_MenuCont;
        if (handler != null) handler();
    }        

    // AnimAppNameController - public static metods
    public static void mOnExitAppNameStart_AnimAppNameController () {
        Debug.Log("EVENT ---> MEM.mOnExitAppNameStart_AnimAppNameController");
        var handler = OnExitAppNameStart_AnimAppNameController;
        if (handler != null) handler();
    }            


}
