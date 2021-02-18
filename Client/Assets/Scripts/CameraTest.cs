using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTest : MonoBehaviour
{
    //1/128  5
    const float devHeight =1280/128f;//设计大小
    const float devWidth =720/128f;
    void Start()
    {
        float devAspecRatio =devWidth/devHeight;//设计宽高比
        float screenWidth = Screen.width;//屏幕宽度
        Debug.Log("screenWidth ="+screenWidth);
        float orthoGraphicSize = GetComponent<Camera>().orthographicSize;
        float aspectRatio = Screen.width*1f/Screen.height;//屏幕宽高比
        Debug.Log("aspectRatio ="+aspectRatio);
        float cameraHeight =orthoGraphicSize*2;//摄影机实际高度 orthoGraphicSize*2*  Screen.width*1f/Screen.height
        
        Debug.Log("cameraHeight ="+cameraHeight);
        if(devAspecRatio>aspectRatio)//如果实际宽高比小于设计宽高比（屏幕更高)
        {
            orthoGraphicSize = devHeight*devAspecRatio*10;
            GetComponent<Camera>().orthographicSize =orthoGraphicSize;
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
