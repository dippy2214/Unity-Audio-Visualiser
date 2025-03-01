using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.UI;

public class ParamCube : MonoBehaviour
{
    public int _band;
    float _startScale = 1, _scaleMultiplier = 5;
    public bool _useBuffer;
    public bool _64band = false;
    Material _material;
    public float _shiftspeed = 5f;
    float _colorshift = 0f;
    public AudioPeer _audioPeer;

    public Toggle _toggle;
    public TMP_Dropdown _colDrop;
    public float radius = 2;

    int _colorIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        _audioPeer = FindAnyObjectByType<AudioPeer>();
        _material = GetComponent<MeshRenderer>().materials[0];

        //_material = GetComponent<Material>();
        _toggle.onValueChanged.AddListener(OnToggleTrigger);

        _colDrop.onValueChanged.AddListener(colorlistener);
    }

    // Update is called once per frame
    void Update()
    {
        if (!_64band)
        {
            if (_useBuffer)
            {
                transform.localScale = new Vector3(transform.localScale.x, (_audioPeer._audioBandBuffer[_band] * _scaleMultiplier) + _startScale, transform.localScale.z);
                Color _color = new Color (_audioPeer._audioBandBuffer[_band], _audioPeer._audioBandBuffer[_band],_audioPeer._audioBandBuffer[_band]);
                _material.SetColor("_EmissionColor", _color);
            }
            if (!_useBuffer)
            {
                transform.localScale = new Vector3(transform.localScale.x, (_audioPeer._audioBand[_band] * _scaleMultiplier) + _startScale, transform.localScale.z);
                Color _color = new Color (_audioPeer._audioBand[_band], _audioPeer._audioBand[_band],_audioPeer._audioBand[_band]);
                _material.SetColor("_EmissionColor", _color);
            }
        }
        if (_64band)
        {
            if (_band < 64)
            {
            if (_useBuffer)
            {
                transform.localScale = new Vector3(transform.localScale.x, (_audioPeer._audioBandBuffer64[_band] * _scaleMultiplier) + _startScale, transform.localScale.z);
                Color _color = new Color (_audioPeer._audioBandBuffer64[_band], _audioPeer._audioBandBuffer64[_band],_audioPeer._audioBandBuffer64[_band]);
                _material.SetColor("_EmissionColor", _color);
            }
            if (!_useBuffer)
            {
                transform.localScale = new Vector3(transform.localScale.x, (_audioPeer._audioBand64[_band] * _scaleMultiplier) + _startScale, transform.localScale.z);
                Color _color = new Color (_audioPeer._audioBand64[_band], _audioPeer._audioBand64[_band],_audioPeer._audioBand64[_band]);
                _material.SetColor("_EmissionColor", _color);
            }

            _colorshift += Time.deltaTime * 10f;

            if (_colorshift > 64.0f)
            {
                _colorshift -= 64f;
            }
            
            UpdateColor(_colorIndex);
        }
        else if (_band == 64)
        {
            transform.localScale = new Vector3(transform.localScale.x, (_audioPeer._audioBandBuffer64[_band - 1] * _scaleMultiplier)/2 + _startScale, transform.localScale.z);
            Color _color = new Color (_audioPeer._audioBandBuffer64[_band-1]/2, _audioPeer._audioBandBuffer64[_band-1]/2,_audioPeer._audioBandBuffer64[_band-1]/2);
            _material.SetColor("_EmissionColor", _color);
            
            _colorshift += Time.deltaTime * 10f;
            
            if (_colorshift > 64.0f)
            {
                _colorshift -= 64f;
            }
            
            UpdateColor(_colorIndex);
        }
        else if (_band == 65)
        {
            transform.localScale = new Vector3(transform.localScale.x, (_audioPeer._audioBandBuffer64[0] * _scaleMultiplier)/2 + _startScale, transform.localScale.z);
            Color _color = new Color (_audioPeer._audioBandBuffer64[0]/2, _audioPeer._audioBandBuffer64[0]/2,_audioPeer._audioBandBuffer64[0]/2);
            _material.SetColor("_EmissionColor", _color);
            
            _colorshift += Time.deltaTime * 10f;

            if (_colorshift > 64.0f)
            {
                _colorshift -= 64f;
            }
            
            UpdateColor(_colorIndex);
        }
        
        }
    }

    void OnToggleTrigger(bool on)
    {
        Transform trans = GetComponent<Transform>();
        if (on)
        {
            //trans.LookAt(new Vector3(0, 0, 0));
            float angle = ((360.0f/66.0f)*_band)+90;

            float x = radius * Mathf.Cos(Mathf.Deg2Rad*angle);
            float y = radius * Mathf.Sin(Mathf.Deg2Rad*angle);
            
            trans.Rotate(new Vector3(0, 0, angle + 90));
            trans.position = (new Vector3(x, y, 0));
            //trans.LookAt(new Vector3(0, 0, 0));
        }
        else
        {
            //trans.Rotate(new Vector3(0, 0, 0));
            if (_band == 65)
            {
                trans.rotation = Quaternion.identity;
                float x = (0 - (33f+0.5f))/66f * 6.0f *_scaleMultiplier;
                trans.position = (new Vector3(x, 0, 0));
            }
            else
            {
                trans.rotation = Quaternion.identity;
                float x = ((_band+1) - (33f+0.5f))/66f * 6.0f *_scaleMultiplier;
                trans.position = (new Vector3(x, 0, 0));
                //trans.LookAt(new Vector3(0, 0, -25));
            }
        }
    }

    void colorlistener(int index)
    {
        _colorIndex = index;
    }

    void UpdateColor(int index)
    {
        if (index == 0)
        {
            float colstore = _band;
            colstore += _colorshift;
            colstore = colstore % 66;
            float _Rcol = Mathf.Abs((colstore-33f)/33f);
            float _Gcol = 1f-Mathf.Abs((colstore-33f)/33f) - 0.1f;
            float _Bcol = 1f-Mathf.Abs((colstore-33f)/33f) + 0.2f;
            float _Bcol2 = Mathf.Abs((colstore-33f)/33f);
            if (_Bcol2 > _Bcol)
            {
                _Bcol = _Bcol2;
            }

            Color _Bcolor = new Color (_Rcol, _Gcol, _Bcol);
               
            _material.SetColor("_Color", _Bcolor);
        }
        else if (index == 1)
        {
            float colstore = _band;
            colstore += _colorshift;
            colstore = colstore % 66;
            float _Rcol = Mathf.Abs((colstore-33f)/33f);
            float _Gcol = 1f-Mathf.Abs((colstore-33f)/33f) - 0.1f;
            float _Bcol = 1f-Mathf.Abs((colstore-33f)/33f) + 0.2f;
            float _Bcol2 = Mathf.Abs((colstore-33f)/33f);
            if (_Bcol2 > _Bcol)
            {
                _Bcol = _Bcol2;
            }

            Color _Bcolor = new Color (0.2f, _Bcol, _Gcol);
               
            _material.SetColor("_Color", _Bcolor);
        }
        else if (index == 2)
        {
            float colstore = _band;
            colstore += _colorshift;
            colstore = colstore % 66;
            float _Rcol = Mathf.Abs((colstore-33f)/33f);
            float _Gcol = 1f-Mathf.Abs((colstore-33f)/33f) - 0.1f;
            float _Bcol = 1f-Mathf.Abs((colstore-33f)/33f) + 0.2f;
            float _Bcol2 = Mathf.Abs((colstore-33f)/33f);
            if (_Bcol2 > _Bcol)
            {
                _Bcol = _Bcol2;
            }

            Color _Bcolor = new Color (1f, _Gcol, 0.2f);
               
            _material.SetColor("_Color", _Bcolor);
        }
    }
}

