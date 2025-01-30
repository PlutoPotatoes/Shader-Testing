using UnityEngine;
using System.Collections.Generic;
[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]


public class DatamoshEffect : MonoBehaviour
{
    public Material DMmat;
    private float DMoshStr;
    private RenderTexture pr;
    private Queue<RenderTexture> frameBuffer = new Queue<RenderTexture>();
    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.GetComponent<Camera>().depthTextureMode = DepthTextureMode.MotionVectors;

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
            Shader.SetGlobalFloat("_VariableButton", Mathf.Max(buttonVal-Time.deltaTime, 0f));
        }
    }
}
