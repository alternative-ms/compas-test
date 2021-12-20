using System.Collections;
using UnityEngine;
using UnityEngine.Android;

public sealed class LocationServiceController : MonoBehaviour
{
    [SerializeField]
    private bool _locationServiceStarted = false;

    private int _locationServiceTimeoutSeconds = 10;
    private void Awake()
    {
        if (!Application.isEditor)
        {
            if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation)) Permission.RequestUserPermission(Permission.FineLocation);
        }
    }

    private void Start()
    {
        StartCoroutine(StartLocationService());
    }

    private IEnumerator StartLocationService()
    {
        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("Waiting to start Location Service");
            yield break;
        }

        Input.location.Start(0.1f, 0.1f);

        while (Input.location.status == LocationServiceStatus.Initializing && _locationServiceTimeoutSeconds > 0)
        {
            yield return new WaitForSeconds(1);
            _locationServiceTimeoutSeconds--;
        }

        if (_locationServiceTimeoutSeconds <= 0)
        {
            Debug.Log("Location Service start timeout");
            yield break;
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("Location Service start failed");
            yield break;
        }

        _locationServiceStarted = true;

        Debug.Log("Location Service started");

        yield break;
    }

    public void ShowPermissionRequestDialog()
    {

    }
}
