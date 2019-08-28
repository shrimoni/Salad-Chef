using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VegetableType
{
    Onion = 0,
    Tomato = 1,
    Carrot = 2,
    Capsicum = 3,
    Cucumber = 4,
    Mushrooms = 5
}

public class Vegetable : MonoBehaviour
{
    public VegetableType vegetableType; // type of the vegetable

    [SerializeField] private bool isPlayerReadyToPick;
    [SerializeField] private Player player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Check if player is ready to pickup
        if (isPlayerReadyToPick && Input.GetKeyDown(player.pickupKey))
        {
            PickUp();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);
        if (collision.tag == "Player")
        {
            isPlayerReadyToPick = true;
            player = collision.GetComponent<Player>();
            player.UpdateDialogBox(this.name + "\nPickup: "+player.pickupKey.ToString());

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Debug.Log(collision.name);
            isPlayerReadyToPick = false;
            player.HideDialogBox();
            player = null;
        }
    }

    // Method which allows the player to pickup the vegetable
    private void PickUp()
    {
        if (player == null)
            return;

        if(player.vegetables.Count < 2)
        {
            Debug.Log("Picking Up: "+this.name);
            player.vegetables.Add(this);
            player.UpdatePickedUpItems();
        }

    }
}
