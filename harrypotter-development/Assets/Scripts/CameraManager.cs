using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using UnityEngine;

public class CameraManager : Instancable<CameraManager>
{
    public CinemachineVirtualCamera virtualCamera;
    public bool cameraShake;
    private CinemachineBasicMultiChannelPerlin virtualPerlin;

    private Dictionary<ShakeIntensity, float> intensityDictionary = new()
    {
        {ShakeIntensity.Low, 1},
        {ShakeIntensity.Mid, 2},
        {ShakeIntensity.High, 4}
    };

    private Dictionary<ShakeTime, float> timeDictionary = new()
    {
        {ShakeTime.Instant, 0.1f},
        {ShakeTime.Quick, 0.2f},
        {ShakeTime.Normal, 0.5f},
        {ShakeTime.Slow, 1f}
    };

    public void SetFollowObject(Transform myTransform)
    {
        virtualCamera.Follow = myTransform;
    }

    public void ShakeCamera(ShakeIntensity intensity, ShakeTime time, bool smooth = false)
    { 
        if(!cameraShake) return;
        SetCameraNoiseAmplitude(intensityDictionary[intensity]);
        if (smooth)
        {
            DOTween.To(() => virtualPerlin.m_AmplitudeGain, x => virtualPerlin.m_AmplitudeGain = x, 0, timeDictionary[time])
                .SetEase(Ease.Linear);
        }
        else
        {
            DOVirtual.DelayedCall(timeDictionary[time], () => { SetCameraNoiseAmplitude(0.0f); });
        }
    }
    
    public void ShakeCamera(float intensity, float time, bool smooth = false)
    {
        if(!cameraShake) return;
        
        SetCameraNoiseAmplitude(intensity);
        if (smooth)
        {
            DOTween.To(() => virtualPerlin.m_AmplitudeGain, x => virtualPerlin.m_AmplitudeGain = x, 0, time)
                .SetEase(Ease.Linear);
        }
        else
        {
            DOVirtual.DelayedCall(time, () => { SetCameraNoiseAmplitude(0f); });
        }
    }

    private void SetCameraNoiseAmplitude(float amplitude)
    {
        virtualPerlin ??= virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        virtualPerlin.m_AmplitudeGain = amplitude;
    }
}

public enum ShakeIntensity
{
    Low,
    Mid,
    High
}

public enum ShakeTime
{
    Instant,
    Quick,
    Normal,
    Slow
}
