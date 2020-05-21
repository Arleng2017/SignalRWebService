// The following sample code uses modern ECMAScript 6 features 
// that aren't supported in Internet Explorer 11.
// To convert the sample for environments that do not support ECMAScript 6, 
// such as Internet Explorer 11, use a transpiler such as 
// Babel at http://babeljs.io/.

"use strict";

var connection = new signalR.HubConnectionBuilder()
    .withUrl("/chatGroup")
    .build();


connection.on("Send", function (message) {
    var li = document.createElement("li");
    li.textContent = message;
    document.getElementById("messagesList").appendChild(li);
});

connection.on("ReceiveMessage", function (user,message) {
    var li = document.createElement("li");
    li.textContent = user + " : " + message;
    document.getElementById("privateMessagesList").appendChild(li);
});



document.getElementById("is-online").addEventListener("click", async () => {
    var displayname = document.getElementById("displayname").value;
    try {
        await connection.invoke("IsOnline", displayname);
    } catch (e) {
        console.error(e.toString());
    }
    event.preventDefault();

});

connection.on("UserOnline", function (displayname) {
    var li = document.createElement("li");
    li.textContent = displayname + " is Online! ";
    document.getElementById("privateMessagesList").appendChild(li);
});



document.getElementById("send-private").addEventListener("click", async (event) => {
    var userid = document.getElementById("userid").value;
    var message = document.getElementById("msg").value;
    try {
        await connection.invoke("SendPrivateMessage", userid, message);
    }
    catch (e) {
        console.error(e.toString());
    }
    event.preventDefault();
});



// We need an async function in order to use await, but we want this code to run immediately,
// so we use an "immediately-executed async function"
(async () => {
    try {
        await connection.start();
    }
    catch (e) {
        console.error(e.toString());
    }
})();
