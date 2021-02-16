using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Net.Sockets;
using System.ComponentModel;

public class TwitchChat : MonoBehaviour
{
    private TcpClient twitchClient;
    private StreamReader reader;
    private StreamWriter writer;
    private string username, password, channelName;
    private bool sent = false;

    public string ConfigFileLocation;
    public Text chatBox;

    void Start()
    {
        ReadConfigFile(Application.dataPath + ConfigFileLocation);

        Connect();
    }

    void Update()
    {
        if (Math.Round(Time.time, 0) % 15 == 0)
        {
            if (!sent)
            {
                SendIrcMessage("NAMES simboblian");
                print(reader.ReadLine());
                sent = true;
            }
        }
        else
        {
            sent = false;
        }

        if (!twitchClient.Connected)
        {
            Connect();
        }

        ReadChat();
    }

    private void SendIrcMessage(string Message)
    {
        try
        {
            writer.WriteLine(Message);
            writer.Flush();
        }
        catch (Exception ex)
        {
            print(ex.Message);
        }
    }

    private void ReadConfigFile(string FilePath)
    {
        string[] lines = System.IO.File.ReadAllLines(@FilePath);

        // Display the file contents by using a foreach loop.
        username = lines[0];
        password = lines[1];
        channelName = lines[2];
    }

    private void Connect()
    {
        twitchClient = new TcpClient("irc.chat.twitch.tv", 6667);
        reader = new StreamReader(twitchClient.GetStream());
        writer = new StreamWriter(twitchClient.GetStream());

        writer.WriteLine("PASS " + password);
        writer.WriteLine("NICK " + username);
        writer.WriteLine("USER " + username + " 8 * : " + username);
        writer.WriteLine("CAP REQ :twitch.tv/membership");
        writer.WriteLine("JOIN #" + channelName);
        writer.Flush();
    }

    private void ReadChat()
    {
        if (twitchClient.Available > 0)
        {
            var message = reader.ReadLine();

            if (message.Contains("PRIVMSG"))
            {
                var splitPoint = message.IndexOf("!", 1);
                var chatName = message.Substring(0, splitPoint);
                chatName = chatName.Substring(1);

                splitPoint = message.IndexOf(":", 1);
                message = message.Substring(splitPoint + 1);
                //print(String.Format("{0}: {1}", chatName, message));
                chatBox.text = chatBox.text + "\n" + String.Format("{0}: {1}", chatName, message);

                if (message == "!join")
                {

                }
            }
        }
    }
}
