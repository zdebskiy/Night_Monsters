using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadController : MonoBehaviour
{
    public Text textLoading;
    public string sceneName;
    private AsyncOperation asyOperation;

    private float progressValue = 0.0f;
    private string progressText = "0.0%"; 

    private bool flagEndAnimAppName = false;

    private void OnEnable() {
        StartEventManager.OnEndAnimAppName += OnEndAnimAppName;
    }

    private void OnDisable() {
        StartEventManager.OnEndAnimAppName -= OnEndAnimAppName;
    }

    // Start is called before the first frame update
    void Start() {
        StartCoroutine(AsyncLoading());        
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

        progressText = ((float)(progressValue*100)).ToString("#.#") + "%"; 
 
        if (progressText != textLoading.text)
        {
            // Операция интерполяции
            textLoading.text = progressText;
        }
 
        if ((progressValue == 1.0f) && (flagEndAnimAppName))
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

    private void OnEndAnimAppName(){
        flagEndAnimAppName = true;
    }

}
