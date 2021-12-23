using UnityEngine;
using UnityEngine.UI;

public sealed class CompassController : MonoBehaviour
{
    public enum Orientation
    {
        parallel,
        perpendicular
    }

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
    private GameObject _compassArrowAddon;

    [SerializeField]
    private bool _smoothValues = true;

    [SerializeField]
    private bool _lerpValues = true;

    private float _currentXAngle = 0;
    private float _startLerpXAngle = 0;
    private float _parallelXAngle = 0;
    private float _perpendicularXAngle = 75;
    private float _lertTime = 0;

    private bool _lerpToParallel = false;
    private bool _lerpToPerpendicular = false;

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

        if (_lerpToParallel)
        {
            _lertTime += Time.deltaTime;

            if (_lertTime >= 1)
            {
                _lertTime = 1;
                _lerpToParallel = false;
            }

            _currentXAngle = Mathf.LerpAngle(_startLerpXAngle, _parallelXAngle, _lertTime);
        }

        if (_lerpToPerpendicular)
        {
            _lertTime += Time.deltaTime;

            if (_lertTime >= 1)
            {
                _lertTime = 1;
                _lerpToPerpendicular = false;
            }

            _currentXAngle = Mathf.LerpAngle(_startLerpXAngle, _perpendicularXAngle, _lertTime);
        }

        _imageCompassArrow.rectTransform.localEulerAngles = new Vector3(_currentXAngle, 0, _heading);
    }

    public void UpdateOrientation(Orientation newOrientation)
    {
        _lertTime = 0;

        _startLerpXAngle = _currentXAngle;

        if (newOrientation == Orientation.parallel)
        {
            _lerpToParallel = true;
            _lerpToPerpendicular = false;
            _compassArrowAddon.SetActive(false);
        }

        if (newOrientation == Orientation.perpendicular)
        {
            _lerpToPerpendicular = true;
            _lerpToParallel = false;
            _compassArrowAddon.SetActive(true);
        }
    }

}
