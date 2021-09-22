using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
//非运行时也出发效果
[ExecuteInEditMode]
public class WaterWave : MonoBehaviour {
 
    bool ifWave;
    bool ifEnd;
    //Inspector面板上直接拖入
    public Shader shader = null;
    private Material _material = null;
 
    //距离系数
    public float distanceFactor = 60.0f;
    //时间系数
    public float timeFactor = -30.0f;
    //sin函数结果系数
    public float totalFactor = 1.0f;
 
    //波纹宽度
    public float waveWidth = 0.5f;
    //波纹扩散的速度
    public float waveSpeed = 0.5f;
 
    private float waveStartTime;
    private Vector4 startPos = new Vector4(0.5f, 0.5f, 0, 0);
 
    //根据Shader生成材质
    public Material _Material
    {
        get
        {
            if (_material == null)
                _material = GenerateMaterial(shader);
            return _material;
        }
    }
 
    //根据shader创建用于屏幕特效的材质
    protected Material GenerateMaterial(Shader shader)
    {
        if (shader == null)
            shader = Shader.Find("Custom/WaterWave");
        //需要判断shader是否支持
        if (shader.isSupported == false)
            return null;
        Material material = new Material(shader);
        material.hideFlags = HideFlags.DontSave;
        if (material)
            return material;
        return null;
    }
 
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if(!ifWave)
        return;
        //计算波纹移动的距离，根据enable到目前的时间*速度求解
        float curWaveDistance = (Time.time - waveStartTime) * waveSpeed;
        //设置一系列参数
        _Material.SetFloat("_distanceFactor", distanceFactor);
        _Material.SetFloat("_timeFactor", timeFactor);
        _Material.SetFloat("_totalFactor", totalFactor);
        _Material.SetFloat("_waveWidth", waveWidth);
        _Material.SetFloat("_curWaveDis", curWaveDistance);
        _Material.SetVector("_startPos", startPos);
        Graphics.Blit(source, destination, _Material);
        if(!ifEnd)
        Wave();
    }
    void Wave()
    {
        ifEnd =true;
        StartCoroutine(WaveEnd());
    }
    IEnumerator WaveEnd()
    {
        yield return new WaitForSeconds(3f);
        Destroy(this);
    }
    // void Update()
    // {
    //     if (Input.GetMouseButton(0))
    //     {
    //         Vector2 mousePos = Input.mousePosition;
    //         //将mousePos转化为（0，1）区间
    //         startPos = new Vector4(mousePos.x / Screen.width, mousePos.y / Screen.height, 0, 0);
    //         waveStartTime = Time.time;
    //         Debug.Log("startPos,x="+startPos.x+",y="+startPos.y);
    //     }
    // }
    // void Start()
    // {
    //     Debug.Log("test1");
    //     StartCoroutine(WaitForStart());
        
    // }
    // IEnumerator WaitForStart()
    // {
    //     yield return new WaitForSeconds(1f);
    //     waveStartTime = Time.time;
    //     Debug.Log("test2");
    // }
    public static void Emit()
    {
        WaterWave waterWave =Player.instance.transform.gameObject.AddComponent<WaterWave>();
        waterWave.waveStartTime =Time.time;
        waterWave.ifWave = true;
    }
}