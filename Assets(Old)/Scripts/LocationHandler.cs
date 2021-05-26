using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LocationHandler : MonoBehaviour
{
    private LocationService location;
    private Vector2 currentCoordinates;
    [SerializeField] private Text messages;
    [SerializeField] private float locationServiceStartDelay;
    [SerializeField] private float locationServiceAccuracy;
    [SerializeField] private float locationServiceUpdateDelay;
    [SerializeField] private List<ObjectInfo> allObjects;
    private OnObjectNear onObjectNear = new OnObjectNear();
    private OnObjectFar onObjectFar = new OnObjectFar();
    [SerializeField] private ObjectsManager objectsManager;
    [SerializeField] private GameObject testObject;

    public IEnumerator Start()
    {
        Instantiate(testObject, Vector3.forward, Quaternion.identity);
        foreach (ObjectInfo objectInfo in allObjects)
        {
            objectInfo.Player = gameObject;
            objectInfo.PlayerCoordinates = currentCoordinates;
        }
        onObjectNear.AddListener(objectsManager.createInfoCard);
        onObjectFar.AddListener(objectsManager.hideInfoCard);
        objectsManager.PlayerCoordinates = currentCoordinates;
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
                    LocationInfo data = location.lastData;
                    currentCoordinates.x = data.latitude;
                    currentCoordinates.y = data.longitude;
                    messages.text = currentCoordinates.ToString("G3");
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
        if (location.status == LocationServiceStatus.Running)
        {
            LocationInfo data = location.lastData;
            currentCoordinates.x = data.latitude;
            currentCoordinates.y = data.longitude;
            messages.text = currentCoordinates.ToString("G3");
            foreach (ObjectInfo objectInfo in allObjects)
            {
                if (objectInfo.IsNear())
                {
                    onObjectNear.Invoke(objectInfo);
                }
                else if (objectInfo.IsFar())
                {
                    onObjectFar.Invoke(objectInfo);
                }
            }
        }
        else
        {
            messages.text = "Location service failed";
        }
    }

}
