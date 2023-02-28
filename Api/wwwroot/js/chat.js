const chat = document.getElementById("chat-field");
const msg = document.getElementById("message");
const sendBtn = document.getElementById("send-msg-btn");
const username = document.getElementById("username-session-info");
const userlist = document.getElementById("users-list");

const uri = "ws://localhost:5002/ws";

connect();

sendBtn.onclick = () =>
{
    if (msg.value !== "")
    {
        socket.send(msg.value);
        msg.value = "";
    }
}

msg.onkeydown = (e) =>
{
    if (e.key == "Enter") {
        sendBtn.click();
    }
}

function connect()
{
    socket = new WebSocket(uri);

    socket.onopen = function(e)
    {

    };

    socket.onclose = function(e)
    {

    };

    socket.onmessage = function(e)
    {
        let chatMessage = JSON.parse(e.data);

        if(chatMessage.hasOwnProperty("bufferMessages"))
        {
            chatMessage.bufferMessages.forEach(message => { addBufferMessage(message); });
        }
        else if (chatMessage.hasOwnProperty("systemMessage"))
        {
            addSystemMessage(chatMessage.systemMessage);
        }
        else if (chatMessage.hasOwnProperty("userMessage"))
        {
            addUserMessage(chatMessage.userMessage);
        }
        else
        {
            userlist.innerHTML = "";
            chatMessage.userList.forEach(user => {  userlist.innerHTML += `${user} <br/> `; })
        }
    }
}

function addBufferMessage(msg)
{
    if (msg.MessageType == "connectMessage")
    {
        addConnectMessage(msg);
    }
    else if (msg.MessageType == "disconnectMessage")
    {
        addDisconnectMessage(msg);
    }
    else
    {
        addUserMessage(msg);
    }
}

function addUserMessage(msg)
{
    chat.innerHTML += `<br/> <b style=\"color:blue\">[${msg.Time}]</b> <b>${msg.Username}:</b> ${msg.Message}`;
}

function addSystemMessage(msg)
{
    if (msg.MessageType == "connectMessage")
    {
        addConnectMessage(msg);
    }
    else
    {
        addDisconnectMessage(msg);
    }
}

function addConnectMessage(msg)
{
    chat.innerHTML += `<br/> <b style=\"color:green\">[${msg.Time}]</b> <i><b>${msg.Username}</b> ${msg.Message}</i>`;
}

function addDisconnectMessage(msg)
{
    chat.innerHTML += `<br/> <b style=\"color:red\">[${msg.Time}]</b> <i><b>${msg.Username}</b> ${msg.Message}</i>`;
}