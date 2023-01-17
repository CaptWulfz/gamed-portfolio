using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>, ISingleton
{
    private const string DARK_OVERLAY_PATH = "Prefabs/DarkOverlay";

    [SerializeField] private GameCamera gameCamera;
    private DarkOverlay darkOverlay;
    private Player player;


    private bool isDone = false;
    public bool IsDone
    {
        get { return this.isDone; }
    }

    public void Initialize()
    {
        this.gameCamera = GameObject.FindGameObjectWithTag(TagNames.MAIN_CAMERA).GetComponent<GameCamera>();
        LoadDarkOverlay();
        this.isDone = true;
    }

    private void LoadDarkOverlay()
    {
        GameObject loadedOverlay = Resources.Load<GameObject>(DARK_OVERLAY_PATH);
        GameObject overlayObj = Instantiate(loadedOverlay, this.transform);
        this.darkOverlay = overlayObj.GetComponent<DarkOverlay>();
    }

    public void RegisterPlayerToGameManager(Player player)
    {
        this.player = player;
    }

    public void MakeGameCameraFollowTarget(Transform transform)
    {
        this.gameCamera.FollowTarget(transform);
    }

    public void RecallCamera()
    {
        DontDestroyOnLoad(this.gameCamera);
    }
}
