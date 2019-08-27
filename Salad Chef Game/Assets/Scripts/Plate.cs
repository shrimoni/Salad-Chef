using UnityEngine;

public class Plate : MonoBehaviour
{
    public Vegetable vegetable;

    [SerializeField] private bool isPlayerReadyToPlaceVegetable;

    private Player player;

    private void Update()
    {
        if (isPlayerReadyToPlaceVegetable && Input.GetKeyDown(KeyCode.G))
        {
            if (player.vegetables.Count > 0)
            {
                PlaceVegetable(player.vegetables[0]);
            }
            else
            {
                Debug.Log("Can't place vegetable, no vegetable in the inventory");
            }
        }

        if (isPlayerReadyToPlaceVegetable && Input.GetKeyDown(KeyCode.P))
        {
            PickUp();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isPlayerReadyToPlaceVegetable = true;
            player = collision.GetComponent<Player>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isPlayerReadyToPlaceVegetable = false;
            player = null;
        }
    }

    void PlaceVegetable(Vegetable veg)
    {
        vegetable = veg;
        player.vegetables.Remove(vegetable);
    }

    private void PickUp()
    {
        if (player == null)
            return;

        if (player.vegetables.Count < 2)
        {
            Debug.Log("Picking Up Me...");
            player.vegetables.Add(vegetable);
            vegetable = null;
        }

    }
}
