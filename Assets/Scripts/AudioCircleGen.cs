using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class AudioCircleGen : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject CubePrefab;
    public Toggle toggle;
    public TMP_Dropdown coldrop;
    public float radius = 2;
    public float shiftspeed = 2.5f;
    void Start()
    {
        float x = 0, y = 0;
        for (int i = 0; i < 66; i++)
        {
            float angle = ((360.0f/66.0f)*(float)i)+90;

            x = radius * Mathf.Cos(Mathf.Deg2Rad*angle);
            y = radius * Mathf.Sin(Mathf.Deg2Rad*angle);


            GameObject newparamcube = Instantiate(CubePrefab, new Vector3 (x, y, 0), Quaternion.identity);
            newparamcube.GetComponent<Transform>().Rotate(new Vector3(0, 0, angle + 90));
            ParamCube paramcubescript = newparamcube.GetComponent<ParamCube>();
            paramcubescript._band = i;
            paramcubescript._64band = true;
            paramcubescript._shiftspeed = shiftspeed;
            paramcubescript.radius = radius;
            paramcubescript._toggle = toggle;
            paramcubescript._colDrop = coldrop;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
