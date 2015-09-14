using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

using Rug.Osc;

public class NiwController : ReceiveOscBehaviourBase
{
    public GameObject Player;

    #region define CAVE parameters

    public const int tileRows = 6;
    public const int tileCols = 6;

    #endregion

    #region define OSC sender

    public GameObject sendControllerObject;
    private OscSendController m_SendController;

    // NIW must be initialized when java server is launched to start OSC streaming.
    // Since this takes ~10 seconds and the server can be kept running regardless of the Unity player,
    // turn off this flag to skip initialization procedure once the server is initialized.
    public bool doInitializeNiw = true;

    #endregion

    // Use this for initialization
	public override void Start ()
    {
        #region init receiver

        base.Start();

        #endregion

        #region init sender

        OscSendController controller = sendControllerObject.GetComponent<OscSendController>();

        if (controller == null)
        {
            Debug.LogError(string.Format("The GameObject with the name '{0}' does not contain a OscSendController component", sendControllerObject.name));
            return;
        }

        m_SendController = controller;

        #endregion

        #region init NIW

        if (doInitializeNiw)
        {
            Send(new OscMessage("/niw/server/config/invert/low/avg/zero", 0));
            Send(new OscMessage("/niw/server/push/invert/low/avg/zero/contactdetect", "aggregator/floorcontact"));
            Send(new OscMessage("/niw/server/config/invert/low", 0.025f));
            Send(new OscMessage("/niw/server/config/invert/low/avg/zero/contactdetect", 10000));
        }

        #endregion

    }

    // Update is called once per frame
    void Update () {
    }

    protected override void ReceiveMessage(OscMessage message) {
        // Debug.Log(message);
        
/*        // addresses must be listed in Inspector/Osc Addresses
        if (message.Address.Equals("/vicon/Position0"))
        {
            var v = new Vector3((float)(double)message[0], (float)(double)message[2] - 1, (float)(double)message[1]);
            playerController.transform.localPosition = v;
        }
        else if (message.Address.Equals("/vicon/Quaternion0"))
        {
            //var q = new Quaternion((float)(double)message[0], (float)(double)message[1], (float)(double)message[2], (float)(double)message[3]);
        }
        else*/ if (message.Address.Equals("/niw/client/aggregator/floorcontact"))
        {
            // Floor input
            int id = (int)message[1];
            float x = (((float)message[2]) / 6.0f);
            float z = (((float)message[3]) / 6.0f);
            var position = new Vector3(x, 0, z);
            Debug.Log(position);

            Player.GetComponent<Movement>().SendMessage("SetPosition", new Vector3(x, 0, z));

        }
    }

    public void Send(OscMessage msg)
    {

        if (m_SendController != null)
        {
            // Send the message
            m_SendController.Sender.Send(msg);
            Debug.Log(msg);
        }
    }

}
