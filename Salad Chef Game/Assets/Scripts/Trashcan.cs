using UnityEngine;

public class Trashcan : MonoBehaviour
{

    [SerializeField] private bool isPlayerReadyToDisposeVegetables;

    [SerializeField] private Player player;
    [SerializeField] private int scoreForDisposing = -3;

    private void Update()
    {
        // If player is ready to dispose the salad
        if (isPlayerReadyToDisposeVegetables && Input.GetKeyDown(player.placeKey))
        {
            if (player.saladToBeDelivered.vegetables.Count > 0)
            {
                DisposeVegetables();
            }
            else
            {
                Debug.Log("Can't place vegetable, no vegetable in the inventory");
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isPlayerReadyToDisposeVegetables = true;
            player = collision.GetComponent<Player>();
            player.UpdateDialogBox("Dispose \nVeggie: " + player.placeKey);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isPlayerReadyToDisposeVegetables = false;
            player.HideDialogBox();
            player = null;
        }
    }

    private void DisposeVegetables()
    {

        Debug.Log("Disposing Salads");
        player.UpdateScore(scoreForDisposing);
        player.ResetPlayer();
        player.HideDialogBox();
    }

}
