using UnityEngine;

public class DitherEffect : MonoBehaviour
{
    [SerializeField] Material DitherMat;
    [SerializeField] Material thresholdMat;

    Camera cam;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        RenderTexture large = RenderTexture.GetTemporary(1640, 940);
        RenderTexture main = RenderTexture.GetTemporary(820, 470);

        //camera frustrum time
        Vector3[] corners = new Vector3[4];

        cam.CalculateFrustumCorners(new Rect(0, 0, 1, 1), cam.farClipPlane,
            Camera.MonoOrStereoscopicEye.Mono, corners);

        for(int i = 0; i < 4; i++)
        {
            corners[i] = transform.TransformVector(corners[i]);
            corners[i].Normalize();
        }

        DitherMat.SetVector("_BL", corners[0]);
        DitherMat.SetVector("_TL", corners[1]);
        DitherMat.SetVector("_TR", corners[2]);
        DitherMat.SetVector("_BR", corners[3]);


        Graphics.Blit(source, large, DitherMat);
        Graphics.Blit(large, main, thresholdMat);
        Graphics.Blit(main, destination);

        RenderTexture.ReleaseTemporary(large);
        RenderTexture.ReleaseTemporary(main);
    }


}
