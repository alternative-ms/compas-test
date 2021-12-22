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
    private float _lerpTime = 2f;
    [SerializeField]
    private float _lerpDiv = 0f;
    [SerializeField]
    private float _lerpMinDelta = 1f;

    [SerializeField]
    private Image _imageCompassArrow;
    [SerializeField]
    private TMPro.TextMeshProUGUI _textCompassAngle;

    [SerializeField]
    private bool _smoothValues = true;

    [SerializeField]
    private bool _lerpValues = true;

    private void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

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
                if (_lerpDiv > _lerpMinDelta) _lerpDiv = _lerpMinDelta;
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
        _textCompassAngle.text = "" + (int)_heading + "°";

        _imageCompassArrow.rectTransform.localEulerAngles = new Vector3(0, 0, _heading);
    }

}
