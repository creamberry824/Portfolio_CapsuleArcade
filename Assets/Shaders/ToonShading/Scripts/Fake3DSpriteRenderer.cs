using UnityEngine;

public class Fake3DSpriteRenderer : MonoBehaviour
{
	public RenderTexture Texture { get; private set; }
    public Camera OutlineCmera;
    public Renderer _renderer;
    public GameObject _rendererGameObject;
    public int iRenderTexX = 256, iRenderTexY = 256;
    public int iRenderTexZ = 24;

    private void Awake()
    {
        _rendererGameObject.SetActive(true);

        Texture = new RenderTexture(iRenderTexX, iRenderTexY, iRenderTexZ);
        //(256, 256, 24);
        //800

        OutlineCmera.targetTexture = Texture;
        _renderer.material.SetTexture("_MainTex", Texture);
    }
}