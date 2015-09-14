using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

    public GameControlScript control;
    public GameObject RightWall;
    public GameObject LeftWall;
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

    void SetPosition(Vector3 v)
    {
        if (control.isGameOver)
        {
            var position = Vector3.zero;
            position.x = Mathf.Lerp(LeftWall.transform.localPosition.x - 1.0f, RightWall.transform.localPosition.x - 1, v.x);
            position.z = Mathf.Lerp(0, 25, v.z);
            position.y = transform.localPosition.y;
            gameObject.transform.localPosition = position;
        }
    }
}
