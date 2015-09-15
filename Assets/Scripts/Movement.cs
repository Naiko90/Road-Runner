using UnityEngine;
using System.Collections;
using UnityEditor;


public class Movement : MonoBehaviour {

    public GameControlScript control;
    public GameObject RightWall;
    public GameObject LeftWall;
    public KeyCode KeyRight;
    public KeyCode KeyLeft;

    public float XVelocity = 0.6f;
    public float ZVelocity = 0.5f;
        
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
        if (!control.isGameOver)
        {
            var position = Vector3.zero;
            position.x = Mathf.Lerp(RightWall.transform.localPosition.x + 1.0f, LeftWall.transform.localPosition.x - 1.0f, v.x);
            position.z = Mathf.Lerp(0.0f, -6.0f, v.z);
            position.y = transform.localPosition.y;
            // gameObject.transform.localPosition = position;

            float distanceX = gameObject.transform.localPosition.x - position.x;
            if (distanceX > 0)
            {
                if (System.Math.Abs(distanceX) > XVelocity)
                {
                    position.x = transform.localPosition.x - XVelocity;
                }
                else
                {
                    position.x = transform.localPosition.x;
                }
            }
            else if (distanceX < 0)
            {
                if (System.Math.Abs(distanceX) > XVelocity)
                {
                    position.x = transform.localPosition.x + XVelocity; ;
                }
                else
                {
                    position.x = transform.localPosition.x;
                }
            }

            float distanceZ = gameObject.transform.localPosition.z - position.z;
            if (distanceZ > 0)
            {
                if (System.Math.Abs(distanceZ) > ZVelocity)
                {
                    position.z = transform.localPosition.z - ZVelocity;
                }
                else
                {
                    position.z = transform.localPosition.z;
                }
            }
            else if (distanceZ < 0)
            {
                if (System.Math.Abs(distanceZ) > ZVelocity)
                {
                    position.z = transform.localPosition.z + ZVelocity;
                }
                else
                {
                    position.z = transform.localPosition.z;
                }
            }

            gameObject.transform.localPosition = position;
        }
    }
}
