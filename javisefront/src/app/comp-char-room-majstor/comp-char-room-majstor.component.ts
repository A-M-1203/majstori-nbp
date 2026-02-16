import { Component, ElementRef, ViewChild } from '@angular/core';
import { SocketService } from '../service/ser-socket';
import { ServiceOcenaKontakt } from '../service/ser-ocene';
import { Ocena } from '../interface/ocena';
import { jwtDecode } from 'jwt-decode';
import { ServiceMessage } from '../service/ser-message';
import { Observable } from 'rxjs';
import { Message } from '../interface/message';
import { ActivatedRoute, Route, Router } from '@angular/router';
@Component({
  selector: 'app-comp-char-room-majstor',
  templateUrl: './comp-char-room-majstor.component.html',
  styleUrl: './comp-char-room-majstor.component.css'
})
export class CompCharRoomMajstorComponent {
goToPage(id: string|undefined) {
  if(id){
    this.router.navigate(["/glavnaStranicaMajstor/room/"+id]);
  }
}
selectedChat: boolean=false;
room:string='';
id:string='';
@ViewChild('messagesContainer') private messagesContainer!: ElementRef;
    sendMessage() {
        let message:any={sadrzaj:this.messageContent,chat:this.room,korisnik:this.id};
        this.servisMessage.sendMessage(message);
        this.messageContent = '';
    }
messageContent: any;
    selectChat(id: string) {
      if(this.selectedChat){
        this.serviceSocket.leaveRoom(this.room);
      }
      this.room=id;
      this.servisMessage.getMessages(this.room);
      this.serviceSocket.joinRoom(this.room);
      this.selectedChat=true;
      this.serviceSocket.onNewMessage((message)=>{
        this.messages.push(message);
      });
    }
    messages:Message[]=[];
    chats:Ocena[]=[];
    obs:Observable<Message[]>
    constructor(private serviceSocket:SocketService,private servisOcenaKontakt:ServiceOcenaKontakt,private servisMessage:ServiceMessage,private router:Router){
      this.obs=servisMessage.returnObs();
      this.obs.subscribe(x=>{
        this.messages=x;
      })
      servisOcenaKontakt.getChats().subscribe(x=>{
        console.log(x.chats);
        this.chats=x.chats;
      })

    }
    ngAfterViewChecked() {
      this.scrollToBottom();
    }
    private scrollToBottom(): void {
      try {
        this.messagesContainer.nativeElement.scrollTop = this.messagesContainer.nativeElement.scrollHeight;
      } catch (err) {
        console.error('Could not scroll to bottom:', err);
      }
    }
}
