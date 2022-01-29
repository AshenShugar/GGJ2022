using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }

    private CinemachineVirtualCamera cam;
    private float shakeTimer;

    private float shakeTimerTotal;
    private float startingIntensity;

    void Awake()
    {
        Instance = this;
        cam = GetComponent<CinemachineVirtualCamera>();
    }

    public void ShakeCamera(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin camNoise = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        camNoise.m_AmplitudeGain = intensity;

        //TURN THESE ON FOR SMOOTHER COOLDOWN
        //startingIntensity = intensity;
        //shakeTimerTotal = time;

        shakeTimer = time;
    }


    // Update is called once per frame
    void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0)
            {
                CinemachineBasicMultiChannelPerlin camNoise = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                camNoise.m_AmplitudeGain = 0;

                //SAME WITH THIS
                //camNoise.m_AmplitudeGain = Mathf.Lerp(startingIntensity, 0, 1-(shakeTimer/shakeTimerTotal));
            }
        }
    }
}
