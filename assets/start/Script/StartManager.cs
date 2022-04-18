using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FirebaseManagerController.FBA_EventScreenView("Start_Screen");
    }

    // Update is called once per frame
    void Update()
    {
        
    }



}
