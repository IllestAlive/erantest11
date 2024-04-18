using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartManagerInGame : Instancable<StartManagerInGame>
{
    public GameObject zeroScreen, firstScreen, secondScreen, thirdScreen;
    public GameObject leftCharHolder, rightCharHolder;
    public GameObject ragnvald, erling, vs;
    public bool canStart;
    // public GameObject ; // 90 -> 21.1f
    public CinemachineVirtualCamera virtualCam;
    public CinemachineFramingTransposer framingTransposer;
    public Image loadingBar;
    public TextMeshProUGUI loadingText;

    private float camFinalY = 23.3f, camFinalZ = -20.6f;

    private float camRotateFinalX = -42f;
    private float zoomInTime = 8f;

    
    private float rotateTime = 4f;
    private void Start()
    {
        
        framingTransposer = virtualCam.GetCinemachineComponent<CinemachineFramingTransposer>();
        
        canStart = true;

        StartCoroutine(canStartChange());
        IEnumerator canStartChange()
        {
            yield return new WaitForSeconds(1f);
            canStart = false;
        }
        
        zeroScreen.SetActive(true);
        float loading = 0;
        DOTween.To(() => loading, x => loading = x, 100, 7)
            .OnUpdate(() =>
            {
                int loadingInt = Convert.ToInt16(loading);
                loadingBar.transform.localScale = new Vector3(loading / 100f, 1, 1);
                loadingText.text = "%" + loadingInt + " LOADING...";
            }).OnComplete(() =>
            {
                leftCharHolder.SetActive(false);
                rightCharHolder.SetActive(false);
                loadingBar.transform.parent.gameObject.SetActive(false);
                loadingText.gameObject.SetActive(false);
                zeroScreen.GetComponent<Image>().DOFade(0, 1).OnComplete(() =>
                {
                    var value = framingTransposer.m_TrackedObjectOffset.y;
                    DOTween.To(() => value, x => value = x, camFinalY, zoomInTime)
                        .OnUpdate(() =>
                            framingTransposer.m_TrackedObjectOffset =
                                new Vector3(0, value, camFinalZ));

                    virtualCam.transform.DORotate(new Vector3(camRotateFinalX, 0, 0), rotateTime, RotateMode.FastBeyond360).SetRelative().SetEase(Ease.Linear);
                    zeroScreen.SetActive(false);
                    firstScreen.SetActive(true);
                    firstScreen.GetComponent<Image>().DOFade(0, 1).SetDelay(3f).OnComplete(() =>
                    {
                        firstScreen.SetActive(false);
                        secondScreen.SetActive(true);
                        ragnvald.transform.GetComponent<RectTransform>().DOAnchorPosX(0, 2f);
                        erling.transform.GetComponent<RectTransform>().DOAnchorPosX(0, 2f);
                        vs.transform.DOScale(1f, 3f).OnComplete(() =>
                        {
                            thirdScreen.SetActive(true);
                            vs.gameObject.SetActive(false);
                            thirdScreen.transform.DOScale(1f,1f).OnComplete(() =>
                            {
                                thirdScreen.SetActive(false);
                                canStart = true;
                                gameObject.SetActive(false);
                                UIManager.Instance.opponentHp.gameObject.SetActive(true);
                            }).OnStart(() =>
                            {
                                secondScreen.SetActive(false);
                            }).SetDelay(1f).SetEase(Ease.Linear);
                        });

                    });
                });
            });
    }
}
