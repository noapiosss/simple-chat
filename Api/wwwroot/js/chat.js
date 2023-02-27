const chat = document.getElementById("chat-textarea");
const msg = document.getElementById("message");
const sendBtn = document.getElementById("send-msg-btn");
const username = document.getElementById("username-session-info");

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
        chat.innerHTML += `<br/> ${e.data}`;
        console.log(e.data);
    }
}