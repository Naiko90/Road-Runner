using UnityEngine;
using System.Collections;
using UnityEditor;
using Rug.Osc;
using System.Collections.Generic;

public class Movement : MonoBehaviour {

    public GameControlScript control;
    public GameObject RightWall;
    public GameObject LeftWall;
    public KeyCode KeyRight;
    public KeyCode KeyLeft;

    public float XVelocity = 0.6f;
    public float ZVelocity = 0.5f;

    public GameObject sendControllerObject;
    private OscSendController m_SendController;

    // Use this for initialization
    void Start()
    {
        OscSendController controller = sendControllerObject.GetComponent<OscSendController>();

        if (controller == null)
        {
            Debug.LogError(string.Format("The GameObject with the name '{0}' does not contain a OscSendController component", sendControllerObject.name));
            return;
        }

        m_SendController = controller;
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
        int x = 0, z = 0;

        VirtualToPhysicalCoordinates(other.gameObject.transform.localPosition, out x, out z);
        
        if (other.gameObject.tag == "PowerUp")
        {
            control.PowerupCollected();

            SetTilesTexture(GameObject.FindGameObjectWithTag("NiwController").GetComponent<NiwController>().GetFeetPosition(), "ice");

            //if (x != 0 && z != 0)
            //{
            //    Send(x, z, "ice");
            //}
        }
        else if (other.gameObject.tag == "Obstacle")
        {
            control.AlcoholCollected();

            SetTilesTexture(GameObject.FindGameObjectWithTag("NiwController").GetComponent<NiwController>().GetFeetPosition(), "can");

            //if (x != 0 && z != 0)
            //{
            //    Send(x, z, "can");
            //}
        }

        Destroy(other.gameObject);
    }

