using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class ObjectsManager : MonoBehaviour
{
    [SerializeField] private LocationHandler locationHandler;
    private Vector3 playerCoordinates;
    private List<ObjectInfo> allObjects;
    [SerializeField] private GameObject infoCard;
    private List<GameObject> createdCards;

    private XmlSerializer serializer;
    
    [SerializeField] private Text messages;
    [SerializeField] private LineRenderer lineRenderer;

    public Vector3 PlayerCoordinates
    {
        get => playerCoordinates;
        set => playerCoordinates = value;
    }

    public List<ObjectInfo> AllObjects => allObjects;

    public void Start()
    {
        allObjects = XmlHandler.LoadObjectsFromDataFile();
        createdCards = new List<GameObject>();
    }

    public void CreateInfoCard(ObjectInfo objectInfo)
    {
        // messages.text = "Card created";  
        Vector3 direction = Locationf.Direction(playerCoordinates, objectInfo.ObjectCoordinates).normalized;
        float northHeading = locationHandler.Compass.trueHeading;
        Vector3 sceneDirection = new Vector3(direction.x * Mathf.Sin(northHeading), direction.y,
            direction.z * Mathf.Cos(northHeading));
        // messages.text = sceneDirection.ToString();
        float distance = Locationf.Distance(playerCoordinates, objectInfo.ObjectCoordinates);
        Vector3 spawnPosition = transform.position + sceneDirection * distance;
        GameObject card = Instantiate(infoCard, spawnPosition, Quaternion.identity);
        createdCards.Add(card);
        card.transform.GetComponentsInChildren<Text>().FirstOrDefault(text =>
            text.gameObject.name.Equals("Name")).text = objectInfo.ObjectName;
        card.transform.GetComponentsInChildren<Text>().FirstOrDefault(text =>
            text.gameObject.name.Equals("Description")).text = objectInfo.ObjectDescription;
        card.transform.LookAt(transform);
        card.transform.Find("InfoCard").localPosition = Vector3.forward * objectInfo.ObjectRadius;
        card.GetComponent<InfoCardBehavior>().Info = objectInfo;
        card.GetComponent<InfoCardBehavior>().Player = gameObject;
        // messages.text = transform.position.ToString();
        // lineRenderer.SetPosition(0, transform.position);
        // lineRenderer.SetPosition(0, card.transform.position);
    }

    public void HideInfoCard(ObjectInfo objectInfo)
    {
        // messages.text = "Card deleted";
        GameObject unusedCard = createdCards.Find(card =>
            card.GetComponent<InfoCardBehavior>().Info.ObjectName.Equals(objectInfo.ObjectName));
        createdCards.Remove(unusedCard);
        Destroy(unusedCard);
    }

    public void AddObject(ObjectInfo objectInfo)
    {
        allObjects.Add(objectInfo);
    }

    public void RemoveObject(ObjectInfo objectInfo)
    {
        if (createdCards.Select(card => card.GetComponent<InfoCardBehavior>().
            Info.ObjectName.Equals(objectInfo.ObjectName)).SingleOrDefault())
        {
            HideInfoCard(objectInfo);
        }
        allObjects.Remove(objectInfo);
    }

    public void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            XmlHandler.SaveObjectsToDataFile(allObjects);
        }
    }
}
