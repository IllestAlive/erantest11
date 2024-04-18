using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBarSelect : MonoBehaviour
{
    public GameObject fillRed, fillGreen;
    public GameObject greenEllipse, redEllipse;
    public TextMeshProUGUI nameText;
    public Color normalColor, midColor, lowColor;
    public float flashTime;
    public bool isMyHealthBar;

    private Image fillGreenImage;
    
    private Sequence midFlash, lowFlash;
    

    public void InitializeHealthBar(bool isMine)
    {
        isMyHealthBar = isMine;
        if (isMine)
        {
            fillGreen.SetActive(true);
            fillRed.SetActive(false);
            GetComponent<Slider>().fillRect = fillGreen.GetComponent<RectTransform>();
            
            redEllipse.SetActive(false);
            greenEllipse.SetActive(true);
            nameText.text = "RAGNVALD";
        }
        else
        {
            fillGreen.SetActive(false);
            fillRed.SetActive(true);
            GetComponent<Slider>().fillRect = fillRed.GetComponent<RectTransform>();
            
            greenEllipse.SetActive(false);
            redEllipse.SetActive(true);
            nameText.text = "ERLING";
        }
    }

    public void CheckHealthStaus()
    {
        if(!isMyHealthBar) return;
        var slider = GetComponent<Slider>();

        if (slider.value <= 0.5f)
        {
            if(slider.value <= 0.3f) ColorFlashLow();
            else ColorFlashMid();
        }
        else
        {
            SetNormalColor();
        }
    }

    public void ColorFlashMid()
    {
        SetNormalColor();
        if(!fillGreenImage) return;

        midFlash = DOTween.Sequence()
            .Append(fillGreenImage.DOColor(midColor, flashTime / 2))
            .Append(fillGreenImage.DOColor(normalColor, flashTime / 2))
            .SetLoops(-1, LoopType.Restart);
    }

    public void ColorFlashLow()
    {
        SetNormalColor();
        if(!fillGreenImage) return;
        
        lowFlash = DOTween.Sequence()
            .Append(fillGreenImage.DOColor(midColor, flashTime / 3))
            .Append(fillGreenImage.DOColor(lowColor, flashTime / 3))
            .Append(fillGreenImage.DOColor(normalColor, flashTime / 3))
            .SetLoops(-1, LoopType.Restart);
    }

    private void KillTweens()
    {
        if(midFlash != null) midFlash.Kill();
        if(lowFlash != null) lowFlash.Kill();
    }

    private void SetNormalColor()
    {
        fillGreenImage ??= fillGreen.GetComponent<Image>();
        if(!fillGreenImage) return;
        
        KillTweens();
        fillGreenImage.color = normalColor;
    }
}
