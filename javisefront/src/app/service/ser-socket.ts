import { Injectable } from '@angular/core';
import { io, Socket } from 'socket.io-client';

@Injectable({
  providedIn: 'root'
})
export class SocketService {
  private socket: Socket;

  constructor() {
    this.socket = io('http://localhost:3100');
  }

  joinRoom(room: string) {
    this.socket.emit('joinRoom', room);
  }
  leaveRoom(room:string){
    this.socket.emit('leaveRoom',room);
  }
  sendMessage(message: any) {
    this.socket.emit('sendMessage', message);
  }

  onNewMessage(callback: (message: any) => void) {
    this.socket.off('newMessage');
    this.socket.on('newMessage', callback);
  }

  disconnect() {
    if (this.socket) {
      this.socket.disconnect();
    }
  }
}