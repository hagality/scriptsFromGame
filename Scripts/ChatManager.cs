using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Lidgren.Network;

public class ChatManager : MonoBehaviour
{
    private int maxLimitInChat = 20;
    private Queue<Message> Chat = new Queue<Message>();
    public Text contentText;

    public static ChatManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        InitializeMessages();
        RefreshChat();
    }

    public void BtnSendMsg()
    {
        NetOutgoingMessage message = Client.instance.client.CreateMessage();

        message.Write((byte)PacketType.Packet_To_Server.ChatMessage);
        message.Write((byte)msg.chatTypeEnum);
        message.Write((string)msg.whoSaid);
        message.Write((string)msg.whatSaid);
        message.Write((string)msg.name_chat);

        Client.instance.client.SendMessage(message, NetDeliveryMethod.ReliableOrdered);
        Client.instance.client.FlushSendQueue();
    }

    public void BtnCreatePrivateChat()
    {
        RequestCreatePrivateChat(Client.instance.client, GUIHandler.instance.inputChatNick.text);
    }

    public void AddMessageToChat(string whoSaid, string whatSaid, string name_chat, bool ownMessage = true)
    {
        Message msg = new Message(whoSaid, whatSaid, ChatTypeEnum.Default, name_chat);

        print(msg.ToString());

        if (msg.name_chat != "Default")
        {
            msg.chatTypeEnum = ChatTypeEnum.Private;

            if (ownMessage == false)
            {
                GUIHandler.instance.LastPrivateTextOnCenterScreen.GetComponent<OnOffComponent>().DisplayText(msg.whoSaid + ": " + msg.whatSaid);
            }
        }

        Chat.Enqueue(msg);

        if (Chat.Count > maxLimitInChat)
        {
            Chat.Dequeue();
        }

        RefreshChat();

        if (ownMessage == false)
        {
            if (name_chat == "Default")
            {
                for (int i = 0; i < BtnChatManager.ChatTypeOpen.Count; i++)
                {
                    if ((BtnChatManager.ChatTypeOpen[i].name_chat == "Default") && (BtnChatManager.name_chat != "Default"))
                    {
                        BtnChatManager.ChatTypeOpen[i].colorChange = true;
                    }
                }
            }
            else
            {
                for (int i = 0; i < BtnChatManager.ChatTypeOpen.Count; i++)
                {
                    if ((BtnChatManager.ChatTypeOpen[i].name_chat == whoSaid) && (BtnChatManager.name_chat != whoSaid))
                    {
                        BtnChatManager.ChatTypeOpen[i].colorChange = true;
                    }
                }
            }

        }
    }


    public void ResponseCreatePrivateDialog(NetIncomingMessage msg)
    {
        bool exist = msg.ReadBoolean();

        if (exist == true)
        {
            CreateNewPrivateDialog(msg.ReadString(), ChatTypeEnum.Private, true);
        }
    }

    public void RefreshChat(bool displayPrivateMessageOnCenter = false)
    {
        string tempContent = "";

        foreach (Message msg in Chat)
        {
            if (BtnChatManager.lastClickedEnum == ChatTypeEnum.Default)
            {
                if (msg.chatTypeEnum == ChatTypeEnum.Default)
                {



                    tempContent += "<color=orange>" + msg.whoSaid + ": " + msg.whatSaid + "</color>" + "\n";
                }
                else if (msg.chatTypeEnum == ChatTypeEnum.Private)
                {
                    if (BtnChatManager.ChatTypeIsOpen(msg.whoSaid) == false && msg.whoSaid != ClientGameManager.instance.mainPlayer.nick)
                    {
                        tempContent += "<color=aqua>" + msg.whoSaid + ": " + msg.whatSaid + "</color>" + "\n";

                    }
                }

            }

            else if (BtnChatManager.lastClickedEnum == ChatTypeEnum.Private)
            {
                if (msg.chatTypeEnum == ChatTypeEnum.Private)
                {
                    if (msg.whoSaid == BtnChatManager.name_chat)
                    {
                        tempContent += "<color=aqua>" + msg.whoSaid + ": " + msg.whatSaid + "</color>" + "\n";

                    }
                    else if (msg.whoSaid == ClientGameManager.instance.mainPlayer.nick && msg.name_chat == BtnChatManager.name_chat)
                    {
                        tempContent += "<color=blue>" + msg.whoSaid + ": " + msg.whatSaid + "</color>" + "\n";
                    }
                }


            }



        }


        contentText.text = tempContent;



    }


    private void ScrollDownChat()
    {
        if (Chat.Count > 5)
        {
            int val = Chat.Count - 5;
            val = Mathf.Abs(val * 20);

            contentText.GetComponent<RectTransform>().anchoredPosition = new Vector2(
              contentText.GetComponent<RectTransform>().anchoredPosition.x,
              val);

        }
    }

    private void InitializeMessages()
    {
        CreateNewPrivateDialog("Default", ChatTypeEnum.Default, false);
        BtnChatManager.BtnChangeLastCommunication(ChatTypeEnum.Default, "Default");

        Message msg1 = new Message("Tibia3d", "Welcome");
        Chat.Enqueue(msg1);
    }










    private void RequestCreatePrivateChat(NetClient client, string nick)
    {
        NetOutgoingMessage message = client.CreateMessage();

        message.Write((byte)PacketType.Packet_To_Server.Request_CreatePrivateChat);
        message.Write((string)nick);
        client.SendMessage(message, NetDeliveryMethod.ReliableOrdered);
        client.FlushSendQueue();
    }





    private void CreateNewPrivateDialog(string nick, ChatTypeEnum chatTypeEnum = ChatTypeEnum.Private, bool canClose = false)
    {
        ChatType newChatBtn = (Instantiate(GUIHandler.instance.prefabChatBtn, GUIHandler.instance.ChatSwitchPanel.transform)).GetComponent<ChatType>();
        newChatBtn._chatTypeEnum = chatTypeEnum;
        newChatBtn.name_chat = nick;
        newChatBtn.canClose = canClose;

        newChatBtn.Initialize();

        BtnChatManager.ChatTypeOpen.Add(newChatBtn);
        GUIHandler.instance.MenuCreateNewChat.SetActive(false);
        newChatBtn.GetComponent<Button>().onClick.AddListener(() => BtnChatManager.BtnChangeLastCommunication(newChatBtn._chatTypeEnum, newChatBtn.name_chat));


        BtnChatManager.BtnChangeLastCommunication(newChatBtn._chatTypeEnum, newChatBtn.name_chat);
        RefreshChat();
    }



}

