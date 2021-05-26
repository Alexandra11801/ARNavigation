using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocationHandler : MonoBehaviour
{
    private LocationService location;
    private Vector3 currentCoordinates;
    private bool canTrackLocation;
    private Compass compass;
    [SerializeField] private float locationServiceStartDelay;
    [SerializeField] private float locationServiceAccuracy;
    [SerializeField] private float locationServiceUpdateDelay;

    [SerializeField] private ObjectsManager objectsManager;
    private OnObjectNear onObjectNear = new OnObjectNear();
    private OnObjectFar onObjectFar = new OnObjectFar();

    [SerializeField] private GameObject testObject;
    [SerializeField] private Text messages;

    public Compass Compass => Input.compass;

    public Vector3 CurrentCoordinates => currentCoordinates;

    public IEnumerator Start()
    {
        onObjectNear.AddListener(objectsManager.CreateInfoCard);
        onObjectFar.AddListener(objectsManager.HideInfoCard);
        location = Input.location;
        if (location.isEnabledByUser)
        {
            location.Start(locationServiceAccuracy, locationServiceUpdateDelay);
            while (location.status == LocationServiceStatus.Initializing
                   && locationServiceStartDelay > 0)
            {
                yield return new WaitForSeconds(1);
                locationServiceStartDelay--;
            }
            switch (location.status)
            {
                case LocationServiceStatus.Failed:
                    messages.text = "Location service failed";
                    break;
                case LocationServiceStatus.Initializing:
                    messages.text = "Time out";
                    break;
                case LocationServiceStatus.Running:
                    canTrackLocation = true;
                    break;
            }
        }
        else
        {
            messages.text = "Enable location service";
        }
    }

    public void Update()
    {
        if (canTrackLocation)
        {
            if (location.status == LocationServiceStatus.Running)
            {
                LocationInfo data = location.lastData;
                currentCoordinates = new Vector3(data.latitude, data.altitude, data.longitude);
                objectsManager.PlayerCoordinates = currentCoordinates;
                // messages.text = currentCoordinates.ToString("F7");
                if (objectsManager.AllObjects.Count != 0)
                {
                    foreach (ObjectInfo objectInfo in objectsManager.AllObjects)
                    {
                        objectInfo.PlayerCoordinates = currentCoordinates;
                        // messages.text = Locationf.Distance(currentCoordinates, 
                        //     objectInfo.ObjectCoordinates).ToString("F5");
                        if (objectInfo.GetCloser())
                        {
                            //Instantiate(testObject, transform.forward, Quaternion.identity);
                            onObjectNear.Invoke(objectInfo);
                        }
                        else if (objectInfo.GetAway())
                        {
                            onObjectFar.Invoke(objectInfo);
                        }
                    }
                }
            }
            else
            {
                messages.text = "Location service failed";
            }
        }
    }

}
