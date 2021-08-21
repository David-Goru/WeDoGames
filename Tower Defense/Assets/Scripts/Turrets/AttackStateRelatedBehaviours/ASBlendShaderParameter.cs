using UnityEngine;

public class ASBlendShaderParameter : EffectComponent, ITurretAttackState
{
    enum BlendState
    {
        NONE, IN, OUT
    }
    const float LERPING_END = 0.005f;
    [SerializeField] MeshRenderer meshRenderer = null;
    [SerializeField] string renderParameter = "";
    [SerializeField] float minValue = 0f;
    [SerializeField] float maxValue = 1f;
    [SerializeField] float blendingSpeed = 2f;

    Material material;

    float blending = 0f;
    BlendState blendState = BlendState.NONE;

    void Awake()
    {
        material = meshRenderer.material;
    }

    public override void InitializeComponent()
    {
        initMembers();
    }

    void initMembers()
    {
        blending = maxValue;
        material.SetFloat(renderParameter, blending);
        blendState = BlendState.NONE;
    }

    public override void UpdateComponent()
    {
        switch (blendState)
        {
            case BlendState.IN:
                blendParameter(minValue);
                break;
            case BlendState.OUT:
                blendParameter(maxValue);
                break;
        }
    }

    void blendParameter(float destinationValue)
    {
        blending = Mathf.Lerp(blending, destinationValue, blendingSpeed * Time.deltaTime);
        material.SetFloat(renderParameter, blending);
        if (Mathf.Abs(blending - destinationValue) <= LERPING_END) blendState = BlendState.NONE;
    }

    public void OnAttackEnter()
    {
        blendState = BlendState.IN;
    }

    public void OnAttackExit()
    {
        blendState = BlendState.OUT;
    }
}