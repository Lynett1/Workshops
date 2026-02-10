using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private Rigidbody rb;
    private float movementX;
    private float movementY;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement * speed * Time.fixedDeltaTime);
    }

   
}
