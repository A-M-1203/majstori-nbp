export interface NotificationItem {
  id: string;
  text: string;
  time: number;          // epoch ms
  avatarUrl?: string;    // optional profile pic
  korisnik:string        // optional click target
}
