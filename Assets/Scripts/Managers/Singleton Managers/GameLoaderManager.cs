using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoaderManager : Singleton<GameLoaderManager>, ISingleton
{
    private const string MAIN_HUD_PATH = "Prefabs/UI/MainHud";

    private Canvas mainCanvas;

    private MainHud mainHud;
    private GameCamera gameCamera;

    private bool isMainHudLoaded = false;

    private bool isDone = false;
    public bool IsDone
    {
        get { return this.isDone; }
    }
    #region Initialization
    public void Initialize()
    {
        this.mainCanvas = GameObject.FindGameObjectWithTag(TagNames.MAIN_CANVAS).GetComponent<Canvas>();
        this.gameCamera = GameObject.FindGameObjectWithTag(TagNames.MAIN_CAMERA).GetComponent<GameCamera>();
        this.gameCamera.Initialize();

        StartCoroutine(WaitForInitialization());
    }

    private IEnumerator WaitForInitialization()
    { 
        LoadMainHud();

        yield return new WaitUntil(() => { return this.isMainHudLoaded; });

        this.isDone = true;
    }

    public void LoadGameScene(Action onFinish = null)
    {
        StartCoroutine(LoadFirstScene(onFinish));
    }

    public void ReloadGame()
    {
        //Destroy(GameObject.Find("Root"));
        SceneManager.LoadSceneAsync(SceneNames.STARTUP_SCENE);
    }

    private IEnumerator LoadFirstScene(Action onFinish = null)
    {
        Scene unloadScene = SceneManager.GetActiveScene();
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneNames.FINAL, LoadSceneMode.Single);

        yield return new WaitUntil(() => { return asyncLoad.isDone; });
        //SceneManager.UnloadSceneAsync(unloadScene);

        onFinish?.Invoke();
        ToggleMainHud(true);
        MoveObjectToScene(this.gameCamera.gameObject, SceneManager.GetActiveScene());

        InputManager.Instance.ToggleAllowInput(true);
    }

    private void LoadMainHud()
    {
        GameObject mainHud = Resources.Load<GameObject>(MAIN_HUD_PATH);
        GameObject hud = Instantiate(mainHud, mainCanvas.transform);
        hud.SetActive(false);
        this.mainHud = hud.GetComponent<MainHud>();
        this.isMainHudLoaded = true;
    }
    #endregion


    /// <summary>
    /// Moves a Game Object DontDestroyOnLoad to a new scene
    /// </summary>
    /// <param name="obj">Game Object to move</param>
    /// <param name="scene">Scene to move the Game Object to</param>
    public void MoveObjectToScene(GameObject obj, Scene scene)
    {
        obj.transform.SetParent(null);
        SceneManager.MoveGameObjectToScene(obj, scene);
    }

    /// <summary>
    /// Toggles the Main Hud on or off
    /// </summary>
    /// <param name="active">Main Hud active value to be set. true or false</param>
    public void ToggleMainHud(bool active)
    {
        this.mainHud.gameObject.SetActive(active);
    }
}
