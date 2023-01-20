using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiWorldMapManager : MonoBehaviour
{
    public static UiWorldMapManager instance;
    public Image bgToFade;

    private void Awake()
    {
        instance = this;
    }
    
    public void FadeInAndLoadLvl(string lvlname)
    {
        bgToFade.DOFade(1,1f).OnComplete((() => SceneManager.LoadScene(lvlname)));
    }
}
