using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Player : MonoBehaviour
{
    // The speed at which the player moves
    public float playerMovementspeed;

    // Key Inputs name which will be varying depends on the player 
    public string hMovementKeyName = "Horizontal";
    public string vMovementKeyName = "Vertical";

    // Input keys
    public KeyCode pickupKey;
    public KeyCode placeKey;
    public KeyCode chopKey;

    public List<Vegetable> vegetables = new List<Vegetable>(); // list of vegetables in player's hand
    public Salad saladToBeDelivered; // the salad which player will be delivered to the client

    public TextMesh messageDisplay; // info message to the player
    public GameObject dialogueBox; // dialog box to hold the message

    public bool isPlayerChoppingVegetable;
    public bool canMove;

    // Player Parameters
    public TextMesh timeText;
    public TextMesh scoreText;
    public int score;

    public TextMesh pickedUpItems;

    public float timeLeft = 120f;

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
        canMove = true;
        saladToBeDelivered = new Salad();
        dialogueBox.SetActive(false);
        UpdateScore(0);
    }

    private void Update()
    {
        timeLeft -= Time.deltaTime;
        timeText.text = "Time: " + (int)timeLeft + 's';

        if (canMove)
        {
            // Getting user inputs
            // Calculating movement based on the user input
            hMovement = Input.GetAxisRaw(hMovementKeyName);
            vMovement = Input.GetAxisRaw(vMovementKeyName);
            moveVelocity = new Vector2(hMovement, vMovement).normalized * playerMovementspeed;
        }
    }

    private void FixedUpdate()
    {
        var pos = rb2D.position + moveVelocity * Time.fixedDeltaTime;
        pos.x = Mathf.Clamp(pos.x, GameManager.Instance.clampPositionLeft.position.x, GameManager.Instance.clampPositionRight.position.x);
        pos.y = Mathf.Clamp(pos.y, GameManager.Instance.clampPositionBottom.position.y, GameManager.Instance.clampPositionTop.position.y);
        // Moves the player
        rb2D.MovePosition(pos);
    }

    // Updates the score of the player
    public void UpdateScore(int playerScore)
    {
        score += playerScore;
        scoreText.text = "Score: " + score;
    }

    // Updates the time left
    public void UpdateTimeLeft(float time)
    {
        timeLeft += time;
    }

    // Update the speed of the player
    public void UpdateSpeed(float multiplier)
    {
        Debug.Log("Updating Speed");
        playerMovementspeed = playerMovementspeed * multiplier;
        StartCoroutine(RestoreSpeedAfterSecs(multiplier));
    }

    // Restore the speed after the booster is over
    IEnumerator RestoreSpeedAfterSecs(float multiplier)
    {
        yield return new WaitForSeconds(5f);
        playerMovementspeed = playerMovementspeed / multiplier;
        Debug.Log("Speed Updated");
    }

    // Reset's the player for next iteration/game
    public void ResetPlayer()
    {
        saladToBeDelivered.vegetables.Clear();
        vegetables.Clear();
        UpdatePickedUpItems();
    }

    // Updates the info on the dialog box
    public void UpdateDialogBox(string dialog)
    {
        dialogueBox.SetActive(true);
        messageDisplay.text = dialog;
    }

    // Hides the dialog box
    public void HideDialogBox()
    {
        dialogueBox.SetActive(false);
    }

    // Update the pickedup items, to the player screen
    public void UpdatePickedUpItems()
    {
        pickedUpItems.text = "PickedUp: ";
        foreach (var item in vegetables)
        {
            pickedUpItems.text += item.name + " ";
        }

    }

}
