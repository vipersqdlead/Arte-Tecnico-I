using UnityEngine;

public class RevealGround : MonoBehaviour
{
    [SerializeField] Transform shield;
    [SerializeField] Material material;

    void Update()
    {
        if (material != null && shield != null)
        {
            material.SetVector("_ShieldPosition", shield.position);
            material.SetFloat("_TransparencySize", shield.localScale.x / 2);
        }
    }
}
