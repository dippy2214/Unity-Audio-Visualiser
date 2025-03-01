using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using System.Reflection;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class NewBehaviourScript : MonoBehaviour
{
    Mesh _mesh;
    Vector3[] _verts;
    int[] _tries;

    public int _xsize = 20;
    public int _zsize = 20;

    public float _downSpeed = 1.0f;
    public float _timeToMove = 0.1f;
    public AudioPeer _audioPeer;


    // Start is called before the first frame update
    void Start()
    {
        _mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = _mesh;
        _mesh.name = "GeneratedMesh";

        StartCoroutine(CreateShape());
        UpdateMesh();
    }

    IEnumerator CreateShape()
    {
        _verts = new Vector3[(_xsize + 1) * (_zsize + 1)];

        for (int index = 0, z = 0; z < _zsize + 1; z++)
        {
            for (int x = 0; x < _xsize + 1; x++)
            {
                _verts[index] = new Vector3(x, AudioPeer._amplitudeBuffer, z);
                index++;
            }
        }
    
        int _vert = 0;
        int _tri = 0;
        _tries = new int[_xsize * _zsize * 6];

        for (int z = 0; z < _zsize; z++)
        {
            for (int x = 0; x < _xsize; x++)
            {
                
                _tries[0 + _tri] = _vert + 1;
                _tries[1 + _tri] = _vert + 0;
                _tries[2 + _tri] = _vert + _xsize + 1;
                _tries[3 + _tri] = _vert + 1;
                _tries[4 + _tri] = _vert + _xsize + 1;
                _tries[5 + _tri] = _vert + _xsize + 2;

                _vert++;
                _tri += 6;

                yield return new WaitForSeconds(0.001f);
            }
            _vert++;
        }
        
    }

    void UpdateMesh()
    {
        _mesh.Clear();
        _mesh.vertices = _verts;
        _mesh.triangles = _tries;

        _mesh.RecalculateNormals();

        UpdateYValues();
    }

    void UpdateYValues()
    {
        
        for (int index = 0, z = 0; z < _zsize + 1; z++)
        {
            for (int x = 0; x < _xsize + 1; x++)
            {
                if (_verts[index].y > 0.0f)
                {
                    _verts[index].y -= _downSpeed * Time.deltaTime;
                }
                index++;

            }
        }
        RaiseRandomPoint();
    }

    void RaiseRandomPoint()
    {
        //_verts[i].y = Mathf.PerlinNoise(x * AudioPeer._amplitudeBuffer, z * AudioPeer._amplitudeBuffer);
        for (int i = 0; i < AudioPeer._amplitudeBuffer; i++)
        {
            System.Random _random = new System.Random();
            int _randomval = _random.Next(0, (_xsize) * (_zsize));
            
            for (int index = 0, z = 0; z < _zsize + 1; z++)
            {
                for (int x = 0; x < _xsize + 1; x++)
                {
                    
                    if (index == _randomval)
                    {
                        _verts[index].y = AudioPeer._amplitudeBuffer;
                    }
                    index++;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (_verts == null)
        {
            return;
        }

        for (int i = 0; i < _verts.Length; i++)
        {
            Gizmos.DrawSphere(_verts[i], 0.1f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMesh();
    }
}
