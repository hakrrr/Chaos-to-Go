using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace TwitchChat
{

    public class TwitchCommander : Singleton<TwitchCommander>
    {
        public delegate void MessageAction(string chatMsg);

        public event MessageAction OnMessage;

        public CommandPair[] commandListUser;

        private Dictionary<string, UnityEvent> commandList;

        // Start is called before the first frame update
        void Start()
        {

            //convert the commandListUser into a dictionary for easy use

            commandList = new Dictionary<string, UnityEvent>();

            for (int i = 0; i < commandListUser.Length; i++)
            {
                commandList[commandListUser[i].commandString] = commandListUser[i].commandEvent;
            }



            
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ProcessMessage(TextMessage newMessage)
        {
            //Determine if the new message corresponds to any of the predetermined commands
            if (commandList.ContainsKey(newMessage.Message))
            {
                commandList[newMessage.Message].Invoke();
            }
            else //else send the message to any of the listeners who are waiting for random commands
            {
                if (OnMessage != null)
                {
                    OnMessage(newMessage.Message);
                }
            }
        }
    }
}


[System.Serializable]
public class CommandPair
{
    public string commandString;
    public UnityEvent commandEvent;
}