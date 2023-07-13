using System;
using System.Linq;
using System.Threading.Tasks;
using StreamChat.Core;
using StreamChat.Core.Helpers;
using StreamChat.Core.Models;
using StreamChat.Core.StatefulModels;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ChatManager : MonoBehaviour
{
    private IStreamChatClient _chatClient;
    private IStreamChannel _mainChannel;

    public TMP_InputField textBox;
    public GameManager gameManager;

    public Transform chatBoxCont;
    public GameObject chatBox;

    List<GameObject> chats = new List<GameObject>();

    private void Start()
    {
        _chatClient = StreamChatClient.CreateDefaultClient();
        
    }

    private void Update()
    {
        if (!_chatClient.IsConnected || _mainChannel == null)   return;

        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    var messageText = "Hello, world! Current local time is: " + DateTime.Now;
        //    SendMessageAsync(messageText).LogExceptionsOnFailed();
        //}

        if (Input.GetKeyDown(KeyCode.D))
        {
            if (_mainChannel.Messages.Count == 0)
            {
                Debug.LogWarning("No message to delete");
            }
            else
            {
                var lastMessage = _mainChannel.Messages.Last();
                lastMessage.HardDeleteAsync().LogExceptionsOnFailed();
            }
        }
    }

    public void OnSend()
    {
        string messageText = textBox.text;
        SendMessageAsync(messageText).LogExceptionsOnFailed();
        textBox.text = "";
    }

    public async Task StartChatAsync()
    {
        // Connect user
        var localUserData = await _chatClient.ConnectUserAsync("j99gtgbcrknz", gameManager.username, StreamChatClient.CreateDeveloperAuthToken(gameManager.username));
        Debug.Log($"User {localUserData.User.Id} is now connected!");

        // Create channel with "main" ID
        _mainChannel = await _chatClient.GetOrCreateChannelWithIdAsync(ChannelType.Livestream, "main");
        Debug.Log($"channel with ID: {_mainChannel.Id} created");

        // Subscribe to channel events so we can react to new messages, reactions, etc.
        _mainChannel.MessageReceived += OnMessageReceived;
        _mainChannel.MessageDeleted += OnMessageDeleted;
        _mainChannel.ReactionAdded += OnReactionAdded;

        UpdateChats();
    }

    void UpdateChats()
    {
        foreach (GameObject chat in chats)
        {
            Destroy(chat);
        }

        chats.Clear();

        foreach (var message in _mainChannel.Messages)
        {
            GameObject chatBoxGo = Instantiate(chatBox) as GameObject;
            chats.Add(chatBoxGo);
            Transform chatBoxTrans = chatBoxGo.transform;
            chatBoxTrans.SetParent(chatBoxCont, false);

            chatBoxTrans.GetChild(0).GetComponent<TMP_Text>().text = message.Text.ToString();
            chatBoxTrans.GetChild(1).GetComponent<TMP_Text>().text = message.User.Id.ToString();

            Debug.Log($"Channel message: {message.Text}, sent by: {message.User.Id} on {message.CreatedAt}");
        }
    }

    private async Task SendMessageAsync(string text)
    {
        var message = await _mainChannel.SendNewMessageAsync(text);
        UpdateChats();
    }

    private void OnMessageReceived(IStreamChannel channel, IStreamMessage message)
    {
        UpdateChats();
        //Debug.Log($"Message {message.Text} was received from {message.User.Id} in {channel.Id} channel");
    }

    private void OnMessageDeleted(IStreamChannel channel, IStreamMessage message, bool isHardDelete)
    {
        UpdateChats();
        Debug.Log($"Message with ID {message.Id} was deleted from {channel.Id} channel.");
    }

    private void OnReactionAdded(IStreamChannel channel, IStreamMessage message, StreamReaction reaction)
    {
        UpdateChats();
        Debug.Log($"Reaction {reaction.Type} was added by {reaction.User.Id}");
    }

    private void OnDestroy()
    {
        if (_mainChannel != null)
        {
            _mainChannel.MessageReceived += OnMessageReceived;
            _mainChannel.MessageDeleted += OnMessageDeleted;
            _mainChannel.ReactionAdded += OnReactionAdded;
        }
    }
}
