using System;
using System.Collections.Generic;
using DG.Tweening;
using Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class VFXManager : Instancable<VFXManager>
{
    [Header("Directional Light")]
    public Light directionalLight;
    public List<Light> sceneLights;
    public LightPreset originalLightPreset;
    public List<LightPreset> sceneLightPresets;
    public LightPreset redLightPreset;
    public float lightTweenTime = 1f;
    private Tween lightTween;
    private List<Tween> sceneLightTweens;
    private Tween colorTween;
    private List<Tween> sceneColorTweens;
    
    [Header("Health Bar")] 
    public Color preHealthBarColor;
    public Color postHealthBarColor;
    public Vector3 preHealthBarScale;
    public float hbTweenTime = 0.25f;

    [Header("Damage Indicator")] 
    public TextMeshProUGUI damageIndicator;
    public float indicatorBaseOffset;

    private void Start()
    {
        GetDefaultLightPresets();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            ChangeLight(); 
        }
    }

    public void IndicateDamage(int damage, Character receivingCharacter)
    {
        var indicator = Instantiate(
            damageIndicator, 
            receivingCharacter.hbs.GetComponent<RectTransform>().position + (Vector3.up * (indicatorBaseOffset + Random.Range(-0.5f,0.5f))) + (Vector3.right * Random.Range(-2f,2f)),
            Quaternion.Euler(45,0,0), 
            receivingCharacter.canvas);
        
        if(receivingCharacter == Character.myCharacter)
            indicator.color = Color.red;
        
        indicator.text = $"{damage}";
        
        DOVirtual.DelayedCall(1f, () => Destroy(indicator.gameObject));
    }
    
    public void DamageVFX(int damage, Character receivingCharacter)
    {
        ChangeLight();
        ChangeHealthBar(receivingCharacter);
        IndicateDamage(damage, receivingCharacter);
    }
    
    public void OpponentDamageVFX(int damage, Character receivingCharacter)
    {
        ChangeHealthBar(receivingCharacter);
        IndicateDamage(damage, receivingCharacter);
    }
    
    public void ChangeLight()
    {
        ChangeLightColor();
        ChangeLightIntensity();
    }

    private void ChangeLightColor()
    {
        // KillSetTween(colorTween);
        KillListTweens(sceneColorTweens);
        
        // directionalLight.color = redLightPreset.lightColor;
        sceneLights.ForEach(x => x.color = redLightPreset.lightColor);
        
        // colorTween = directionalLight.DOColor(originalLightPreset.lightColor, lightTweenTime);
        ChangeLightListValues(true);
    }
    
    private void ChangeLightIntensity()
    {
        // KillSetTween(lightTween);
        KillListTweens(sceneLightTweens);

        // directionalLight.intensity = redLightPreset.intensity;
        sceneLights.ForEach(x => x.intensity = redLightPreset.intensity);

        // lightTween = CreateLightIntensityTween(directionalLight, originalLightPreset);
        ChangeLightListValues(false);
    }

    private Tween CreateLightIntensityTween(Light light, LightPreset preset)
    {
        return DOTween.To(() => light.intensity, x => light.intensity = x,
            preset.intensity, lightTweenTime);
    }
    
    private Tween CreateColorTween(Light light, LightPreset preset)
    {
        return light.DOColor(preset.lightColor, lightTweenTime);
    }

    private void ChangeLightListValues(bool isColor)
    {
        for (int i = 0; i < sceneLights.Count; i++)
        {
            var newTween = isColor 
                ? CreateColorTween(sceneLights[i], sceneLightPresets[i]) 
                : CreateLightIntensityTween(sceneLights[i], sceneLightPresets[i]);
            // if (isColor)
            // {
            //     DOVirtual.DelayedCall(lightTweenTime * 1.05f, () =>
            //     {
            //         KillSetTween(newTween);
            //         newTween = CreateLightIntensityTween(sceneLights[i], sceneLightPresets[i]);
            //     });
            // }
            
            var tweenList = isColor ? sceneColorTweens : sceneLightTweens;

            if (tweenList.IsNullOrEmpty() || tweenList.Count <= i)
            {
                tweenList ??= new List<Tween>();
                tweenList.Add(newTween);
            }
            else
                tweenList[i] = newTween;
        }
    }

    public void ChangeHealthBar(Character character)
    {
        var healthBar = character.hbs;
        preHealthBarScale = healthBar.transform.localScale;
        var seq = DOTween.Sequence()
            .Append(healthBar.transform.DOScale(healthBar.transform.localScale * 1.5f, hbTweenTime))
            .Join(healthBar.transform.GetChild(3).gameObject.GetComponent<Image>()
                .DOColor(postHealthBarColor, hbTweenTime/2f)).OnComplete(() =>
            {
                healthBar.transform.DOScale(preHealthBarScale, hbTweenTime/2f);
                healthBar.transform.GetChild(3).gameObject.GetComponent<Image>()
                    .DOColor(preHealthBarColor, hbTweenTime/2f);
            });
    }

    private void KillSetTween(Tween tween)
    {
        if(tween != null) tween.Kill();
    }

    private void KillListTweens(List<Tween> tweens)
    {
        tweens?.ForEach(KillSetTween);
    }
    
    private void GetDefaultLightPresets()
    {
        if (sceneLightPresets.IsNullOrEmpty())
        {
            foreach (var sceneLight in sceneLights)
            {
                sceneLightPresets.Add(new LightPreset{intensity = sceneLight.intensity, lightColor = sceneLight.color});
            }
        }
    }
}

[Serializable]
public class LightPreset
{
    public Color lightColor;
    public float intensity;
}
