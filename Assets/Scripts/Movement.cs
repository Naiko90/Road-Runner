using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

    public GameControlScript control;
    public KeyCode KeyRight;
    public KeyCode KeyLeft;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update () {
        if (!control.isGameOver)
        {
            if (Input.GetKey(KeyRight))
            {
                if (gameObject.transform.localPosition.x < 4.65f)
                {
                    transform.Translate(new Vector2(0.2f, 0));
                }
            }
            else if (Input.GetKey(KeyLeft))
            {
                if (gameObject.transform.localPosition.x > -4.65f)
                {
                    transform.Translate(new Vector2(-0.2f, 0));
                }
            } 
        }
	}

    //check if the character collects the powerups or the snags
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Powerup(Clone)")
        {
            control.PowerupCollected();
        }
        else if (other.gameObject.name == "Obstacle(Clone)")
        {
            control.AlcoholCollected();
        }

        Destroy(other.gameObject);
    }
}
