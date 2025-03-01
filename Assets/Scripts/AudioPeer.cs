using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;


[RequireComponent (typeof (AudioSource))]
public class AudioPeer : MonoBehaviour
{
    AudioSource _audioSource;
    private float[] _samplesLeft = new  float[512];
    private float[] _samplesRight = new  float[512];

    private float[] _freqBands = new float[8];
    private float[] _bandBuffer = new float[8];
    private float[] _bufferDecrease = new float[8];
    private float[] _freqBandHighest = new float[8];

    //audio 64 channel values
    private float[] _freqBands64 = new float[64];
    private float[] _bandBuffer64 = new float[64];
    private float[] _bufferDecrease64 = new float[64];

    private float[] _freqBandHighest64 = new float[64];

    [HideInInspector]
    public float[] _audioBand, _audioBandBuffer;
    public float[] _audioBand64, _audioBandBuffer64;

    //[HideInInspector]
    public static float _amplitude, _amplitudeBuffer;
    private float _amplitudeHighest;
    public float _audioProfileFloat;
    public float _audioProfileFloat64;


    public enum _channel {Stereo, Left, Right};
    public _channel channel = new _channel();


    // Start is called before the first frame update
    void Start()
    {
        _audioBand = new float[8];
        _audioBandBuffer = new float[8];
        _audioBand64 = new float[64];
        _audioBandBuffer64 = new float[64];
        _audioSource = GetComponent<AudioSource>();
        AudioProfile(_audioProfileFloat);
        AudioProfile64(_audioProfileFloat64);
    }

    // Update is called once per frame
    void Update()
    {
        GetSpectrumAudioSource();
        MakefrequencyBands();
        BandBuffer();
        CreateAudioBands();
        GetAmplitude();

        MakefrequencyBands64();
        BandBuffer64();
        CreateAudioBands64();

    }
    void AudioProfile(float audioprofile)
    {
        for (int i = 0; i < 8; i++)
        {
            _freqBandHighest[i] = audioprofile;
        }
        _amplitudeHighest = audioprofile;
    }

    void AudioProfile64(float audioprofile64)
    {
        for (int i = 0; i < 64; i++)
        {
            _freqBandHighest64[i] = audioprofile64;
        }
    }
    void GetAmplitude()
    {
        float _CurrentAmplitude = 0;
        float _CurrentAmplitudeBuffer = 0;

        for (int i = 0; i < 8; i++)
        {
            _CurrentAmplitude += _audioBand[i];
            _CurrentAmplitudeBuffer += _audioBandBuffer[i];
        }
        if (_CurrentAmplitude > _amplitudeHighest)
        {
            _amplitudeHighest = _CurrentAmplitude;
        }
        _amplitude = _CurrentAmplitude / _amplitudeHighest;
        _amplitudeBuffer = _CurrentAmplitudeBuffer / _amplitudeHighest;
    }
    void CreateAudioBands()
    {
        for (int i = 0; i < 8; i++)
        {
            if (_freqBands[i] > _freqBandHighest[i])
            {
                _freqBandHighest[i] = _freqBands[i];
            }
            _audioBand[i] = (_freqBands[i] / _freqBandHighest[i]);
            _audioBandBuffer[i] = (_bandBuffer[i] / _freqBandHighest[i]);
        }
    }

    void CreateAudioBands64()
    {
        for (int i = 0; i < 64; i++)
        {
            if (_freqBands64[i] > _freqBandHighest64[i])
            {
                _freqBandHighest64[i] = _freqBands64[i];
            }
            _audioBand64[i] = (_freqBands64[i] / _freqBandHighest64[i]);
            _audioBandBuffer64[i] = (_bandBuffer64[i] / _freqBandHighest64[i]);
        }
    }
    void GetSpectrumAudioSource()
    {
        _audioSource.GetSpectrumData(_samplesLeft, 0, FFTWindow.Blackman);
        _audioSource.GetSpectrumData(_samplesRight, 1, FFTWindow.Blackman);

    }

    void BandBuffer()
    {
        for (int i = 0; i < 8; i++)
        {
            if (_freqBands[i] > _bandBuffer[i])
            {
                _bandBuffer[i] = _freqBands[i];
                _bufferDecrease[i] = 0.005f;
            }
            if (_freqBands[i] < _bandBuffer[i])
            {
                _bandBuffer[i] -= _bufferDecrease[i];
                _bufferDecrease[i] *= 1.2f;
            }
        }
    }

    void BandBuffer64()
    {
        for (int i = 0; i < 64; i++)
        {
            if (_freqBands64[i] > _bandBuffer64[i])
            {
                _bandBuffer64[i] = _freqBands64[i];
                _bufferDecrease64[i] = 0.005f;
            }
            if (_freqBands64[i] < _bandBuffer64[i])
            {
                _bandBuffer64[i] -= _bufferDecrease64[i];
                _bufferDecrease64[i] *= 1.2f;
            }
        }
    }
    void MakefrequencyBands()
    {
        /*
        22050 / 512 = 43 hertz per sample
        20 - 60 hertz
        60 - 250 hertz
        250 - 500 hertz
        500 - 2000 hertz
        4000 - 6000 hertz
        6000 - 20000 hertz

        0 - 2 samples = 86 hertz
        1 - 4 samples = 172 hertz -> 87 - 258 hertz range
        2 - 8 = 344 hertz -> 259-602
        3 - 16
        4 - 32
        5 - 64
        6 - 128
        7 - 256

        510 total - 512 samples - add 2 to last one
        */

        int count = 0;

        for (int i = 0; i < 8; i++) 
        {
            float average = 0;
            int sampleCount = (int)Mathf.Pow(2, i) * 2;

            if (i == 7)
            {
                sampleCount += 2;
            }
            for (int j = 0; j < sampleCount; j++)
            {
                if (channel == _channel.Stereo)
                {
                    average += _samplesLeft[count] + _samplesRight[count] * (count + 1);
                }
                if (channel == _channel.Left)
                {
                    average += _samplesLeft[count] * (count + 1);
                }
                if (channel == _channel.Right)
                {
                    average += _samplesRight[count] * (count + 1);
                }
                count++;
            }
            average /= count;

            _freqBands[i] = average * 10;
        }
    }

    void MakefrequencyBands64()
    {
        /*
        0-15 - 1 sample each
        16-31 - 2 samples
        32-39 - 4 samples
        40-47 - 6 sammples
        48-55 - 16 samples
        56-63 - 32 samples

        512 total samples used
        */
        int count = 0;
        int sampleCount = 1;
        int power = 0;
        for (int i = 0; i < 64; i++) 
        {
            float average = 0;

            if (i == 16 || i == 32 || i == 40 || i == 48 || i == 56)
            {
                power ++;
                sampleCount = (int)Mathf.Pow (2, power);
                if (power == 3)
                {
                    sampleCount -= 2;
                }
            }
            for (int j = 0; j < sampleCount; j++)
            {
                if (channel == _channel.Stereo)
                {
                    average += _samplesLeft[count] + _samplesRight[count] * (count + 1);
                }
                if (channel == _channel.Left)
                {
                    average += _samplesLeft[count] * (count + 1);
                }
                if (channel == _channel.Right)
                {
                    average += _samplesRight[count] * (count + 1);
                }
                count++;
            }
            average /= count;

            _freqBands64[i] = average * 8;
        }
    }
}
