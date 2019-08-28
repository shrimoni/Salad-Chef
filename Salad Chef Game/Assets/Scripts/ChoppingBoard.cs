using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoppingBoard : MonoBehaviour
{
    public bool isPlayerReadyToChop;
    public bool isPlayerChoppingVegetables;

    public List<Vegetable> choppedVegetables = new List<Vegetable>(); // List of chopped vegetables
    public Salad salad; // Salad that will be delivered to the customer

    public TextMesh description; // Description of the chopped vegetables

    public float vegetableChoppingTime = 3; // Amount of time taken to chop each vegetable111

    [SerializeField]private Player player;

    // Start is called before the first frame update
    void Start()
    {
        salad = new Salad();
    }

    // Update is called once per frame
    void Update()
    {
        // Checking if player to chop && player is not chopping any vegetables
        // Then start chopping vegetables
        if (isPlayerReadyToChop && !isPlayerChoppingVegetables && Input.GetKeyUp(player.chopKey))
        {
            if (player.vegetables.Count > 0)
            {
                // Make sure always the first item is placed on the chopping board
                StartCoroutine(ChopVegetables(player.vegetables[0]));
            }
            else
            {
                Debug.Log("No Vegetables, can't chop");
            }
        }

        // Check if the player is ready to pickup the salad
        if (isPlayerReadyToChop && !isPlayerChoppingVegetables && Input.GetKeyUp(player.pickupKey))
        {
            player.saladToBeDelivered = salad;
            //salad.vegetables.Clear();
            choppedVegetables.Clear();
            description.text = "";
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isPlayerReadyToChop = true;
            player = collision.GetComponent<Player>();
            player.UpdateDialogBox("Chop \nVeggies: " + player.chopKey + "\n Pickup: " + player.pickupKey);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isPlayerReadyToChop = false;
            player.HideDialogBox();
            player = null;
        }
    }

    /// <summary>
    /// Chop vegetables
    /// Chopping takes some amount of time, so that player has to wait 
    /// Once Chopping is done, the player can pickup and deliver it to the client
    /// </summary>
    /// <param name="vegetable"></param>
    /// <returns></returns>
    private IEnumerator ChopVegetables(Vegetable vegetable)
    {
        Debug.Log("Chopping vegetable: " + vegetable.vegetableType);
        player.UpdateDialogBox("Chopping..");
        player.canMove = false;
        isPlayerChoppingVegetables = true;
        player.vegetables.Remove(vegetable);
        yield return new WaitForSeconds(vegetableChoppingTime);
        choppedVegetables.Add(vegetable);
        salad.vegetables.Add(vegetable.vegetableType);
        isPlayerChoppingVegetables = false;
        Debug.Log("Vegetable Chopped: " + vegetable.vegetableType);
        player.canMove = true;
        player.UpdateDialogBox("Chop \nVeggies: "+player.chopKey+"\n Pickup: "+player.pickupKey);
        UpdateDescription(vegetable.name);
        player.UpdatePickedUpItems();
    }


    // Update the description of the chopped vegetables 
    private void UpdateDescription(string desc)
    {
        description.text += desc + "\n";
    }
}
