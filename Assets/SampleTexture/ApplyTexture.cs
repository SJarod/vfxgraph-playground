using UnityEngine;
using UnityEngine.VFX;

[ExecuteAlways]
[RequireComponent(typeof(VisualEffect))]
public class ApplyTexture : MonoBehaviour
{
    [SerializeField] private Texture2D texture;

    private VisualEffect effect;
    private static readonly int textureProperty = Shader.PropertyToID("VelocityField");

    private void OnEnable()
    {
        effect = GetComponent<VisualEffect>();
        PassTextureToEffect(this.texture);
    }

    private void OnValidate()
    {
        PassTextureToEffect(this.texture);
        effect.Reinit();
        effect.Play();
    }

    private void PassTextureToEffect(Texture2D texture)
    {
        effect.SetTexture(textureProperty, texture);
    }
}
