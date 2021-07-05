using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Chat : MonoBehaviour
{
    const int maxMessagesDisplayed = 25;

    [Header("References")]
    [SerializeField] GameObject textPrefab = null;
    [SerializeField] GameObject messagesContainer = null;
    [SerializeField] InputField input = null;

    [Header("Message colors")]
    [SerializeField] Color playerMessage = Color.white;
    [SerializeField] Color command = Color.green;
    [SerializeField] Color error = Color.red;

    List<Message> messageList = new List<Message>();
    List<Command> commands = new List<Command>();

    [Header("Chat Options")]
    [SerializeField] KeyCode changeVisibilityKey = KeyCode.Space;
    [SerializeField] KeyCode sendMessageKey = KeyCode.Return;
    [SerializeField] char commandPrefix = '/';

    void Start()
    {
        UI.Instance.Chat = this;
        changeUIVisibility();
    }
    
    public void CheckPlayerInput()
    {
        if (isPlayerChangingUIVisibility()) changeUIVisibility();

        if (isPlayerChangingInputFocus()) changeInputFocus();
        
        if (isPlayerSendingMessage()) sendMessage();
    }

    bool isPlayerChangingUIVisibility()
    {
        return Input.GetKeyDown(changeVisibilityKey) && input.text == "";
    }

    void changeUIVisibility()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    bool isPlayerSendingMessage()
    {
        return Input.GetKeyDown(sendMessageKey) && input.text != "";
    }

    bool messageIsCommand()
    {
        return input.text[0] == commandPrefix;
    }

    void changeInputFocus()
    {
        if (input.isFocused) input.DeactivateInputField();
        else input.ActivateInputField();
    }

    void sendCommand()
    {
        string[] arguments = input.text.Split(' ');
        string commandName = arguments[0].Substring(1);

        Command command = commands.Find(x => x.name == commandName);

        if (command == null) sendMessageToChat("ERROR - Command not found", Message.MessageType.error);
        else
        {
            string[] parameters = new string[arguments.Length - 1];
            Array.Copy(arguments, 1, parameters, 0, parameters.Length);
            command.action.Invoke(parameters);
            sendMessageToChat(commandName + " executed", Message.MessageType.info);
        }     
    }

    void sendMessage()
    {
        if (messageIsCommand()) sendCommand();
        else sendMessageToChat(input.text, Message.MessageType.playerMessage);

        input.text = "";
    }

    bool isPlayerChangingInputFocus()
    {
        return input.text == "" && Input.GetKeyDown(sendMessageKey);
    }

    void sendMessageToChat(string text, Message.MessageType messageType)
    {
        if (messageList.Count >= maxMessagesDisplayed) removeOldestMessage();

        Message newMessage = createNewMessage(text, messageType);

        messageList.Add(newMessage);
    }

    Message createNewMessage(string text, Message.MessageType messageType)
    {
        Message newMessage = new Message();
        newMessage.text = text;
        GameObject newText = Instantiate(textPrefab, messagesContainer.transform);
        newMessage.textObject = newText.GetComponent<Text>();
        newMessage.textObject.text = newMessage.text;
        newMessage.textObject.color = messageTypeColor(messageType);

        return newMessage;
    }

    void removeOldestMessage()
    {
        Destroy(messageList[0].textObject.gameObject);
        messageList.Remove(messageList[0]);
    }

    Color messageTypeColor(Message.MessageType messageType)
    {
        Color color = command; //Default

        switch (messageType)
        {
            case Message.MessageType.error:
                color = error;
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
        commands.Add(newCommand);
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