const chat = document.getElementById("chat-field");
const msg = document.getElementById("message");
const sendBtn = document.getElementById("send-msg-btn");
const ownUsername = document.getElementById("username-session-info");
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
            chatMessage.userList.forEach(user => {  userlist.innerHTML += `<div class="user-container">${user}</div>`; })
        }

        chat.scrollTop = chat.scrollHeight;
    }
}

function addBufferMessage(msg)
{
    if (msg.MessageType == "userMessage")
    {        
        addUserMessage(msg);
    }
    else
    {
        addSystemMessage(msg);
    }
}

function addUserMessage(msg)
{
    if (msg.Username === ownUsername.innerHTML)
    {
        chat.innerHTML += `<div class="own-chat-message"> <div class="own-message-text">${msg.Message}</div> <div class="own-chat-message-time">${msg.Time}</div> </div>`;
    }
    else
    {
        chat.innerHTML += `<div class="chat-message"> <div class="message-username">${msg.Username}</div> <div class=="message-text">${msg.Message}</div> <div class="chat-message-time">${msg.Time}</div> </div>`;
    }
}

function addSystemMessage(msg)
{
    chat.innerHTML += `<div class="system-chat-message"> <div class="system-message-text">${msg.Username} ${msg.Message}</div> </div>` ;
}