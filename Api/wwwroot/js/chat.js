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
        //chat.innerHTML += `<br/> <b style="color:red">[${new Date(Date.now()).toLocaleTimeString()}]</b> <b>${username.innerText}:</b> ${msg.value}`;
        msg.value = "";
    }
}



function connect() {
    socket = new WebSocket(uri);

    socket.onopen = function(e) {
        console.log("connection opened");
    };

    socket.onclose = function(e) {
        console.log("connection closed");
    };

    socket.onmessage = function(e) {
        if (e.data.startsWith("userlist"))
        {
            let newUserlist = e.data.split(":")[1].split("&&&");
            userlist.innerHTML = "";
            newUserlist.forEach(user => {
                userlist.innerHTML += `${user} <br/> `;
            });
        }
        else
        {
            chat.innerHTML += `<br/> ${e.data}`;
            chat.scrollTop = chat.scrollHeight - chat.clientHeight;
        }
    }
}