using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

[Serializable]
[XmlRoot("Objects")]
public class ObjectsXmlData
{
    [XmlElement()]public List<ObjectXmlData> Objects { get; set; }

    public ObjectsXmlData(List<ObjectXmlData> objects)
    {
        Objects = objects;
    }

    public ObjectsXmlData() : this(new List<ObjectXmlData>())
    {
        
    }
    
}
