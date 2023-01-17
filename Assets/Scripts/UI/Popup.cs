using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup : MonoBehaviour
{
    [Header("Popup Animation")]
    [SerializeField] Animation anim;

    /// <summary>
    /// Overlay object in the popup. Serves to make sure that anything behind the Popup is not clickable
    /// </summary>
    [Header("Overlay")]
    [SerializeField] GameObject overlay;

    protected string POPUP_OPEN = "PopupOpen";
    protected string POPUP_CLOSE = "PopupClose";

    protected Action onShowStart;
    protected Action onShowFinished;

    protected Action onHideStart;
    protected Action onHideFinished;

    /// <summary>
    /// Shows the Popup. Enables both the game object and the Overlay.
    /// </summary>
    public virtual void Show()
    {
        this.onShowStart?.Invoke();
        this.overlay.SetActive(true);
        this.gameObject.SetActive(true);
        StartCoroutine(AnimationHandler.WaitForAnimation(this.anim, () =>
        {
            onShowFinished?.Invoke();
        }));
    }

    /// <summary>
    /// Hides the Popup. Plays the Hide Animation first before destroying the Popup
    /// </summary>
    protected virtual void Hide()
    {
        this.onHideStart?.Invoke();
        this.anim.Play(POPUP_CLOSE);
        StartCoroutine(AnimationHandler.WaitForAnimation(this.anim, () =>
        {
            onHideFinished?.Invoke();
            PopupManager.Instance.HidePopup(this.gameObject);
        }));
    }

    /// <summary>
    /// Plays an Animation for the Popup
    /// </summary>
    /// <param name="animName">Name of the Animation to Play</param>
    /// <param name="onFinish">Action to invoke after finishing the animation</param>
    protected virtual void PlayAnim(string animName, Action onFinish = null)
    {
        this.anim.Play(animName);
        StartCoroutine(AnimationHandler.WaitForAnimation(this.anim, () =>
        {
            onFinish?.Invoke();
        }));
    }

    /// <summary>
    /// Function to be called when the user interacts with the Close Button
    /// </summary>
    public virtual void OnCloseButton()
    {
        Hide();
    }
}
