import { Klijent } from "./klijent";
import { Majstor } from "./majstor";
import { Message } from "./message";

export interface Ocena{
    _id:string,
    korisnik? :Klijent,
    majstor?:Majstor,
    ocena:Number,
    lastMessage?:Message
}