using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkOverlay : MonoBehaviour
{
    private const string SHOW = "ShowDarkOverlay";
    private const string HIDE = "HideDarkOverlay";

    [SerializeField] private Animation anim;

    public void Initialize()
    {
        this.gameObject.SetActive(false);
    }

    public void ShowOverlay(Vector3 spawnPos)
    {
        this.gameObject.transform.position = spawnPos;
        this.gameObject.SetActive(true);
        this.anim.Play(SHOW);
        StartCoroutine(AnimationHandler.WaitForAnimation(this.anim, () => 
        { 
        
        }));
    }

    public void HideOverlay()
    {
        this.anim.Play(HIDE);
        StartCoroutine(AnimationHandler.WaitForAnimation(this.anim, () =>
        {
            this.gameObject.SetActive(false);
        }));
    }
}
