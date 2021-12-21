using System.Collections;
using UnityEngine;
using UnityEngine.Android;

public sealed class LocationServiceController : MonoBehaviour
{
    [SerializeField]
    private TMPro.TextMeshProUGUI _textDebug;

    [SerializeField]
    private bool _locationServiceStarted = false;

    private int _locationServiceTimeoutSeconds = 10;

    private void Awake()
    {
        _textDebug.text += "\n" + "Awake()";
        AskPermission();
    }

    private void Start()
    {
        _textDebug.text += "\n" + "Start()";
        StartCoroutine(StartLocationService());
    }

    private void AskPermission()
    {
        _textDebug.text += "\n" + "AskPermission()";

        if (!Application.isEditor)
        {
            if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
            {
                _textDebug.text += "\n" + "RequestUserPermission - FineLocation";
                Permission.RequestUserPermission(Permission.FineLocation);
            }
            else
            {
                _textDebug.text += "\n" + "HasUserAuthorizedPermission - FineLocation";
            }
        }
    }

    private IEnumerator StartLocationService()
    {
        if (!Input.location.isEnabledByUser)
        {
            _textDebug.text += "\n" + "Waiting to start Location Service";
            AskPermission();
            yield break;
        }

        _textDebug.text += "\n" + "Input.location.Start...";

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

        _locationServiceStarted = true;

        _textDebug.text += "\n" + "Location Service started";

        yield break;
    }

    public void ShowPermissionRequestDialog()
    {

    }

    /*
     [debug]
    Awake()
    RequestUserPermission - FineLocation
    Start()
    Waiting to start Location Service
     */
}
