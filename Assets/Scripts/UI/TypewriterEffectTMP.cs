using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TypewriterEffectTMP : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI targetText;
    [SerializeField] float characterDelay;

    private string referenceText;

    Coroutine typewriterCoroutine;
    
    private bool isWriting;
    public bool IsWriting
    {
        get { return this.isWriting; }
    }

    private Action onFinishEffect;

    public void StartEffect(Action onFinish = null)
    {
        this.targetText.gameObject.SetActive(false);
        this.referenceText = targetText.text;
        this.targetText.text = "";
        this.typewriterCoroutine = null;
        this.isWriting = false;
        this.onFinishEffect = onFinish;
        this.typewriterCoroutine = StartCoroutine(StartTypeWriter());
    }

    public void StopTypewriterEffect()
    {
        this.targetText.text = this.referenceText;
        StopCoroutine(this.typewriterCoroutine);
        this.typewriterCoroutine = null;
        this.isWriting = false;
        this.onFinishEffect?.Invoke();
    }

    private IEnumerator StartTypeWriter()
    {
        this.targetText.gameObject.SetActive(true);
        this.isWriting = true;
        foreach (char c in referenceText)
        {
            this.targetText.text += c;
            yield return new WaitForSeconds(characterDelay);
        }
        this.onFinishEffect?.Invoke();
        this.isWriting = false;
    }
}