using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour {

    public GameObject star;

    public Sprite chestOpen;

    public int quantity;

    private bool IsOpen = false;

    public void OpenChest()
    {
        CreateStar();
        gameObject.GetComponent<SpriteRenderer>().sprite = chestOpen;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsOpen)
        { 
            
            if (collision.gameObject.name == "Player")
            {
                if (Mathf.Abs((Mathf.Abs(collision.gameObject.transform.position.y) - (transform.position.y))) > 0.6f)
                {
                    OpenChest();
                    IsOpen = true;                
                }
            }
        }
    }

    public void CreateStar()
    {
        for (int i = 0; i < quantity; i++)
        {
            GameObject starsCreate = Instantiate(star, transform.position, Quaternion.identity);
            starsCreate.GetComponent<CircleCollider2D>().enabled = false;
            starsCreate.AddComponent<Rigidbody2D>();
            starsCreate.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-1.5f, 1.5f), 10.0f);
        }
    }
}
