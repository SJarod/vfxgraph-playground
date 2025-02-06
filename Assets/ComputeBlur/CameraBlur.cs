using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class CameraBlur : MonoBehaviour
{
    [Range(1, 100)]
    public int steps = 1;

    public ComputeShader blurCompute;

    public RenderTexture inputTexture;
    public RenderTexture outputTexture;
    public RenderTexture outputTexture2;

    public MeshRenderer previewMeshRenderer;

    static readonly int widthProperty = Shader.PropertyToID("width");
    static readonly int heightProperty = Shader.PropertyToID("height");

    static readonly int inTextureProperty = Shader.PropertyToID("Input");
    static readonly int outTextureProperty = Shader.PropertyToID("Result");

    static readonly int previewTextureProperty = Shader.PropertyToID("_BaseMap");

    int numX;
    int numY;

    Material previewMat;

    private void OnEnable()
    {
        Debug.Assert(blurCompute != null);
        Debug.Assert(inputTexture != null);

        outputTexture = new RenderTexture(inputTexture.width, inputTexture.height, 0, GraphicsFormat.R32G32B32A32_SFloat, 0);
        outputTexture.name = $"Blur Output : {outputTexture.width}*{outputTexture.height}";

        // enable using this texture as UAV
        outputTexture.enableRandomWrite = true;

        outputTexture2 = new RenderTexture(outputTexture);
        outputTexture2.name = $"Blur Output2 : {outputTexture2.width}*{outputTexture.height}";

        numX = outputTexture.width / 4;
        numY = outputTexture.height / 4;

        blurCompute.SetInt(widthProperty, outputTexture.width);
        blurCompute.SetInt(heightProperty, outputTexture.height);

        previewMat = previewMeshRenderer?.material;
    }

    private void OnDisable()
    {
        outputTexture.Release();
        outputTexture2.Release();
    }

    private void LateUpdate()
    {
        Debug.Assert(blurCompute != null);
        Debug.Assert(inputTexture != null);
        Debug.Assert(outputTexture != null);

        for (int i = 0; i < steps; ++i)
        {
            // set texture for kernel
            blurCompute.SetTexture(0, inTextureProperty, i == 0 ? inputTexture : (i % 2 == 0 ? outputTexture2 : outputTexture));
            blurCompute.SetTexture(0, outTextureProperty, i % 2 == 0 ? outputTexture : outputTexture2);

            blurCompute.Dispatch(0, numX, numY, 1);
        }

        // enable preview
        previewMat.SetTexture(previewTextureProperty, steps % 2 == 0 ? outputTexture2 : outputTexture);
    }
}
