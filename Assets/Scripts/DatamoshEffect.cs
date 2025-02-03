using UnityEngine;
using System.Collections.Generic;
[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]


public class DatamoshEffect : MonoBehaviour
{
    [Tooltip("Value to multiply time by as shader fades (1 = 1 second, 2 = 0.5 seconds)")]
    [SerializeField] float fadeSpeed = 1;
    public Material DMmat;
    private float DMoshStr;
    private RenderTexture pr;
    private Queue<RenderTexture> frameBuffer = new Queue<RenderTexture>();
    
    private Camera mainCam; 


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCam = this.GetComponent<Camera>();
        mainCam.depthTextureMode = DepthTextureMode.MotionVectors;

    }

    // Update is called once per frame
    void Update()
    {
        Shader.SetGlobalInteger("_Button", Input.GetButton("Fire1") ? 1 : 0);
        if (Input.GetButton("Fire1"))
        {
            DMoshStr+=0.2f;
            Shader.SetGlobalFloat("_DMOSHSTR", DMoshStr);
            Shader.SetGlobalFloat("_VariableButton", 1f);


        }
        else
        {
            DMoshStr = 0f;
            updateButton();
        }
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        frameBuffer.Enqueue(source);
        if(frameBuffer.Count > 1 && Shader.GetGlobalInteger("_Button") == 0)
        {
            Shader.SetGlobalTexture("_FrameBuffer", frameBuffer.Dequeue());
        }
        Graphics.Blit(source, destination, DMmat);
    }

    private void updateButton()
    {
        float buttonVal = Shader.GetGlobalFloat("_VariableButton");
        if (buttonVal != 0)
        {
            Shader.SetGlobalFloat("_VariableButton", Mathf.Max(buttonVal-(fadeSpeed*Time.deltaTime), 0f));
        }
    }
}
