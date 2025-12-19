export interface NotificationItem {
  id: string;
  text: string;
  time: number;          // epoch ms
  isRead: boolean;
  avatarUrl?: string;    // optional profile pic
  link?: string;         // optional click target
}
