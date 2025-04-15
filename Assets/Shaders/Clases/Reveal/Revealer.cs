using UnityEngine;

[ExecuteAlways]
public class Revealer : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private Material _revealMaterial;

    void Update()
    {
        if (_revealMaterial != null && _player != null)
        {
            _revealMaterial.SetVector("_Revealer_Position", _player.position);
        }
    }
}
