using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

/// <summary>
/// 控制場景的加載、遊戲時間、應用程式(關閉)
/// </summary>
public class SceneController : MonoBehaviour
{
    public string sceneName_MainMenu = "MainMenu";//主選單的名稱

    public UnityEvent OnLoadSceneAsyncBegin;
    public UnityEvent OnLoadSceneAsyncComplete;
    public LoadingEvent OnLoadingAsyncProgress;
    
    private bool isReadyEnterScene;


    /// <summary>
    /// 載入場景時暫停時間，載入完成後手動恢復時間(Press To Continue)
    /// </summary>
    /// <param name="sceneName"></param>
    public void LoadLevel(string sceneName, bool waitToActivateScene)
    {
        Debug.Log("Load Level" + sceneName);
        //不存在的場景名字會導致錯誤
        StartCoroutine(LoadAsynchronously(sceneName, waitToActivateScene));
    }
    public void ReloadLevel()
    {
        Debug.Log("Restart this level");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void LoadMainMenu() 
    {
        Debug.Log("Load " + sceneName_MainMenu);
        StartCoroutine(LoadAsynchronously(sceneName_MainMenu, false));
    }

    /// <summary>
    /// 先卸載舊場景，在載入新場景。
    /// 避免發生，不允許同時出現的物件同時出現的情況。
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    private IEnumerator LoadAsynchronously(string sceneName, bool waitToActivateScene) 
    {
        OnLoadSceneAsyncBegin.Invoke();
        isReadyEnterScene = false;

        AsyncOperation asyncOperation;
        Scene oldScene = SceneManager.GetActiveScene();
        Scene newScene;

        //Load New Scene
        Debug.Log("Load Level Async " + sceneName);
        asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        asyncOperation.allowSceneActivation = false;//不激活場景
        while (!asyncOperation.isDone)
        {
            Debug.Log("Loading " + asyncOperation.progress);
            float loadProgress = Mathf.Clamp01(asyncOperation.progress / 0.9f);
            OnLoadingAsyncProgress.Invoke(loadProgress);

            //載入完成，等待激活
            if (asyncOperation.progress >= 0.9f)
            {
                if (waitToActivateScene)
                {
                    //修改成委派的形式，做更泛用的確認機制
                    yield return new WaitUntil(() => isReadyEnterScene);
                    asyncOperation.allowSceneActivation = true;
                }
                else
                {
                    asyncOperation.allowSceneActivation = true;
                }
            }
            yield return null;
        }
        //"重要"設為啟動，否則物件生成在其他場景，卸載時會跟著被刪除
        newScene = SceneManager.GetSceneByName(sceneName);
        SceneManager.SetActiveScene(newScene);

        OnLoadSceneAsyncComplete.Invoke();

        //Unload Old Scene
        Debug.Log("Unload Level Async " + oldScene.name);
        asyncOperation = SceneManager.UnloadSceneAsync(oldScene);
        asyncOperation.allowSceneActivation = false;
    }

    public void ReadyToEnterScene() 
    {
        isReadyEnterScene = true;
    }

    #region Application Control
    public void Quit()
    {
        Debug.Log("Application Quit");
        Application.Quit();
    }
    #endregion
}


[System.Serializable]
public class LoadingEvent : UnityEvent<float> { }