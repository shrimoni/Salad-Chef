using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vegetable : MonoBehaviour
{
    [SerializeField] private bool isPlayerReadyToPick;
    [SerializeField] private Player player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerReadyToPick && Input.GetKeyDown(KeyCode.P))
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
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Debug.Log(collision.name);
            isPlayerReadyToPick = false;
            player = null;
        }
    }

    private void PickUp()
    {
        if (player == null)
            return;

        if(player.vegetables.Count < 2)
        {
            Debug.Log("Picking Up Me...");
            player.vegetables.Add(this);
        }

    }
}
