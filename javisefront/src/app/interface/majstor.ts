import { Lokacija } from "./lokacija";

export interface Majstor {
    _id?:string,
    ime: string,
    prezime: string,
    email: string,
    broj: string,
    lokacija: string | any,
    adresa: string,
    podkategorija: string[] | any,
    password: string,
    averageRating? :Number,
    profilePicture?: string
}