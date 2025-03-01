using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightOnAudio : MonoBehaviour
{
    // Start is called before the first frame update
    public int _band;
    public float _minIntensity, _maxIntensity;
    Light _light;
    public AudioPeer _audioPeer;
    void Start()
    {
        _light = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        _light.intensity = (_audioPeer._audioBandBuffer[_band] * (_maxIntensity - _minIntensity)) + _minIntensity;
    }
}
