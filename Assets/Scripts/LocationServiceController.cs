using System.Collections;
using UnityEngine;
using UnityEngine.Android;

public sealed class LocationServiceController : MonoBehaviour
{
    [SerializeField]
    private TMPro.TextMeshProUGUI _textDebug;

    [SerializeField]
    private bool _locationServiceStarted = false;
    [SerializeField]
    private bool _locationServiceAborted = false;

    [SerializeField]
    private GameObject _permissionPanel;

    private int _locationServiceTimeoutSeconds = 10;
    private float _locationServiceCheckIntervalSeconds = 0.5f;

    private void Awake()
    {
        _textDebug.text += "\n" + "Awake()";
    }

    private void OnEnable()
    {
        _textDebug.text += "\n" + "OnEnable()";
      }

    private void Start()
    {
        _textDebug.text += "\n" + "Start()";
        //StartLocationService();
    }

    public void StartLocationService()
    {
        _permissionPanel.SetActive(false);
        StartCoroutine(StartLocationServiceRoutine());
    }

    private bool CheckLocationServicePermission()
    {
        _textDebug.text += "\n" + "CheckLocationServicePermission()";

        if (!Application.isEditor)
        {
            if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
            {
                _textDebug.text += "\n" + "RequestUserPermission - FineLocation";
                Permission.RequestUserPermission(Permission.FineLocation);
                return false;
            }
            else
            {
                _textDebug.text += "\n" + "HasUserAuthorizedPermission - FineLocation";
                _locationServiceStarted = true;
                return true;
            }
        } else
        {
            return true;
        }
    }

    private IEnumerator StartLocationServiceRoutine()
    {
        if (!Input.location.isEnabledByUser)
        {
            _textDebug.text += "\n" + "Waiting to start Location Service";
            if (CheckLocationServicePermission())
            {
                _locationServiceAborted = false;
                _locationServiceStarted = true;
                yield break;
            } else {
                _locationServiceAborted = false;
                _locationServiceStarted = false;
                ShowPermissionRequestDialog();
            }
        }

        _textDebug.text += "\n" + "Input.location.status : " + Input.location.status;

        Input.location.Start(0.1f, 0.1f);

        while (Input.location.status == LocationServiceStatus.Initializing && _locationServiceTimeoutSeconds > 0)
        {
            yield return new WaitForSeconds(1);
            _locationServiceTimeoutSeconds--;
            _textDebug.text += "\n" + "Timer :" + _locationServiceTimeoutSeconds;
        }

        if (_locationServiceTimeoutSeconds <= 0)
        {
            _textDebug.text += "\n" + "Location Service start timeout";
            yield break;
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            _textDebug.text += "\n" + "Location Service start failed";
            yield break;
        }

        if (Input.location.status == LocationServiceStatus.Stopped)
        {
            _textDebug.text += "\n" + "Location Service Stopped";
            yield break;
        }

        //_locationServiceStarted = true;

        _textDebug.text += "\n" + "StartLocationServiceRoutine done";

        yield break;
    }

    public void ShowPermissionRequestDialog()
    {
        _textDebug.text += "\n" + "ShowPermissionRequestDialog()";
        _permissionPanel.SetActive(true);
    }

    public void AbortPermissionDialog()
    {
        _textDebug.text += "\n" + "AbortPermissionDialog()";

        _locationServiceAborted = true;
        _locationServiceStarted = false;
        _permissionPanel.SetActive(false);
    }

    private void Update()
    {
        if (!_locationServiceStarted)
        {
            if (!_locationServiceAborted)
            {
                _locationServiceCheckIntervalSeconds -= Time.deltaTime;
                if (_locationServiceCheckIntervalSeconds <= 0)
                {
                    _textDebug.text += "\n" + "Update()";
                    _locationServiceCheckIntervalSeconds = 0.5f;
                    StartLocationService();
                }
            }
        }
    }

}
