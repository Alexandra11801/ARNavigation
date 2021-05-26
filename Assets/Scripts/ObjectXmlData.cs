using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

[Serializable]
[XmlRoot("Object")]
public class ObjectXmlData
{
    [XmlElement()] public string ObjectName { get; set; }
    [XmlElement()] public string ObjectDescription { get; set; }
    [XmlElement()] public Vector3 ObjectCoordinates { get; set; }
    [XmlElement()] public float ObjectRadius { get; set; }
    [XmlElement()] public float TriggerRadius { get; set; }
    
    public ObjectXmlData() : this("New object", "", Vector3.zero, 0, 10)
    {
        
    }
    
    public ObjectXmlData(string name, string description, Vector3 coordinates, float radius, float triggerRadius)
    {
        ObjectName = name;
        ObjectDescription = description;
        ObjectCoordinates = coordinates;
        ObjectRadius = radius;
        TriggerRadius = triggerRadius;
    }

    public ObjectXmlData(ObjectInfo info) : this(info.ObjectName, info.ObjectDescription, info.ObjectCoordinates,
        info.ObjectRadius, info.TriggerRadius)
    {
        
    }
    
}
