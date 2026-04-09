using UnityEngine;

public enum ItemType { Coin, Question, Good, Skull }

public class Item : MonoBehaviour
{
    public ItemType itemType;
    public int value; // cuántas monedas suma, o cuánta vida quita/da

    void OnTriggerEnter2D(Collider2D other)
    {
        // se destruye al tocar el suelo o una plataforma
        if (other.CompareTag("floor") || other.CompareTag("platform"))
        {
            Destroy(gameObject);
            return;
        }

        if (!other.CompareTag("Player")) return;

        switch (itemType)
        {
            case ItemType.Coin:
                GameControl.Instance.AddCoins(value);
                GameControl.Instance.sfxManager.GoodSound();
                break;
            case ItemType.Question:
                GameControl.Instance.LoadQuestion();
                break;
            case ItemType.Good:
                GameControl.Instance.AddWatering(value);
                GameControl.Instance.sfxManager.GoodSound();
                break;
            case ItemType.Skull:
                GameControl.Instance.TakeDamage(value);
                GameControl.Instance.sfxManager.BadSound();
                break;
        }
        Destroy(gameObject);
    }
}
