using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GyroscopeController : MonoBehaviour
{
    [SerializeField]
    private TMPro.TextMeshProUGUI _textGyroscope;

    [SerializeField]
    private Transform _debugGizmoCube;

    private Gyroscope _gyroscope;
    private Quaternion _debugRotation = new Quaternion(0, 0, 1, 0);
    private bool _gyroscopeEnabled = false;
    private bool _needFixRotation = false;

    private void Start()
    {
        if (!Application.isEditor)
        {
            if (!SystemInfo.supportsGyroscope)
            {
                _textGyroscope.text = "Gyroscope not support on this device";
                _gyroscopeEnabled = false;
            }
            else
            {
                _gyroscope = Input.gyro;
                _gyroscope.enabled = true;
                _gyroscopeEnabled = true;
            }
        }
    }

    private void Update()
    {
        if (!Application.isEditor)
        {
            if (_gyroscopeEnabled)
            {
                _textGyroscope.text = "Gyroscope : " + "\nX : " + _gyroscope.attitude.x + "\nY : " + _gyroscope.attitude.y + "\nZ : " + _gyroscope.attitude.z + "\nW : " + _gyroscope.attitude.w;
                if (_needFixRotation) _debugGizmoCube.localRotation = _gyroscope.attitude * _debugRotation; else _debugGizmoCube.localRotation = _gyroscope.attitude;
            }
        }
    }

    public void ToggleRotatrionFix(bool newValue)
    {
        _needFixRotation = newValue;
    }

}
