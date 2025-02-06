using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class CameraBlur : MonoBehaviour
{
    [Range(1, 10)]
    public int steps = 1;

    public ComputeShader blurCompute;

    public RenderTexture inputTexture;
    public RenderTexture outputTexture;

    public MeshRenderer previewMeshRenderer;

    static readonly int widthProperty = Shader.PropertyToID("width");
    static readonly int heightProperty = Shader.PropertyToID("height");

    static readonly int inTextureProperty = Shader.PropertyToID("Input");
    static readonly int outTextureProperty = Shader.PropertyToID("Result");

    static readonly int previewTextureProperty = Shader.PropertyToID("_BaseMap");

    int numX;
    int numY;

    private void OnEnable()
    {
        Debug.Assert(blurCompute != null);
        Debug.Assert(inputTexture != null);

        outputTexture = new RenderTexture(inputTexture.width, inputTexture.height, 0, GraphicsFormat.R32G32B32A32_SFloat, 0);

        // enable using this texture as UAV
        outputTexture.enableRandomWrite = true;
        outputTexture.name = $"Blur Output : {outputTexture.width}*{outputTexture.height}";

        // enable preview
        previewMeshRenderer?.material.SetTexture(previewTextureProperty, outputTexture);

        numX = outputTexture.width / 4;
        numY = outputTexture.height / 4;

        // set texture for kernel
        blurCompute.SetTexture(0, inTextureProperty, inputTexture);
        blurCompute.SetTexture(0, outTextureProperty, outputTexture);

        blurCompute.SetInt(widthProperty, outputTexture.width);
        blurCompute.SetInt(heightProperty, outputTexture.height);
    }

    private void OnDisable()
    {
        outputTexture.Release();
    }

    private void LateUpdate()
    {
        Debug.Assert(blurCompute != null);
        Debug.Assert(inputTexture != null);
        Debug.Assert(outputTexture != null);

        blurCompute.Dispatch(0, numX, numY, 1);
    }
}
