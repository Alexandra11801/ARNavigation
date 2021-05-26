using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectsAddingHandler : MonoBehaviour
{
    [SerializeField] private Button openMenuButton;
    [SerializeField] private Button closeMenuButton;
    [SerializeField] private GameObject menu;
    
    [SerializeField] private Toggle setCurrentLocationToggle;
    private bool locationSettingMethodDefault;
    [SerializeField] private InputField[] coordinatesFields = new InputField[3];
    [SerializeField] private InputField nameField;
    [SerializeField] private InputField descriptionField;
    [SerializeField] private InputField radiusField;
    [SerializeField] private InputField triggerRadiusField;
    [SerializeField] private Button addObjectButton;
    
    [SerializeField] private Vector3 defaultCoordinates;
    [SerializeField] private string defaultName;
    [SerializeField] private string defaultDescription;
    [SerializeField] private float defaultRadius;
    [SerializeField] private float defaultTriggerRadius;

    [SerializeField] private LocationHandler locationHandler;
    [SerializeField] private ObjectsManager objectsManager;

    [SerializeField] private Text messages;
        
    public void Start()
    {
        openMenuButton.onClick.AddListener(OpenMenu);
        closeMenuButton.onClick.AddListener(CloseMenu);
        setCurrentLocationToggle.onValueChanged.AddListener(SetCurrentLocation);
        foreach (InputField field in coordinatesFields)
        {
            field.onValueChanged.AddListener(ProceedToManualCoordinatesSetting);
        }
        addObjectButton.onClick.AddListener(AddObject);
    }

    public void OpenMenu()
    {
        menu.SetActive(true);
    }

    public void CloseMenu()
    {
        menu.SetActive(false);
    }

    public void SetCurrentLocation(bool change)
    {
        if (change)
        {
            Vector3 currentCoordinates = locationHandler.CurrentCoordinates;
            coordinatesFields[0].text = currentCoordinates.x.ToString();
            coordinatesFields[1].text = currentCoordinates.z.ToString();
            coordinatesFields[2].text = currentCoordinates.y.ToString();
            locationSettingMethodDefault = true;
        }
    }

    public void ProceedToManualCoordinatesSetting(string value)
    {
        if (locationSettingMethodDefault)
        {
            setCurrentLocationToggle.isOn = false;
            locationSettingMethodDefault = false;
        }
    }

    public bool ValuesValid()
    {
        foreach (InputField field in coordinatesFields)
        {
            if (!float.TryParse(field.text, out _))
            {
                return false;
            }
        }
        if (!float.TryParse(radiusField.text, out _))
        {
            return false;
        }
        if (!float.TryParse(triggerRadiusField.text, out _))
        {
            return false;
        }
        if (objectsManager.AllObjects.Exists(o => o.ObjectName.Equals(nameField.text)))
        {
            return false;
        }
        return true;
    }

    public void AddObject()
    {
        if (ValuesValid())
        {
            Vector3 coordinates = new Vector3(float.Parse(coordinatesFields[0].text), 
                float.Parse(coordinatesFields[2].text), float.Parse(coordinatesFields[1].text));
            string name = nameField.text;
            string description = descriptionField.text;
            float radius = float.Parse(radiusField.text);
            float triggerRadius = float.Parse(triggerRadiusField.text);
            ObjectInfo newObject = new ObjectInfo(name, description, coordinates, radius, triggerRadius);
            objectsManager.AddObject(newObject);
            SetDefaultValues();
            CloseMenu();
        }
    }

    public void SetDefaultValues()
    {
        coordinatesFields[0].text = defaultCoordinates.x.ToString();
        coordinatesFields[1].text = defaultCoordinates.z.ToString();
        coordinatesFields[2].text = defaultCoordinates.y.ToString();
        setCurrentLocationToggle.isOn = false;
        nameField.text = defaultName;
        descriptionField.text = defaultDescription;
        radiusField.text = defaultRadius.ToString();
        triggerRadiusField.text = defaultTriggerRadius.ToString();
    }
}
