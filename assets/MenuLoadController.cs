using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuLoadController : MonoBehaviour
{
    public Text textLoading;    
    public string sceneName;
    private AsyncOperation asyOperation;

    private float progressValue = 0f;
    private string progressText = "0%"; 

    private void OnEnable() {
        FirebaseManagerController.FBA_EventScreenView("Load Screen");
        StartCoroutine(AsyncLoading());
    }

    private void OnDisable() {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (asyOperation.progress >= 0.9f)
        {
            // Максимальное значение operation.progress - 0,9
            progressValue = 1.0f;
        } else {
            progressValue = asyOperation.progress;
        }

        progressText = ((float)(progressValue*100)).ToString("#") +"%"; 
 
        if (progressText != textLoading.text)
        {
            // Операция интерполяции
            textLoading.text = progressText;
        }
 
        if (progressValue == 1.0f)
        {
            // Разрешить автоматическое переключение сцен после асинхронной загрузки
            asyOperation.allowSceneActivation = true;
        }
    }

    IEnumerator AsyncLoading()
    {
        asyOperation = SceneManager.LoadSceneAsync(sceneName);
                 // Предотвращаем автоматическое переключение при завершении загрузки
        asyOperation.allowSceneActivation = false;
 
        yield return asyOperation;
    }
}
