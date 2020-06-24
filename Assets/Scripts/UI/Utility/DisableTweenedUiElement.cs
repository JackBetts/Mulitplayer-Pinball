using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DisableTweenedUiElement : MonoBehaviour
{
    private DOTweenAnimation tween;

    private void Start()
    {
        tween = GetComponent<DOTweenAnimation>(); 
    }

    public void DisableElement()
    {
        tween = GetComponent<DOTweenAnimation>(); 
        if (!tween) return;

        tween.onRewind.AddListener(OnRewind);
        
        tween.DOPlayBackwards();
    }

    private void OnRewind()
    {
        gameObject.SetActive(false);
    }
}
