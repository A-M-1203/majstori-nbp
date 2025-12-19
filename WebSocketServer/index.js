require('dotenv').config();
const express=require('express');
const http=require('http');
const bodyParser=require('body-parser');
const cors=require('cors');
const socketIo = require('socket.io');

//App settings
const app=express();
const corsOptions = {
    origin: 'http://localhost:4050',
    methods: 'GET,HEAD,PUT,PATCH,POST,DELETE',
    allowedHeaders: 'Content-Type,Authorization'
  };
app.use(cors(corsOptions));
app.use(bodyParser.json());
const server=http.createServer(app);
const io = socketIo(server, {
    cors: {
      origin: "http://localhost:4050", 
      methods: ["GET", "POST"]
    }
  });
io.on('connection',(socket)=>{
    console.log("New client je povezan");
    socket.on('joinRoom', (room) => {
        socket.join(room);
        console.log(`Client joined room: ${room}`);
      });
    socket.on('leaveRoom',(room)=>{
        socket.leave(room);
        console.log('Client left room '+room);
    });
    socket.on('disconnect', () => {
        console.log('Client disconnected');
      });
});
console.log("Server started1");
server.listen(4500);