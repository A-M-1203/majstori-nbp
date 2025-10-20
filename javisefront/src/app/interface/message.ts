import { Klijent } from "./klijent";
import { Majstor } from "./majstor";

export interface Message{
    _id:string,
    sadrzaj:string,
    korisnik:string,
    datum:Date,
    chat:string
}