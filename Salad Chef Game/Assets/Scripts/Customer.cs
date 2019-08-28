using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Customer class responsible for customer related activities
/// </summary>
public class Customer : MonoBehaviour
{
    public Transform waitingTimeIndicator; // Waiting Time indicator UI
    public Salad requestedSalad; // The Salad requested by the customer

    public TextMesh customerRequest; // Simple Text UI to print the customer request

    [SerializeField] private float waitingTime = 30f; // Waiting time for the Salad
    [SerializeField] private float timeLeft;
    [SerializeField] private float waitingTimePerVegetable = 5f; // simple number to calculate the waiting time
    [SerializeField] private bool isCustomerAngry; // to check if customer is angry or not
    [SerializeField] private bool isVegetablesForSaladCreated; // customer requested vegetables for a salad
    [SerializeField] private bool isPlayerReadyToDeliver; // to check if player is ready to deliver or no
    [SerializeField] private Player player;
    [SerializeField] private int scoreForDelivery = 10;
    [SerializeField] List<int> vegetableIndices = new List<int>(); // list to generate random vegetable salad request 
    private int minVegetables = 2; // minimum vegetables in the salad
    private int maxVegetables = 3; // maximum vegetables in the salad

    // Start is called before the first frame update
    void Start()
    {
        requestedSalad = new Salad();
        StartCoroutine(RequestSalad());
    }

    // Update is called once per frame
    void Update()
    {
        if (isVegetablesForSaladCreated)
        {
            // if customer waiting time is over
            if (timeLeft < 0)
            {
                GameManager.Instance.player1.UpdateScore(-5);
                GameManager.Instance.player2.UpdateScore(-5);
                GameManager.Instance.player1.HideDialogBox();
                GameManager.Instance.player2.HideDialogBox();
                ResetCustomer();
                Debug.Log("Time Over");
            }
            // if customer is angry 
            if (!isCustomerAngry)
                timeLeft -= Time.deltaTime;
            else
                timeLeft -= Time.deltaTime + 0.05f;
            var scale = waitingTimeIndicator.localScale;
            scale.x = timeLeft / waitingTime;
            waitingTimeIndicator.localScale = scale;
        }

        // if player is ready to deliver the salad
        if (player != null && isPlayerReadyToDeliver && Input.GetKeyDown(player.placeKey))
        {
            ReceiveSalad();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isPlayerReadyToDeliver = true;
            player = collision.GetComponent<Player>();
            player.UpdateDialogBox("Deliver \nSalad: "+player.placeKey);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isPlayerReadyToDeliver = false;
            if (player != null)
            {
                player.HideDialogBox();
                player = null;
            }

        }
    }

    /// <summary>
    /// Request the salad to the chefs
    /// </summary>
    private IEnumerator RequestSalad()
    {
        var nofVegetablesInSalad = Random.Range(minVegetables, maxVegetables + 1);
        Debug.Log("Creating Salad Request with " + nofVegetablesInSalad + "vegetables");
        StartCoroutine(CreateRandomVegetables(nofVegetablesInSalad));
        yield return new WaitUntil(() => isVegetablesForSaladCreated);
        waitingTime = waitingTimePerVegetable * nofVegetablesInSalad;
        timeLeft = waitingTime;
        foreach (var vegetable in vegetableIndices)
        {
            requestedSalad.vegetables.Add((VegetableType)vegetable);
            customerRequest.text += (VegetableType)vegetable + "\n";
        }
    }

    // Creates random indices for vegetables generation
    private IEnumerator CreateRandomVegetables(int vegetableCount)
    {
        int number;
        for (int i = 0; i < vegetableCount; i++)
        {
            do
            {
                number = Random.Range(0, 6);
            } while (vegetableIndices.Contains(number));
            vegetableIndices.Add(number);
        }
        yield return new WaitUntil(() => (vegetableCount == vegetableIndices.Count));
        isVegetablesForSaladCreated = true;
    }

    // Method that receives salad from the chef
    private void ReceiveSalad()
    {
        var isSaladCorrect = CheckIfSaladDeliveredIsCorrect(player.saladToBeDelivered);
        if (isSaladCorrect)
        {
            var percentage = waitingTimeIndicator.localScale.x * 100;
            if (percentage >= 70)
            {
                Debug.Log("Spawn Pickup");
                SpawnBooster();
            }
            player.UpdateScore(scoreForDelivery);
            Debug.Log("You get some points");
            player.ResetPlayer();
            player.HideDialogBox();

            ResetCustomer();
        }
        else
        {
            if(isCustomerAngry)
            {
                player.UpdateScore(-scoreForDelivery * 2);
            }
            isCustomerAngry = true;
            Debug.Log("You don't deserve points");
        }
    }

    // To check whether the deilvered salad is correct or not
    private bool CheckIfSaladDeliveredIsCorrect(Salad salad)
    {
        if (salad.vegetables.Count == requestedSalad.vegetables.Count)
        {
            Debug.Log("Vegetable counts are equal");
            for (int i = 0; i < requestedSalad.vegetables.Count; i++)
            {
                if (!salad.vegetables.Contains(requestedSalad.vegetables[i]))
                {
                    Debug.Log("Vegetables mismatch");
                    return false;
                }
            }
            Debug.Log("Vegetables Matched");
            return true;
        }
        return false;
    }

    // Spawn's booster if certain criteria meets by the chefs
    void SpawnBooster()
    {
        var pickupIndex = Random.Range(0, 4);
        Debug.Log(pickupIndex);
        var booster = Instantiate(GameManager.Instance.boosterPrefab);
        booster.boosterType = (BoosterType)pickupIndex;
        booster.ChangeBoosterSprite(booster.boosterType);
        booster.player = player;
    }

    // Reset's the customer to inital phase for new request
    void ResetCustomer()
    {
        requestedSalad.vegetables.Clear();
        vegetableIndices.Clear();
        StartCoroutine(RequestSalad());
        isVegetablesForSaladCreated = false;
        player = null;
        customerRequest.text = "";
        isCustomerAngry = false;
    }

}
