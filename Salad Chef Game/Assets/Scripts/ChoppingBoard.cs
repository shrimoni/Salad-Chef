using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoppingBoard : MonoBehaviour
{
    public bool isPlayerReadyToChop;
    public bool isPlayerChoppingVegetables;

    public List<Vegetable> choppedVegetables = new List<Vegetable>();

    [SerializeField]private Player player;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Checking if player to chop && player is not chopping any vegetables
        // Then start chopping vegetables
        if (isPlayerReadyToChop && !isPlayerChoppingVegetables && Input.GetKeyUp(KeyCode.C))
        {
            if(player.vegetables.Count > 0)
            {
                // Make sure always the first item is placed on the chopping board
                StartCoroutine(ChopVegetables(player.vegetables[0]));
            }
            else
            {
                Debug.Log("No Vegetables, can't chop");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isPlayerReadyToChop = true;
            player = collision.GetComponent<Player>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isPlayerReadyToChop = false;
            player = null;
        }
    }

    /// <summary>
    /// Chop vegetables
    /// Chopping takes some amount of time, so that player has to wait 
    /// Once Chopping is done it will move to the salad plate
    /// </summary>
    /// <param name="vegetable"></param>
    /// <returns></returns>
    private IEnumerator ChopVegetables(Vegetable vegetable)
    {
        Debug.Log("Chopping vegetable: " + vegetable.vegetableType);
        player.canMove = false;
        isPlayerChoppingVegetables = true;
        player.vegetables.Remove(vegetable);
        yield return new WaitForSeconds(5f);
        choppedVegetables.Add(vegetable);
        isPlayerChoppingVegetables = false;
        Debug.Log("Vegetable Chopped: " + vegetable.vegetableType);
        player.canMove = true;
    }
}
