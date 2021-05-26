using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class XmlHandler
{

    private static string dataFilePath = "jar:file://" + Application.persistentDataPath + "/DataFile.xml";
    private static string shortDataFilePath = Application.persistentDataPath + "/DataFile.xml";

    public static void CopyDataToPersistentData(string path)
    {
        WWW www = new WWW(path);
        while (!www.isDone) ;
        File.WriteAllBytes(shortDataFilePath, www.bytes);
        Debug.Log(www.text);
    }
    
    public static List<ObjectInfo> LoadObjectsFromDataFile()
    {
        // File.Delete(shortDataFilePath);
        if (!File.Exists(shortDataFilePath))
        {
            string path = "jar:file://" + Application.dataPath + "!/assets/DataFile.xml";
            CopyDataToPersistentData(path);
        }
        WWW www = new WWW(dataFilePath);
        while (!www.isDone) ;
        MemoryStream stream = new MemoryStream(www.bytes); 
        XmlSerializer serializer = new XmlSerializer(typeof(ObjectsXmlData));
        List<ObjectXmlData> xmlDataList = ((ObjectsXmlData)serializer.Deserialize(stream)).Objects;
        stream.Close();
        List<ObjectInfo> result = Enumerable.ToList(xmlDataList.Select(data => new ObjectInfo(data)));
        return result;
    }

    public static void SaveObjectsToDataFile(List<ObjectInfo> objects)
    {
        List<ObjectXmlData> xmlDataList = Enumerable.ToList(objects.Select(o => 
            new ObjectXmlData(o)));
        ObjectsXmlData data = new ObjectsXmlData(xmlDataList);
        StreamWriter writer = new StreamWriter(shortDataFilePath, false, Encoding.UTF8);
        XmlSerializer serializer = new XmlSerializer(typeof(ObjectsXmlData));
        serializer.Serialize(writer, data);
        writer.Close();
    }
    
}
