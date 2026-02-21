# majstori-nbp

Projekat za predmet Napredne baze podataka

Ovaj projekat predstavlja distribuiranu web aplikaciju za povezivanje korisnika sa majstorima radi pretrage, komunikacije i ugovaranja usluga. Sistem je realizovan korišćenjem Angular frontend-a, ASP.NET backend-a, Node.js WebSocket servera, Neo4j graf baze i Redis in-memory baze.

## Arhitektura sistema

Sistem je zasnovan na višeslojnoj i događajno-orijentisanoj arhitekturi i sastoji se iz sledećih komponenti:

- Angular klijentska aplikacija  
- ASP.NET backend API  
- Node.js WebSocket server  
- Neo4j graf baza  
- Redis in-memory baza sa Pub/Sub mehanizmom  

### Komunikacija između komponenti

- Angular → ASP.NET : HTTP/REST  
- Angular ↔ WebSocket server : WebSocket  
- ASP.NET → Redis : Pub/Sub + skladištenje  
- WebSocket server ← Redis : Pub/Sub pretplata  

## Funkcionalnosti sistema

Sistem omogućava:

- registraciju i autentikaciju korisnika i majstoa   
- pretragu vodoinstalatera po kategorijam i podkategorijam
- razmenu poruka između korisnika i majstora 
- real-time notifikacije o novim porukama  

## Tehnologije

**Frontend**

- Angular  

**Backend**

- ASP.NET Web API  

**Real-time komunikacija**

- Node.js WebSocket server  
- Redis Pub/Sub  

**Baze podataka**

- Neo4j (graf podaci)  
- Redis (sesije, poruke, notifikacije)  

## Model podataka

### Neo4j

U Neo4j graf bazi čuvaju se domenski entiteti i njihove veze:

- korisnici  
- vodoinstalateri  
- kategorije  
- podkategorije  
- relacije između korisnika, vodoinstalatera i kategorija  

Graf struktura omogućava efikasno povezivanje korisnika sa relevantnim vodoinstalaterima na osnovu usluge.

### Redis

Redis se koristi kao in-memory baza i sistem za razmenu događaja.

U Redis-u se čuvaju:

- web sesioni tokeni  
- poruke  
- notifikacije  

Takođe se koristi Redis Pub/Sub za komunikaciju između ASP.NET backend-a i WebSocket servera.

## Tok poruke kroz sistem

Slanje poruke funkcioniše na sledeći način:

1. Korisnik šalje poruku iz Angular aplikacije  
2. Angular šalje HTTP zahtev ASP.NET backendu  
3. Backend čuva poruku u Redis  
4. Backend objavljuje događaj na Redis Pub/Sub kanalu  
5. Node.js WebSocket server prima događaj  
6. WebSocket server šalje poruku odgovarajućem Angular klijentu  

Ovim pristupom omogućena je real-time komunikacija bez direktne zavisnosti klijenta od backend servera za isporuku poruka.

## Prednosti arhitekture

- odvajanje real-time komunikacije od poslovne logike  
- skalabilna distribucija poruka preko Redis Pub/Sub  
- brza obrada i isporuka poruka (in-memory Redis)  
- efikasna relacijska pretraga (Neo4j graf model)  
- modularna i proširiva arhitektura  

## Zaključak

Sistem predstavlja distribuiranu web aplikaciju zasnovanu na mikroservisnim principima, sa kombinacijom graf baze (Neo4j), in-memory baze (Redis) i WebSocket real-time komunikacije. Arhitektura omogućava efikasno povezivanje korisnika i vodoinstalatera, brzu razmenu poruka i skalabilno proširenje sistema.

##Pokretanje

Prvo git clonuj repo
Zatim napraviti .env file unutar Seeding foldera i neka bude ovakav sadrzaj

```
NEO4J_URI=bolt://localhost:7687
NEO4J_USERNAME=neo4j
NEO4J_PASSWORD=tvoj_password
NEO4J_DATABASE=neo4j

```

Zatim napraviti .env file unutar majstor-nbp-server foldera i neka bude ovakav sadrzaj:

```
NEO4J_URI=bolt://neo4j:7687
NEO4J_USERNAME=neo4j
NEO4J_PASSWORD=tvoj_password
NEO4J_DATABASE=neo4j
AURA_INSTANCEID=b079bc42
AURA_INSTANCENAME=Instance01
JWT_ISSUER=milutin
JWT_AUDIENCE=user
JWT_SECRET=D5B36A9DEE6C9BB716882A3AD4DE2714734EBD6E2516C
REDIS_URI=redis
REDIS_PORT=6379
REDIS_USER=default
REDIS_PASSWORD=redis_password

```

Zatim napraviti .env file unutar WebSocketServer folder i neka bude ovakav sardzaj:

```
NEO4J_AUTH=neo4j/tvoj_password
```

### Pokretanje

```
mkdir neo4j_data #volume za neo4j data
docker-compose build #za bildovanje image, u slucaju linux sudo docker-compose build
ducker-compose up # za povretanje instanci, u slucaju linux sudo docker-compose up
```
