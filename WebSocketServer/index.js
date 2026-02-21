const express=require('express');
const http=require('http');
const bodyParser=require('body-parser');
const cors=require('cors');
const socketIo = require('socket.io');
const redis = require('redis');




//App settings
const app=express();
const corsOptions = {
    origin: ['http://localhost:4050', 'http://localhost:4200'],
    methods: 'GET,HEAD,PUT,PATCH,POST,DELETE',
    allowedHeaders: 'Content-Type,Authorization'
  };
app.use(cors(corsOptions));
app.use(bodyParser.json());
const server=http.createServer(app);
const io = socketIo(server, {
    cors: {
      origin: ['http://localhost:4050', 'http://localhost:4200'], 
      methods: ["GET", "POST"]
    }
  });

  const client = redis.createClient({
    url: 'redis://redis:6379' 
  });
  client.connect();
  (async ()=>{
    const subClient=client.duplicate();
    await subClient.connect();
    await subClient.subscribe("chat",(message)=>{
        console.log(message);
        messageJson=JSON.parse(message);
        io.to(messageJson.chat).emit("newMessage",messageJson);
      });
    await subClient.subscribe("notification",(notification)=>{
      notificationJson=JSON.parse(notification)
      console.log(notificationJson);
      io.to(notificationJson.korisnik).emit("newNotification",notificationJson);
    })
  })();

io.on('connection',(socket)=>{
    console.log("New client je povezan");
    socket.on('joinRoom', (room) => {
        socket.join(room);
        console.log(`Client joined room: ${room}`);
      });

    socket.on("joinNotificationRoom",async (room)=>{
      
      const value = await client.get("session:"+room);
      const jsonObject=JSON.parse(value);
      console.log(jsonObject);
      if(jsonObject && jsonObject.userId){
        socket.join(jsonObject.userId);
      }
    });

    socket.on("leaveNotificationRoom",async (room)=>{
      const value = await client.get("session:"+room);
      const jsonObject=JSON.parse(value);
      console.log(`Client joined room: ${jsonObject.userId}`);
      socket.leave(jsonObject.userId);
    })

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