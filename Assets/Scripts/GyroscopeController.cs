using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GyroscopeController : MonoBehaviour
{
    enum Orientation
    {
        parallel,
        perpendicular,
    }

    [SerializeField]
    private TMPro.TextMeshProUGUI _textGyroscope;

    private Gyroscope _gyroscope;
    private bool _gyroscopeEnabled = false;
    private Orientation _previousOrientation = Orientation.parallel;
    private Orientation _currentOrientation = Orientation.parallel;

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

                float angleX = _gyroscope.attitude.eulerAngles.x; /* convert from 0..360° to -180°..180° */ if (angleX > 180) angleX -= 360; if (angleX < -180) angleX += 360;
                float angleY = _gyroscope.attitude.eulerAngles.y; /* convert from 0..360° to -180°..180° */ if (angleY > 180) angleY -= 360; if (angleY < -180) angleY += 360;
                float angleZ = _gyroscope.attitude.eulerAngles.z; /* convert from 0..360° to -180°..180° */ if (angleZ > 180) angleZ -= 360; if (angleZ < -180) angleZ += 360;

                _textGyroscope.text += "\nGizmo : " + "\nX : " + angleX + "\nY : " + angleY + "\nZ : " + angleZ;

                if ((Mathf.Abs(angleX) >= 50) || (Mathf.Abs(angleY) >= 50))
                {
                    _textGyroscope.text += "\nThe device is rotated perpendicular to the ground";
                } else
                {
                    _textGyroscope.text += "\nThe device is parallel to the ground";
                }

                if (Input.deviceOrientation == DeviceOrientation.Portrait)
                {
                    _textGyroscope.text += "\nDeviceOrientation - Portrait";
                }

                if (Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown)
                {
                    _textGyroscope.text += "\nDeviceOrientation - PortraitUpsideDown";
                }

                if (Input.deviceOrientation == DeviceOrientation.LandscapeLeft)
                {
                    _textGyroscope.text += "\nDeviceOrientation - LandscapeLeft";
                }

                if (Input.deviceOrientation == DeviceOrientation.LandscapeRight)
                {
                    _textGyroscope.text += "\nDeviceOrientation - LandscapeRight";
                }

                if (Input.deviceOrientation == DeviceOrientation.FaceUp)
                {
                    _textGyroscope.text += "\nDeviceOrientation - FaceUp";
                }

                if (Input.deviceOrientation == DeviceOrientation.FaceDown)
                {
                    _textGyroscope.text += "\nDeviceOrientation - FaceDown";
                }
            }
        }
    }

}
