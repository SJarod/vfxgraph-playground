
// https://github.com/keijiro/com.unity.visualeffectgraph-7.1.6-fix/blob/9ee1ca1ab62fdf21dabccd443452999085ed5f06/Shaders/VFXCommon.hlsl#L86
void ApplyVelocityField(inout VFXAttributes attributes, VFXSampler2D velocityField)
{
    float4 v = velocityField.t.SampleLevel(velocityField.s, attributes.position.xy, 0.0);
    attributes.velocity = float3(v.x, 0.0, v.y);
}