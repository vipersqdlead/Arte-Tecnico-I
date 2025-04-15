using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hologramController : MonoBehaviour
{
    [SerializeField] private Vector3 _timeMaxValues;
    [SerializeField] Material _hologram;

    void Awake()
    {
        _hologram = GetComponent<Renderer>().material;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Glitch());
    }

    private IEnumerator Glitch()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(Random.Range(_timeMaxValues.x, _timeMaxValues.y));
            _hologram.SetFloat("_Glitch_Direction", Random.Range(0, 2));
            _hologram.SetFloat("_Glitch_Enable", 1);
            yield return new WaitForSecondsRealtime(Random.Range(_timeMaxValues.x, _timeMaxValues.z));
            _hologram.SetFloat("_Glitch_Direction", Random.Range(0, 2));
            _hologram.SetFloat("_Glitch_Enable", 0);
        }
    }
}
