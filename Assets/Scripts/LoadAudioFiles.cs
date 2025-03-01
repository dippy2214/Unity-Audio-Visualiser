using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.Networking;

public class LoadAudioFiles : MonoBehaviour
{
    public string _audioPath = "Assets/Resources/Audio";
    public AudioSource _audioSource;
    public TMP_Dropdown _dropdown;
    public UnityEngine.UI.Slider _slider;
    public RawImage _pausePlayIcon;
    public GameObject _canvas;
    string filePath = @"C:\Program Files\AudioVisualiser\Audio";
    string myDocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
    List<AudioClip> clips = new List<AudioClip>();
    // Start is called before the first frame update
    void Start()
    {
        LoadAudio();
    }

    void LoadAudio()
    {
        StartCoroutine(GetAudioClips(filePath, clips));
        _dropdown.ClearOptions();
        
        //AudioClip[] _mp3Files = Resources.LoadAll<AudioClip>("Audio");
        
        _dropdown.onValueChanged.AddListener(OnDropdownChange);
        _dropdown.value = 0;
    }

    IEnumerator GetAudioClips(string fileName,List<AudioClip> audioClips)
    {
        string folderPath = Path.Combine(myDocumentsPath, "AudioVisualiserMusic");

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        

        string[] audioFiles = Directory.GetFiles(folderPath, "*.mp3");
        List<string> _options = new List<string>();

        foreach (string file in audioFiles)
        {
            UnityWebRequest webRequest = UnityWebRequestMultimedia.GetAudioClip(
                "file://"+ file, AudioType.MPEG);
         
            yield return webRequest.SendWebRequest();
 
            if(webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                
                Debug.Log(webRequest.error);
            }
            else
            {
                AudioClip clip = DownloadHandlerAudioClip.GetContent(webRequest);
                clip.name = Path.GetFileNameWithoutExtension(file);
                audioClips.Add(clip);
 
            }
        }
        foreach (AudioClip clip in audioClips)
        {
            _options.Add(returnFirst20Chars(clip.name));
            
            //Debug.Log("found mp3 file: " + clip.name);
        }

        _dropdown.AddOptions(_options);
    }

    string returnFirst20Chars(string input)
    {
        return input.Length > 20 ? input.Substring(0, 20) : input;
    }
    void OnDropdownChange(int index)
    {
        string mp3AudioName = _dropdown.options[index].text;
        AudioClip _mp3Clip = clips[index];
        _audioSource.clip = _mp3Clip;
        _audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        //_dropdown.options.FindIndex(option => option.text == _audioSource.clip.name);
        _audioSource.volume = _slider.value/100.0f;
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (_audioSource.isPlaying)
            {
                _audioSource.Pause();
                _pausePlayIcon.uvRect = new Rect(0.0f, 0.25f, 0.5f, 0.5f);

            }
            else
            {
                _audioSource.UnPause();
                _pausePlayIcon.uvRect = new Rect(0.5f, 0.25f, 0.5f, 0.5f);

            }
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            _canvas.SetActive(!_canvas.activeInHierarchy);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        
    }
}