public class Message
{
    public string whoSaid;
    public string whatSaid;
    public ChatTypeEnum chatTypeEnum;
    public string name_chat;

    public Message(string _whoSaid, string _whatSaid, ChatTypeEnum _chatTypeEnum = ChatTypeEnum.Default, string _name_chat = "Default")
    {
        whoSaid = _whoSaid;
        whatSaid = _whatSaid;
        chatTypeEnum = _chatTypeEnum;
        name_chat = _name_chat;
    }

    public override string ToString()
    {
        return "whoSaid: " + whoSaid + " " + "whatSaid: " + whatSaid + " " + "chatTypeEnum: " + chatTypeEnum + " " + "name_chat: " + name_chat;
    }
}


public static class BtnChatManager
{
    public static ChatTypeEnum lastClickedEnum;
    public static string name_chat;
    public static List<ChatType> ChatTypeOpen = new List<ChatType>();


    public static void BtnChangeLastCommunication(ChatTypeEnum chatType, string name_chat)
    {

        BtnChatManager.lastClickedEnum = chatType;
        BtnChatManager.name_chat = name_chat;


        ChatManager.instance.RefreshChat();

        for (int i = 0; i < BtnChatManager.ChatTypeOpen.Count; i++)
        {
            if (BtnChatManager.ChatTypeOpen[i].name_chat == name_chat)
            {
                BtnChatManager.ChatTypeOpen[i].colorChange = false;
            }
        }
    }

    public static bool ChatTypeIsOpen(string nameChat)
    {
        bool found = false;
        for (int i = 0; i < ChatTypeOpen.Count; i++)
        {
            if (ChatTypeOpen[i].name_chat == nameChat)
            {
                found = true;
                break;
            }
        }

        if (found)
        {
            return true;
        }
        else
        {
            return false;
        }


    }
}



