using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Iris : MonoBehaviour
{
    public GameObject arcam;
    public float rotationSpeed;
    public float movementSpeed;

    private bool movement;
    private AudioSource audioSource;
    private Vector3 targetPos;

    private void Start()
    {
        movement = false;
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(!movement)
        {
            UpdateRotation();
        }
        else
        {
            MoveIrisToPlayer();
        }
    }

    public void SetMovement(bool value)
    {
        movement = value;
        targetPos = arcam.transform.position + arcam.transform.forward;
        audioSource.Play();
    }

    private void UpdateRotation()
    {
        Vector3 direction = arcam.transform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }

    private void MoveIrisToPlayer()
    {
        transform.position = Vector3.Lerp(transform.position, targetPos, movementSpeed);
        if(transform.position == targetPos)
        {
            movement = false;
        }
    }
}
