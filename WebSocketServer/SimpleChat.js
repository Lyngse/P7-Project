const WebSocket = require('ws');
const wss = new WebSocket.Server({ port: 5000 });
console.log('Server started');

// Broadcast a message to all clients
wss.broadcast = function broadcast(data) {
  wss.clients.forEach(function each(client) {
   // if (client.readyState === WebSocket.OPEN) {
      client.send(data);
   // }
  });
};

// This function adds the remote address to each connection for later use
wss.on('connection', function connection(ws, req) {
  console.log(req.connection.remoteAddress  + " joined the chat\n");
  wss.broadcast(req.connection.remoteAddress);
  wss.clients.forEach(function each(client) {
    if (client === ws) {
      client.remoteAddress = req.connection.remoteAddress;
    }
  });
});

// This function forwards messages to a specified client
// Needs error handling
wss.on('connection', function connection(ws) {
  ws.on('message', function incoming(data) {

    console.log("Debug1: " + data);
    var message = JSON.parse(data);
    console.log("Debug2: " + message.type + ": " + message.message);
    
    if(message.type === 'forward'){
      console.log("Debug3: " );
      
      wss.clients.forEach(function each(client) {
        console.log("Debug4: " + client.remoteAddress);
        
        if(message.reciever == client.remoteAddress){
          client.send(data);
          console.log("Debug5: Send to" + client.remoteAddress);
        }
      
      });
    }

  });
});

/* This function broadcasts all messages to the other clients
wss.on('connection', function connection(ws) {
  ws.on('message', function incoming(data) {
    // Broadcast to everyone else.
    console.log('received: %s', data);
    wss.clients.forEach(function each(client) {
      if (client !== ws && client.readyState === WebSocket.OPEN) {
        client.send(data);
      }
    });
  });
});
*/

//// Load the TCP Library
//net = require('net');
//
//// Keep track of the chat clients
//var clients = [];
//
//// Start a TCP Server
//net.createServer(function (socket) {
//
//  // Identify this client
//  socket.name = socket.remoteAddress + ":" + socket.remotePort 
//
//  // Put this new client in the list
//  clients.push(socket);
//
//  // Send a nice welcome message and announce
//  socket.write("Welcome " + socket.name + "\n");
//  broadcast(socket.name + " joined the chat\n", socket);
//
//  // Handle incoming messages from clients.
//  socket.on('data', function (data) {
//    broadcast(socket.name + "> " + data, socket);
//  });
//
//  // Remove the client from the list when it leaves
//  socket.on('end', function () {
//    clients.splice(clients.indexOf(socket), 1);
//    broadcast(socket.name + " left the chat.\n");
//  });
//  
//  // Send a message to all clients
//  function broadcast(message, sender) {
//    clients.forEach(function (client) {
//      // Don't want to send it to sender
//      if (client === sender) return;
//      client.write(message);
//    });
//    // Log it to the server output too
//    process.stdout.write(message)
//  }
//
//}).listen(5000);
//
//// Put a friendly message on the terminal of the server.
//console.log("Chat server running at port 5000\n");