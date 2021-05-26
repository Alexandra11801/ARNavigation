using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectsRemovingHandler : MonoBehaviour
{
    [SerializeField] private Button openMenuButton;
    [SerializeField] private Button closeMenuButton;
    [SerializeField] private GameObject menu;

    private List<Toggle> toggles;
    [SerializeField] private Sprite toggleBackgroundSprite;
    [SerializeField] private Sprite toggleCheckmarkSprite;
    [SerializeField] private float toggleWidth;
    [SerializeField] private float toggleHeight;
    [SerializeField] private int labelFontSize;
    
    [SerializeField] private Text noObjectsMessage;
    [SerializeField] private Button objectsRemovingButton;
    
    [SerializeField] private ObjectsManager objectsManager;

    public void Start()
    {
        openMenuButton.onClick.AddListener(OpenMenu);
        closeMenuButton.onClick.AddListener(CloseMenu);
        objectsRemovingButton.onClick.AddListener(RemoveSelectedObjects);
    }
    
    public void OpenMenu()
    {
        menu.SetActive(true);
        toggles = FillObjectsList();
        Debug.Log(toggles.Count);
        if (toggles.Count > 0)
        {
            objectsRemovingButton.gameObject.SetActive(true);
            noObjectsMessage.gameObject.SetActive(false);
        }
        else
        {
            objectsRemovingButton.gameObject.SetActive(false);
            noObjectsMessage.gameObject.SetActive(true);
        }
    }

    public List<Toggle> FillObjectsList()
    {
        List<Toggle> result = new List<Toggle>();
        if (objectsManager.AllObjects.Count != 0)
        {
            for(int i = 0; i < objectsManager.AllObjects.Count; i++)
            {
                ObjectInfo objectInfo = objectsManager.AllObjects[i];
                DefaultControls.Resources resources = new DefaultControls.Resources();
                resources.background = toggleBackgroundSprite;
                resources.checkmark = toggleCheckmarkSprite;
                GameObject newToggle = DefaultControls.CreateToggle(resources);
                newToggle.transform.parent = menu.transform;
                RectTransform rectTransform = newToggle.GetComponent<RectTransform>();
                rectTransform.pivot = Vector2.up;
                rectTransform.sizeDelta = new Vector2(toggleWidth, toggleHeight);
                newToggle.transform.localPosition = new Vector3(-250, 250 - i * toggleHeight);
                Text label = newToggle.GetComponentInChildren<Text>();
                label.text = objectInfo.ObjectName;
                label.alignment = TextAnchor.MiddleLeft;
                label.fontSize = labelFontSize;
                label.transform.localPosition += Vector3.right * 20;
                newToggle.GetComponentInChildren<Image>().transform.localPosition += 
                    Vector3.down * toggleHeight / 4;
                newToggle.GetComponent<Toggle>().isOn = false;
                result.Add(newToggle.GetComponent<Toggle>());
            }
        }
        return result;
    }

    public void CloseMenu()
    {
        toggles.Clear();
        menu.SetActive(false);
    }

    public void RemoveSelectedObjects()
    {
        foreach (Toggle toggle in toggles)
        {
            if (toggle.isOn)
            {
                ObjectInfo objectInfo = objectsManager.AllObjects.Find(info =>
                    info.ObjectName.Equals(toggle.GetComponentInChildren<Text>().text));
                objectsManager.RemoveObject(objectInfo);
                Destroy(toggle.gameObject);
            }
        }
        CloseMenu();
    }

}
