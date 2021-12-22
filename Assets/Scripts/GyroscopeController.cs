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
    private bool _gyroscopeEnabled = false;

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
                _debugGizmoCube.localRotation = _gyroscope.attitude;
                _textGyroscope.text = "Gyroscope : " + "\nX : " + _gyroscope.attitude.x + "\nY : " + _gyroscope.attitude.y + "\nZ : " + _gyroscope.attitude.z + "\nW : " + _gyroscope.attitude.w;

                float angleX = _debugGizmoCube.localEulerAngles.x; /* convert from 0..360° to -180°..180° */ if (angleX > 180) angleX -= 180; if (angleX < -180) angleX += 180;
                float angleY = _debugGizmoCube.localEulerAngles.y; /* convert from 0..360° to -180°..180° */ if (angleY > 180) angleY -= 180; if (angleY < -180) angleY += 180;
                float angleZ = _debugGizmoCube.localEulerAngles.z; /* convert from 0..360° to -180°..180° */ if (angleZ > 180) angleZ -= 180; if (angleZ < -180) angleZ += 180;

                _textGyroscope.text += "\nGizmo : " + "\nX : " + angleX + "\nY : " + angleY + "\nZ : " + angleZ;
            }
        }
    }

    //on table, portrait, vertical
    // x -0.007(7..8)
    // y -0.006(3..5)
    // z -0.923(4..5)
    // w 0.38(1..2)

    //on table, portrait, horizontal
    // x -0.006(2..3)
    // y -0.006(1..2)
    // z -0.43(1..2)
    // w 0.902(1..2)

    //on table, landscape, horizontal
    // x 0
    // y 0.009(2..3)
    // z 0.940(1..2)
    // w -0.340(3..5)

    //on table, landscape, vertical
    // x 0.001(7..8)
    // y 0.009(5..6)
    // z 0.902(0..1)
    // w 0.431(5..7)

    // on table - Z axis fron the screen , like cam direction

}
