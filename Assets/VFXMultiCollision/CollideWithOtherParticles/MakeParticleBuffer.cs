using UnityEngine;
using UnityEngine.VFX;

[ExecuteAlways]
[RequireComponent(typeof(VisualEffect))]
public class MakeParticleBuffer : MonoBehaviour
{
    [Range(16, 65536)]
    public int particleCount = 256;

    VisualEffect effect;
    static readonly int bufferProperty = Shader.PropertyToID("ParticleBuffer");
    static readonly int particleCountProperty = Shader.PropertyToID("ParticleCount");

    GraphicsBuffer buffer;

    private void OnEnable()
    {
        effect = GetComponent<VisualEffect>();
        UpdateBuffer(particleCount);
    }

    void UpdateBuffer(int size)
    {
        buffer?.Dispose();
        buffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, size, 4 * sizeof(float));
        effect.SetGraphicsBuffer(bufferProperty, buffer);
        effect.SetInt(particleCountProperty, size);
    }

    private void OnValidate()
    {
        UpdateBuffer(particleCount);
        effect.Reinit();
        effect.Play();
    }

    private void OnDisable()
    {
        buffer.Dispose();
    }

    Vector4[] data = new Vector4[256];

    [ContextMenu("Dump")]
    void DumpData()
    {
        buffer.GetData(data);
        for (int i = 0; i < 32; ++i)
        {
            Debug.Log(data[i]);
        }
    }
}
