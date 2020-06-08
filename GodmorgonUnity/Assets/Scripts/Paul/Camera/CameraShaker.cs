using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    Transform _target;
    Vector3 _initialPos;

    [Range(0f, 2f)]
    public float shakeIntensity = 0f;

    private void Start()
    {
        _target = GetComponent<Transform>();
        _initialPos = _target.localPosition;
    }

    float _shakingDuration = 0f;

    public void Shake(float duration)
    {
        _initialPos = _target.localPosition;

        if (duration > 0)
        {
            _shakingDuration += duration;
        }
    }

    bool _isShaking = false;

    void Update()
    {
        if(_shakingDuration > 0 && !_isShaking)
        {
            StartCoroutine(DoShake());
        }
    }

    IEnumerator DoShake()
    {
        _isShaking = true;

        var startTime = Time.realtimeSinceStartup;
        while(Time.realtimeSinceStartup < startTime + _shakingDuration)
        {
            var randomPoint = new Vector3(Random.Range(-1f, 1f)* shakeIntensity, Random.Range(-1f, 1f)* shakeIntensity, _initialPos.z);
            _target.localPosition += randomPoint;
            yield return null;
        }

        _shakingDuration = 0f;
        _target.localPosition = _initialPos;
        _isShaking = false;
    }
}
