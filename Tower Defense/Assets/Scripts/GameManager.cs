using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public string userName;

    public int maxMessages = 25;

    public GameObject chatPanel, textObject;
    public InputField chatBox;

    public GameObject ChatLogView, ChatLogInput;

    public Color playerMessage, info, errorColor;

    [SerializeField] List<Message> messageList = new List<Message>();

    [HideInInspector] public List<Command> Commands = new List<Command>();

    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && chatBox.text == "")
        {
            if (ChatLogView.activeSelf == false)
            {
                ChatLogInput.SetActive(true);
                ChatLogView.SetActive(true);
            }
            else
            {
                ChatLogInput.SetActive(false);
                ChatLogView.SetActive(false);
            }
        }

        if(chatBox.text != "")
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if(chatBox.text[0] == '/')
                {
                    string[] arguments = chatBox.text.Split(' ');
                    string commandName = arguments[0].Substring(1);

                    Command command = Commands.Find(x => x.name == commandName);

                    if(command == null)
                    {
                        SendMessageToChat(userName + ": ERROR - Command not found", Message.MessageType.error);
                    }
                    else
                    {
                        string[] parameters = new string[arguments.Length - 1];

                        Array.Copy(arguments, 1, parameters, 0, parameters.Length);

                        command.action.Invoke(parameters);

                        SendMessageToChat(userName + ": " + commandName + " executed", Message.MessageType.info);
                    }

                    chatBox.text = "";
                }
                else
                {
                    SendMessageToChat(userName + ": " + chatBox.text, Message.MessageType.playerMessage);
                    chatBox.text = "";
                }
            }
        }
        else if (!chatBox.isFocused && Input.GetKeyDown(KeyCode.Return))
        {
            chatBox.ActivateInputField();
        }
    }

    public void SendMessageToChat(string text, Message.MessageType messageType)
    {
        if(messageList.Count >= maxMessages)
        {
            Destroy(messageList[0].textObject.gameObject);
            messageList.Remove(messageList[0]);
        }

        Message newMessage = new Message();

        newMessage.text = text;

        GameObject newText = Instantiate(textObject, chatPanel.transform);

        newMessage.textObject = newText.GetComponent<Text>();

        newMessage.textObject.text = newMessage.text;
        newMessage.textObject.color = MessageTypeColor(messageType);

        messageList.Add(newMessage);
    }

    private Color MessageTypeColor(Message.MessageType messageType)
    {
        Color color = info; //Default

        switch (messageType)
        {
            case Message.MessageType.error:
                color = errorColor;
                break;
            case Message.MessageType.playerMessage:
                color = playerMessage;
                break;
        }

        return color;
    }

    public void AddCommand(string commandName, UnityAction<string[]> commandAction)
    {
        Command newCommand = new Command(commandName, commandAction);
        Commands.Add(newCommand);
    }
}

[System.Serializable]
public class Message
{
    public string text;
    public Text textObject;
    public MessageType messageType;

    public enum MessageType
    {
        playerMessage,
        info,
        error
    };
}

[System.Serializable]
public class Command
{
    public string name;
    public UnityAction<string[]> action;

    public Command(string _name, UnityAction<string[]> _action)
    {
        name = _name;
        action = _action;
    }
}