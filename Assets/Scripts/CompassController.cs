using UnityEngine;
using UnityEngine.UI;

public sealed class CompassController : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField]
#endif
    private float _rawHeading = 0; // The heading in degrees relative to the geographic North Pole in current frame
    private float _heading = 0;
    private float _deltaHeading = 0;

    [SerializeField]
    private float _compassThreshold = 4;
    [SerializeField]
    private float _lerpTime = 50f;
    [SerializeField]
    private float _lerpDiv = 0f;

    [SerializeField]
    private Image _imageCompassArrow;
    [SerializeField]
    private Text _textCompassAngle;

    [SerializeField]
    private bool _smoothValues = false;

    [SerializeField]
    private bool _lerpValues = false;

    private void Start()
    {
#if !UNITY_EDITOR
        Input.compass.enabled = true;
#endif
    }

    private void Update()
    {
#if !UNITY_EDITOR
        _rawHeading = Input.compass.trueHeading;
#endif
        if (_smoothValues)
        {
            _deltaHeading = _heading - _rawHeading;
            if (_lerpValues)
            {
                _lerpDiv = Mathf.Abs(_deltaHeading);
                if (_lerpDiv == 0) _lerpDiv = 0.0001f;
                _heading = Mathf.LerpAngle(_heading, _rawHeading, Time.deltaTime * _lerpTime / _lerpDiv);
            }
            else
            {
                if (_deltaHeading > 180) _deltaHeading -= 360; else if (_deltaHeading < -180) _deltaHeading += 360;
                if (Mathf.Abs(_deltaHeading) > _compassThreshold) _heading = _rawHeading;
            }
        } else
        {
            _heading = _rawHeading;
        }

        UpdateVisualCompass();
    }

    private void UpdateVisualCompass()
    {
        _textCompassAngle.text = "" + _heading + "°";

        _imageCompassArrow.rectTransform.localEulerAngles = new Vector3(0, 0, _heading);
    }

    public void ToggleSmoothValueState(bool newValue)
    {
        _smoothValues = newValue;
    }

    public void ToggleLerpState(bool newValue)
    {
        _lerpValues = newValue;
    }
}
