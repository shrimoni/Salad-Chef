using UnityEngine;

public enum BoosterType
{
    Speed = 0,
    Time = 1,
    Score = 2

}

public class Booster : MonoBehaviour
{
    public BoosterType boosterType;
    public Player player;

    public Sprite timerSprite;
    public Sprite speedSprite;
    public Sprite scoreSprite;

    public int boosterScore = 5;
    public float boosterSpeed = 2f;
    public float boosterTime = 10f;

    [SerializeField]private SpriteRenderer spriteRenderer;

    private void Start()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (collision.GetComponent<Player>() == player)
            {
                if (boosterType == BoosterType.Score)
                {
                    player.UpdateScore(boosterScore);

                }
                else if (boosterType == BoosterType.Speed)
                {
                    player.UpdateSpeed(boosterSpeed);
                }
                else if (boosterType == BoosterType.Time)
                {
                    player.UpdateTimeLeft(boosterTime);
                }
                Destroy(gameObject);
            }
        }
    }

    // Chamges the booster sprite for the given booster type
    public void ChangeBoosterSprite(BoosterType bType)
    {
        switch(bType)
        {
            case BoosterType.Score:
                spriteRenderer.sprite = scoreSprite;
                break;
            case BoosterType.Speed:
                spriteRenderer.sprite = speedSprite;
                break;
            case BoosterType.Time:
                spriteRenderer.sprite = timerSprite;
                break;
            default:
                break;
        }
    }
}
