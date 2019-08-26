using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Player : MonoBehaviour
{
    // The speed at which the player moves
    public float playerMovementspeed; 

    // Key Inputs name which will be varying depends on the player 
    public string hMovementKeyName = "Horizontal";
    public string vMovementKeyName = "Vertical";

    public List<Vegetable> vegetables = new List<Vegetable>();

    // Rigidbody for physics
    private Rigidbody2D rb2D;
    private BoxCollider2D boxCollider2D;

    // Movement parameters
    private Vector2 moveVelocity;
    private float hMovement;
    private float vMovement;

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        // Getting user inputs
        // Calculating movement based on the user input
        hMovement = Input.GetAxisRaw(hMovementKeyName);
        vMovement = Input.GetAxisRaw(vMovementKeyName);
        moveVelocity = new Vector2(hMovement, vMovement).normalized * playerMovementspeed;
    }

    private void FixedUpdate()
    {
        // Moves the player
        rb2D.MovePosition(rb2D.position + moveVelocity * Time.fixedDeltaTime);
    }

}
