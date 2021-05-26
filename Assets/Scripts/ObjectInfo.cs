using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ObjectInfo
{
    [SerializeField] private string objectName;
    [SerializeField] private string objectDescription;
    [SerializeField] private Vector3 objectCoordinates;
    [SerializeField] private float objectRadius;
    [SerializeField] private float triggerRadius;

    private Text messages;

    private bool isNear;
    
    private GameObject player;
    private Vector3 playerCoordinates;

    public string ObjectName => objectName;

    public string ObjectDescription => objectDescription;

    public Vector3 ObjectCoordinates => objectCoordinates;
    
    public float ObjectRadius => objectRadius;

    public float TriggerRadius => triggerRadius;

    public GameObject Player
    {
        get => player;
        set => player = value;
    }

    public Vector3 PlayerCoordinates
    {
        get => playerCoordinates;
        set => playerCoordinates = value;
    }

    public bool IsNear => isNear;

    public ObjectInfo(string name, string description, Vector3 coordinates, float radius, float triggerRadius)
    {
        objectName = name;
        objectDescription = description;
        objectCoordinates = coordinates;
        objectRadius = radius;
        this.triggerRadius = triggerRadius;
    }

    public ObjectInfo() : this("New object", "", Vector3.zero, 0, 10)
    {
        
    }

    public ObjectInfo(ObjectXmlData xmlData) : this(xmlData.ObjectName, xmlData.ObjectDescription,
        xmlData.ObjectCoordinates, xmlData.ObjectRadius, xmlData.TriggerRadius)
    {
        
    }
    
    public bool GetCloser()
    {
        if (!isNear && Locationf.Distance(playerCoordinates, objectCoordinates)
            <= objectRadius + triggerRadius)
        {
            // messages.text = "Get Closer";
            isNear = true;
            return true;
        }
        return false;
    }
    
    public bool GetAway()
    {
        if (isNear && Locationf.Distance(playerCoordinates, objectCoordinates)
            > objectRadius + triggerRadius)
        {
            // messages.text = "Get Away";
            isNear = false;
            return true;
        }
        return false;
    }
}
