using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoCardBehavior : MonoBehaviour
{
    private ObjectInfo info;
    private GameObject player;

    private Text messages;

    public ObjectInfo Info
    {
        get => info;
        set => info = value;
    }

    public GameObject Player
    {
        get => player;
        set => player = value;
    }

    public void Start()
    {
        messages = GameObject.Find("Messages").GetComponent<Text>();
    }

    public void Update()
    {
        transform.LookAt(player.transform.position);
    }

}
