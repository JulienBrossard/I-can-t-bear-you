using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScenePrewarmer : MonoBehaviour
{
    private GameObject player;
    [SerializeField] private float warmTime = 2;
    [SerializeField] private float timeScaleWhilePrewarming = 10;
    [SerializeField] private Image bgToFade;
    [SerializeField] private TextMeshProUGUI txtToFade;

    void Start()
    {
        player = PlayerStateManager.instance.gameObject;
        player.SetActive(false);
        StartCoroutine(ActivatePlayer());
        Time.timeScale = timeScaleWhilePrewarming;

    }


    IEnumerator ActivatePlayer()
    {
        yield return new WaitForSeconds(warmTime);
        player.SetActive(true);
        Time.timeScale = 1;
        bgToFade.DOFade(0, 1).OnComplete(()=> bgToFade.gameObject.SetActive(false));
        txtToFade.DOFade(0, 1);
    }
}
