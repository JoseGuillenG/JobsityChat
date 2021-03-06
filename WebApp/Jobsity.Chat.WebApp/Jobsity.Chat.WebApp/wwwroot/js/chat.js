"use strict";
var apiEndpoint = "https://localhost:7235/";

var connection = new signalR.HubConnectionBuilder().withUrl(apiEndpoint + "chatHub").build();

//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    li.textContent = `${user} says ${message}`;
});

connection.start().then(function () {
    fetch(apiEndpoint + "api/Message")
        .then(response => response.json())
        .then(data => {
            for (var message of data) {
                var li = document.createElement("li");
                document.getElementById("messagesList").prepend(li);
                li.textContent = `${message}`;
            }
        });
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;

    var data = {
        Id: "id",
        UserName: user,
        Message: message,
        Code: "Test",
        ChatRoom: "room1",
        //MessageDateTime : new Date().getTime()
    };

    fetch(apiEndpoint + "api/Message", {
        method: 'POST',
        body: JSON.stringify(data),
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        }
    });
    event.preventDefault();
});