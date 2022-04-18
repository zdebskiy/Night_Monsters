using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartEventManager : MonoBehaviour
{

    // AppNameAnim - delegates and events
    public delegate void AppNameAnim_Delegate();
    //public static event AppNameAnim_Delegate OnStartAnimAppName;
    public static event AppNameAnim_Delegate OnEndAnimAppName;



    // AppNameAnim - public static metods
    /*
    public static void mOnStartAnimAppName () {
        Debug.Log("EVENT Anim ---> mOnStartAnimAppName");
        var handler = OnStartAnimAppName;
        if (handler != null) handler();
    } 
    */       
    public static void mOnEndAnimAppName () {
        Debug.Log("EVENT Anim ---> mOnEndAnimAppName");
        var handler = OnEndAnimAppName;
        if (handler != null) handler();
    }        


}
