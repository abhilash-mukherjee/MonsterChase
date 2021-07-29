using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private GameObject[] playerList;
    private GameObject playerObject;
    private Transform player;
    private Vector3 tempPos;

    [SerializeField]
    private float minX;
    [SerializeField]
    private float maxX;
    void Start()
    {
        StartCoroutine(WaitUntilPlayerObjectIsInstantiated());
        
    }

    private IEnumerator WaitUntilPlayerObjectIsInstantiated()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            yield return null;

        }
        else
        {
            yield return new WaitUntil(() => GameObject.FindGameObjectWithTag("Player") != null);
            playerObject = GameObject.FindGameObjectWithTag("Player");
        }
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!playerObject)
        {
            StartCoroutine(WaitUntilPlayerObjectIsInstantiated());
            return;
        }
        player = playerObject.transform;
        tempPos = transform.position;
        tempPos.x = player.position.x;
        

        if(tempPos.x < minX)
        {
            tempPos.x = minX;
        }

        if(tempPos.x > maxX)
        {
            tempPos.x = maxX;
        }
        gameObject.transform.position = tempPos;
        

    }
}
