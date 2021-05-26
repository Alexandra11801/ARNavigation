using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ObjectInfo
{
    [SerializeField] private string objectName;
    [SerializeField] private string objectDescription;
    [SerializeField] private float objectRadius;
    [SerializeField] private Vector2 objectCoordinates;
    [SerializeField] private float triggerRadius;
    private bool isNear;
    private GameObject player;
    private Vector2 playerCoordinates;

    public string ObjectName => objectName;

    public string ObjectDescription => objectDescription;

    public float ObjectRadius => objectRadius;

    public Vector2 ObjectCoordinates => objectCoordinates;

    public GameObject Player
    {
        get => player;
        set => player = value;
    }

    public Vector2 PlayerCoordinates
    {
        get => playerCoordinates;
        set => playerCoordinates = value;
    }

    public bool IsNear()
    {
        bool result = !isNear && (objectCoordinates - playerCoordinates).magnitude > triggerRadius;
        isNear = result;
        return result;
    }

    public bool IsFar()
    {
        bool result = isNear && (objectCoordinates - playerCoordinates).magnitude > triggerRadius;
        isNear = !result;
        return result;
    }

}
