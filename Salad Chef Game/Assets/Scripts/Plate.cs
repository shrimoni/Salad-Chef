using UnityEngine;

public class Plate : MonoBehaviour
{
    public Vegetable vegetable; // Vegetable in the plate

    [SerializeField] private bool isPlayerReadyToPlaceVegetable;
    [SerializeField] private TextMesh itemsInPlateDesc; // description of the vegetable placed in the plate

    private Player player;

    private void Update()
    {
        // If Player places vegetable in the plate
        if (isPlayerReadyToPlaceVegetable && Input.GetKeyDown(player.placeKey))
        {
            if (player.vegetables.Count > 0)
            {
                PlaceVegetableOnPlate(player.vegetables[0]); // place the firstmost vegetable
            }
            else
            {
                Debug.Log("Can't place vegetable, no vegetable in the inventory");
            }
        }

        // If Player picks up the vegetable from the plate
        if (isPlayerReadyToPlaceVegetable && Input.GetKeyDown(player.pickupKey))
        {
            PickVegetableFromPlate();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isPlayerReadyToPlaceVegetable = true;
            player = collision.GetComponent<Player>();
            player.UpdateDialogBox("Place \nVeggie: " + player.placeKey);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isPlayerReadyToPlaceVegetable = false;
            player.HideDialogBox();
            player = null;
        }
    }

    // Places the Vegetable on the table
    private void PlaceVegetableOnPlate(Vegetable veg)
    {
        vegetable = veg;
        itemsInPlateDesc.text = veg.name;
        player.vegetables.Remove(vegetable);
        player.UpdatePickedUpItems();
    }

    private void PickVegetableFromPlate()
    {
        if (player == null)
            return;

        if (player.vegetables.Count < 2)
        {
            Debug.Log("Picking Up Me...");
            player.vegetables.Add(vegetable);
            vegetable = null;
            itemsInPlateDesc.text = "";
        }

    }
}
