export interface Kategorija {
    naziv: string
    podkategorije: {naziv:String,_id:String,kategorija:String}[]
    _id: string
}