    void SetPosition(Vector3 v)
    {
        if (!control.isGameOver)
        {
            Vector3 position = Vector3.zero;
            if (!GameObject.Find("NiwController").GetComponent<NiwController>().HeadTrackingMovement)
            {
                position.x = Mathf.Lerp(RightWall.transform.localPosition.x + 1.0f, LeftWall.transform.localPosition.x - 1.0f, v.x);
                position.z = Mathf.Lerp(3.0f, -3.0f, v.z);
                position.y = transform.localPosition.y;
            }
            else
            {
                position = v;
                position.y = transform.localPosition.y; 
            }

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

    void VirtualToPhysicalCoordinates(Vector3 v, out int x, out int z)
    {
        // Compute constants to be used for the mapping
        float xConstant = 1.2f / GameObject.FindGameObjectWithTag("NiwController").GetComponent<NiwController>().bounds.extents.x;
        float zConstant = 1.2f / GameObject.FindGameObjectWithTag("NiwController").GetComponent<NiwController>().bounds.extents.z;
        
        // Subtract Bounds Center
        v.x -= (GameObject.FindGameObjectWithTag("NiwController").GetComponent<NiwController>().bounds.center.x + gameObject.transform.localPosition.x);
        v.z -= (GameObject.FindGameObjectWithTag("NiwController").GetComponent<NiwController>().bounds.center.z + gameObject.transform.localPosition.z);

        // Multiply subtracted values for the constants previously calculated
        v.x *= xConstant;
        v.z *= zConstant;

        // Add 0.9, so that we move everything in the origin
        v.x += 0.9f;
        v.z += 0.0f;

        // Define tile number
        if (v.x >= 0 && v.x <= 0.3)
        {
            x = 1;
        }
        else if (v.x > 0.3 && v.x <= 0.6)
        {
            x = 2;
        }
        else if (v.x > 0.6 && v.x <= 0.9)
        {
            x = 3;
        }
        else if (v.x > 0.9 && v.x <= 1.2)
        {
            x = 4;
        }
        else if (v.x > 1.2 && v.x <= 1.5)
        {
            x = 5;
        }
        else if (v.x > 1.5 && v.x <= 1.8)
        {
            x = 6;
        }
        else // Boundary
        {
            x = 0;
        }

        if (v.z >= 0 && v.z <= 0.3)
        {
            z = 6;
        }
        else if (v.z > 0.3 && v.z <= 0.6)
        {
            z = 5;
        }
        else if (v.z > 0.6 && v.z <= 0.9)
        {
            z = 4;
        }
        else if (v.z > 0.9 && v.z <= 1.2)
        {
            z = 3;
        }
        else if (v.z > 1.2 && v.z <= 1.5)
        {
            z = 2;
        }
        else if (v.z > 1.5 && v.z <= 1.8)
        {
            z = 1;
        }
        else // Boundary
        {
            z = 0;
        }
    }

    public void Send(OscMessage[] a)
    {
        // OscMessage[] a = { new OscMessage("/niw/preset", x, z, texture), new OscMessage("/niw/trigger", x, z), new OscMessage("/niw/preset", x, z, "none") };
        // OscMessage[] a = { new OscMessage("/niw/preset/all", texture), new OscMessage("/niw/trigger/all"), new OscMessage("/niw/preset/all", "none") };

        if (m_SendController != null && a.Length != 0)
        {
            // Send the message
            foreach (OscMessage m in a)
            {
                m_SendController.Sender.Send(m);
                Debug.Log(m); 
            }
        }
    }

    public void Send(int x, int z, string texture)
    {
        // OscMessage[] a = { new OscMessage("/niw/preset", x, z, texture), new OscMessage("/niw/trigger", x, z), new OscMessage("/niw/preset", x, z, "none") };
        OscMessage[] a = { new OscMessage("/niw/preset/all", texture), new OscMessage("/niw/trigger/all"), new OscMessage("/niw/preset/all", "none") };

        if (m_SendController != null)
        {
            // Send the message
            foreach (OscMessage m in a)
            {
                m_SendController.Sender.Send(m);
                Debug.Log(m);
            }
        }
    }

    /// <summary>
    /// Set texture for a specific tile
    /// </summary>
    /// <param name="x">X-axis tile number</param>
    /// <param name="z"> Z-axis tile number</param>
    /// <param name="texture">Texture to be set</param>
    /// <param name="trigger">If true, the texture will automatically be triggered</param>
    void SetTilesTexture(int x, int z, string texture, bool trigger = false)
    {
        Send(new[] { new OscMessage("/niw/preset", x, z, texture) });

        if(trigger)
        {
            TriggerAndResetOneTile(x, z);
        }
    }

    /// <summary>
    /// Set the same texture for all the tiles
    /// </summary>
    /// <param name="texture">Texture to be set</param>
    /// <param name="trigger">If true, the texture will automatically be triggered</param>
    void SetTilesTexture(string texture, bool trigger = false)
    {
        Send(new[] { new OscMessage("/niw/preset/all", texture) });

        if (trigger)
        {
            TriggerAndResetAllTiles();
        }
    }

    /// <summary>
    /// Set texture for all the tiles on which the haptic floor senses a foot
    /// </summary>
    /// <param name="b"></param>
    /// <param name="texture">Texture to be set</param>
    /// <param name="trigger">If true, the texture will automatically be triggered</param>
    void SetTilesTexture(bool[,] b, string texture, bool trigger = false)
    {
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                if (b[i, j])
                {
                    Send(new[] { new OscMessage("/niw/preset", i + 1, j + 1, texture) });
                }
            }
        }

        if (trigger)
        {
            TriggerAndResetAllTiles();
        }
    }

    void TriggerAndResetOneTile(int x, int z)
    {
        Send(new[] { new OscMessage("/niw/trigger", x, z), new OscMessage("/niw/preset", x, z, "none") });
    }

    void TriggerAndResetAllTiles()
    {
        Send(new[] { new OscMessage("/niw/trigger/all"), new OscMessage("/niw/preset/all", "none") });
    }
}
