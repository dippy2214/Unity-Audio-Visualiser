using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AmpSphere : MonoBehaviour
{
    public float _startScale, _maxScale;
    public bool _useBuffer;
    Material _material;
    public float _red, _green, _blue;
    // Start is called before the first frame update
    void Start()
    {
        _material = GetComponent<MeshRenderer>().materials[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (!_useBuffer)
        {
            transform.localScale = new Vector3 ((AudioPeer._amplitude * _maxScale) + _startScale, (AudioPeer._amplitude * _maxScale) + _startScale,(AudioPeer._amplitude * _maxScale) + _startScale);
            Color _color = new Color (_red * AudioPeer._amplitude, _green * AudioPeer._amplitude, _blue * AudioPeer._amplitude);
            _material.SetColor("_EmissionColor", _color);
        }
        if (_useBuffer)
        {
            transform.localScale = new Vector3 ((AudioPeer._amplitudeBuffer * _maxScale) + _startScale, (AudioPeer._amplitudeBuffer * _maxScale) + _startScale,(AudioPeer._amplitudeBuffer * _maxScale) + _startScale);
            Color _color = new Color (_red * AudioPeer._amplitudeBuffer, _green * AudioPeer._amplitudeBuffer, _blue * AudioPeer._amplitudeBuffer);
            _material.SetColor("_EmissionColor", _color);
        }
    }
}
