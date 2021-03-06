using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

namespace TwitchChat
{
    public class TwitchChatBot : MonoBehaviour
    {
        [SerializeField]
        private Sprite[] ConnectionIcon;

        public bool connected = false;
        private const string FileName = "ChatBotConfig";
        private const string path = FileName + ".cfg"; //"Assets/Resources/"+FileName+".txt";
        private const string Username = "username: ";
        private const string ChannelName = "channelName: ";
        private const string Password = "password: ";

        private const string Query = @":(.*)!.*@.*\.tmi\.twitch\.tv PRIVMSG #.* :(.*)";
     
        private TcpClient _twitchClient;
        private StreamReader _reader;
        private StreamWriter _writer;

        private string _username;
        private string _password;
        private string _channelName;

        public List<TextMessage> TextMessages { get; }

        public TwitchChatBot()
        {
            TextMessages = new List<TextMessage>();
        }

        private void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
            EvalConfig();
        }
        private void EvalConfig()
        {
            string resource = null;
            try
            {
                resource = File.ReadAllText(path);
            }
            catch (FileNotFoundException) { };

            if (resource == null)
            {
                Debug.Log("(!) Failed to open Config file: " + path + ", creating file...");
                File.Create(path);
                return;
            }
            else
            {
                // Debug.Log("ChatBot-Configuration was loaded succesfully!");
            }

            var lines = Regex.Split(resource, "\r\n|\r|\n");
            foreach (var line in lines)
            {
                if (line.StartsWith(Username))
                    _username = line.Substring(Username.Length);
                else if (line.StartsWith(ChannelName))
                    _channelName = line.Substring(ChannelName.Length);
                else if (line.StartsWith(Password))
                    _password = line.Substring(Password.Length);
            }

            if (string.IsNullOrWhiteSpace(_username)
                || string.IsNullOrWhiteSpace(_channelName)
                || string.IsNullOrWhiteSpace(_password))
            {
                Debug.LogError("ChatBot-Configuration not valid! Expected Content: 'username; channelName; password'");
                //gameObject.SetActive(false);
            }
        }

        private void Start()
        {   
            Connect();
        }

        private void Update()
        {
            if (!_twitchClient.Connected)
            {
                Connect();
            }
            
            ReadChat();
        }

        // username: chaos_to_go
        // channelName: chaos_to_go
        // password: oauth:x8tisyc91b191avctc7whf0be00wfs
        private void Connect()
        {
            _twitchClient = new TcpClient("irc.chat.twitch.tv", 6667);
            _reader = new StreamReader(_twitchClient.GetStream());
            _writer = new StreamWriter(_twitchClient.GetStream());
            
            _writer.WriteLine("PASS " + _password);
            _writer.WriteLine("NICK " + _username);
            _writer.WriteLine("USER " + _username + " 8 * :" + _username);
            _writer.WriteLine("JOIN #" + _channelName);
            _writer.Flush();

            char[] bytes = new char[_twitchClient.ReceiveBufferSize];
            _reader.Read(bytes, 0, (int)_twitchClient.ReceiveBufferSize);
            string returndata = new string(bytes);
            connected = returndata.Contains("Welcome, GLHF!");
            
            //Check if connection established & change notification/icon
            SetIcon(connected);
            if (connected)
            {
                GameObject.Find("ConnectionText1").GetComponent<Text>().text = "Success!";
                GameObject.Find("ConnectionText2").GetComponent<Text>().text = 
                    "Connected to User: " + _username;
            }
            else
            {
                GameObject.Find("ConnectionText1").GetComponent<Text>().text = "Failed Connection!";
                GameObject.Find("ConnectionText2").GetComponent<Text>().text =
                    "Could not connect to a Twitch Account with the entered username, password, token.";
            }

            Debug.Log("This is what the host returned to you: " + returndata);
        }

        public void Reconnect(string userName, string channelName, string oAuthToken)
        {
            File.WriteAllText(path, string.Empty);
            StreamWriter writer = new StreamWriter(path, true);
            writer.WriteLine("username: " + userName + " \r\nchannelName: " + channelName + " \r\npassword: " + oAuthToken);
            writer.Flush();
            writer.Close();
            EvalConfig();
            Connect();
        }

        private void SetIcon(bool connected)
        {
            Image Icon = GameObject.Find("ConnectionIcon").GetComponent<Image>();
            Icon.sprite = connected ? ConnectionIcon[0] : ConnectionIcon[1];
        }

        private void ReadChat()
        {
            if (_twitchClient.Available <= 0) return;
            
            var line = _reader.ReadLine();
            var textMessages = ExtractTextMessages(line);
            foreach (var message in textMessages)
            {
                //save message 
                TextMessages.Add(message);
                Debug.Log(message.ToString());
                
                //send message to twitch commander
                TwitchCommander.Instance.ProcessMessage(message);
            }
        }

        private IEnumerable<TextMessage> ExtractTextMessages(string input)
        {
            var result = new List<TextMessage>();
            
            if (string.IsNullOrWhiteSpace(input))
                return result;

            var matches = Regex.Matches(input, Query);

            if (matches.Count == 0)
                return result;
            
            for (var i = 0; i < matches.Count; i++)
            {
                var match = matches[i];
                if(match.Groups.Count < 3)
                    continue;
                
                var username = match.Groups[1].Value;
                var message = match.Groups[2].Value;

                if (!UserManager.Instance.UserExists(username))
                    MakeUserMod(username);
                
                var user = UserManager.Instance.RegisterOrUpdateUser(username);
                
                result.Add(TextMessage.Create(user, message));
            }
            
            return result;
        }
        //This can be used to override twitchs spam protection
        private void MakeUserMod(string username)
        {
        string[] job_titles = {
        "Executive Chef",
        "Chef de Cuisine",
        "Sous Chef",
        "Chef de Partie",
        "Sauté Chef",
        "Commis Chef",
        "Poissonnier",
        "Kitchen Porter",
        "Dishwasher",
        "Waitress"};

        int job_title = Random.Range(0, 10);


        _writer.WriteLine($"PRIVMSG #{_channelName} :Hello {job_titles[job_title]} {username}. Please get to work right away!");
         //   _writer.WriteLine($"PRIVMSG #{_channelName} :/mod {username}");
            _writer.Flush();
        }
    }
}
