using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;


public class HologramController : MonoBehaviour
{
    [SerializeField] private Vector3 _timeValues;
    [SerializeField] private bool _activate;
    private Material _hologram;
    private void Awake()
    {
        _hologram = GetComponent<Renderer>().material;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Glitch());
    }
    private IEnumerator Glitch()
    {
        while (true)
        {
            float time = Random.Range(_timeValues.x, _timeValues.y);
            float timeMiddlePoint = (_timeValues.x + _timeValues.y) / 2;
            int vertical = timeMiddlePoint >= time ? 1 : 0;
            float noiseStregth = Random.Range(0.1f, 0.9f);

            _hologram.SetFloat("_Horizontal_Glitch", vertical);
            _hologram.SetFloat("_Noise_Stregth", noiseStregth);
            yield return new WaitForSecondsRealtime(time);
            _hologram.SetFloat("_Enable_Glitch", 1);
            yield return new WaitForSecondsRealtime(Random.Range(_timeValues.x, _timeValues.z));
            _hologram.SetFloat("_Enable_Glitch", 0);
        }
    }
}
