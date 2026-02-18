import { Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { SocketService } from '../service/ser-socket';
import { ActivatedRoute } from '@angular/router';
import { jwtDecode } from 'jwt-decode';
import { Message } from '../interface/message';
import { Observable } from 'rxjs';
import { ServiceMessage } from '../service/ser-message';
import { ServiceKlijent } from '../service/ser-klijent';
@Component({
  selector: 'app-comp-char-room-client',
  templateUrl: './comp-char-room-client.component.html',
  styleUrl: './comp-char-room-client.component.css'
})
export class CompCharRoomClientComponent implements OnInit,OnDestroy {

  room:string|null=null;
  id:string='';
  messages:Message[]=[];
  messageContent: string='';


  sendMessage() {
    let message:any={sadrzaj:this.messageContent,chat:this.room,korisnik:this.id};
    this.messageServis.sendMessage(message);
    this.messageContent = '';
  }

@ViewChild('messagesContainer') private messagesContainer!: ElementRef;
  constructor(private servisSocket:SocketService,private route:ActivatedRoute,private messageServis:ServiceMessage,private korisnikService:ServiceKlijent){
    korisnikService.getId().subscribe(x=>{
      this.id=x.id;
      console.log(this.id);
    })
  }
  ngOnDestroy(): void {
    //this.servisSocket.disconnect();
  }
  ngOnInit(): void {
    this.route.paramMap.subscribe(map=>{
      this.room=map.get('id');
      console.log(this.room);
      if(this.room){
        const token:any=localStorage.getItem("jwtToken");
        if(token){
          //this.id=jwtDecode<any>(token).userId;
        }
        console.log(this.room);
        this.messageServis.getMessages(this.room).subscribe(x=>{
          this.messages=x;
        })
        this.servisSocket.joinRoom(this.room);
        this.servisSocket.onNewMessage((message)=>{
          this.messages.push(message);
        })
      }
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
