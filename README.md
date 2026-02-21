# majstori-nbp

Projekat za predmet Napredne baze podataka

Ovaj projekat predstavlja distribuiranu web aplikaciju za povezivanje korisnika sa majstorima radi pretrage, komunikacije i ugovaranja usluga. Sistem je realizovan korišćenjem Angular frontend-a, ASP.NET backend-a, Node.js WebSocket servera, Neo4j graf baze i Redis in-memory baze.

## Arhitektura sistema

Sistem je zasnovan na višeslojnoj i event-driven arhitekturi i sastoji se iz sledećih komponenti:

- Angular klijentska aplikacija
- ASP.NET backend API
- Node.js WebSocket server
- Neo4j graf baza
- Redis in-memory baza sa Pub/Sub mehanizmom

### Komunikacija između komponenti

- Angular → ASP.NET : HTTP/REST
- Angular ↔ WebSocket server : WebSocket
- ASP.NET → Redis : Pub/Sub + skladištenje
- WebSocket server ← Redis : Pub/Sub mehanizam

## Funkcionalnosti sistema

Sistem omogućava:

- registraciju i autentifikaciju korisnika i majstora
- pretragu majstora po kategorijam i podkategorijama
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

- Neo4j (korisnici, majstori, kategorije, podkategorije i njihove međusobne veze)
- Redis (sesije, poruke, notifikacije)

## Model podataka

### Neo4j

U Neo4j graf bazi čuvaju se domenski entiteti i njihove veze:

- korisnici
- majstori
- kategorije
- podkategorije
- relacije između korisnika, majstora i kategorija

Graf struktura omogućava efikasno povezivanje korisnika sa relevantnim majstorima na osnovu usluge.

### Redis

Redis se koristi kao in-memory baza i sistem za razmenu događaja.

U Redis-u se čuvaju:

- web session tokeni
- poruke
- notifikacije

Takođe se koristi Redis Pub/Sub za komunikaciju između ASP.NET backend-a i WebSocket servera.

## Tok poruke kroz sistem

Slanje poruke funkcioniše na sledeći način:

1. Korisnik šalje poruku iz Angular aplikacije
2. Angular šalje HTTP zahtev ASP.NET backend-u
3. Backend čuva poruku u Redis-u
4. Backend objavljuje događaj na Redis Pub/Sub kanalu
5. Node.js WebSocket server prima događaj
6. WebSocket server šalje poruku odgovarajućem Angular klijentu

Ovim pristupom omogućena je real-time komunikacija bez direktne zavisnosti klijenta od backend servera za isporuku poruka.

## Prednosti arhitekture

- odvajanje real-time komunikacije od poslovne logike
- skalabilna distribucija poruka preko Redis Pub/Sub-a
- brza obrada i isporuka poruka (in-memory Redis)
- efikasna pretraga po vezama (Neo4j graf model)
- modularna i proširiva arhitektura

## Zaključak

Sistem predstavlja distribuiranu web aplikaciju zasnovanu na mikroservisnim principima, sa kombinacijom graf baze (Neo4j), in-memory baze (Redis) i WebSocket real-time komunikacije. Arhitektura omogućava efikasno povezivanje korisnika i majstora, brzu razmenu poruka i skalabilno proširenje sistema.

## Pokretanje

Prvo klonirati repozitorijum:

```
git clone https://github.com/A-M-1203/majstori-nbp.git
```

Zatim napraviti .env fajl unutar Seeding foldera i neka bude ovakav sadržaj:

```
NEO4J_URI=bolt://localhost:7687
NEO4J_USERNAME=neo4j
NEO4J_PASSWORD=tvoj_password
NEO4J_DATABASE=neo4j
```

Zatim napraviti .env fajl unutar majstor-nbp-server foldera i neka bude ovakav sadržaj:

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

Zatim napraviti .env fajl unutar WebSocketServer folder i neka bude ovakav sardzaj:

```
NEO4J_AUTH=neo4j/tvoj_password
```

### Pokretanje

```
mkdir neo4j_data     # volume za neo4j data (mora da bude u root folderu projekta)
docker-compose build # za build-ovanje image-a, u slucaju linux-a sudo docker-compose build
docker-compose up    # za pokretanje instanci, u slucaju linux-a sudo docker-compose up
```

Na kraju seed-ovati podatke iz Seeding foldera:

```
cd Seeding
npm install
node seed.js
```

Otvoriti frontend preko web pretraživača na adresi:

```
localhost:4050
```
