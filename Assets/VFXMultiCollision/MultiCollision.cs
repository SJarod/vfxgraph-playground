using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.VFX.Utility;

[ExecuteAlways]
[RequireComponent(typeof(VisualEffect))]
public class MultiCollision : MonoBehaviour
{
    public ExposedProperty BufferProperty = "SphereColliders";
    public ExposedProperty CountProperty = "Count";

    VisualEffect effect;
    GraphicsBuffer graphicsBuffer;

    public SphereCollider[] colliders;
    public Vector4[] data;

    private void OnEnable()
    {
        effect = GetComponent<VisualEffect>();
        graphicsBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, colliders.Length, 4 * sizeof(float));
        effect.SetGraphicsBuffer(BufferProperty, graphicsBuffer);
        effect.SetInt(CountProperty, colliders.Length);
        data = new Vector4[colliders.Length];
    }

    private void OnDisable()
    {
        graphicsBuffer.Dispose();
    }

    void Update()
    {
        for (int i = 0; i < colliders.Length; ++i)
        {
            var collider = colliders[i];
            var pos = collider.transform.position;
            Vector4 d = data[i];
            d.x = pos.x;
            d.y = pos.y;
            d.z = pos.z;
            d.w = collider.radius * collider.transform.localScale.x;
            data[i] = d;
        }
        graphicsBuffer.SetData(data);
    }
}
