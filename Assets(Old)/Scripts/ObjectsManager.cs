using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ObjectsManager : MonoBehaviour
{
    [SerializeField] private GameObject infoCard;
    [SerializeField] private GameObject player;
    private Vector2 playerCoordinates;
    private List<GameObject> createdCards;
    [SerializeField] private Text messages;

    public Vector2 PlayerCoordinates
    {
        get => playerCoordinates;
        set => playerCoordinates = value;
    }

    public void createInfoCard(ObjectInfo objectInfo)
    {
        Vector3 spawnPosition = player.transform.position + (Vector3) objectInfo.ObjectCoordinates -
                                (Vector3) playerCoordinates;
        GameObject card = Instantiate(infoCard, spawnPosition, Quaternion.identity);
        createdCards.Add(card);
        card.transform.Find("Name").GetComponent<Text>().text = objectInfo.ObjectName;
        card.transform.Find("Description").GetComponent<Text>().text = objectInfo.ObjectDescription;
        card.transform.LookAt(player.transform);
    }

    public void hideInfoCard(ObjectInfo objectInfo)
    {
        GameObject unusedCard = createdCards.Find(card =>
            card.transform.Find("Name").GetComponent<Text>().text.Equals(objectInfo.ObjectName));
        Destroy(unusedCard);
        createdCards.Remove(unusedCard);
    }
}
