var neo4j=require('neo4j-driver');
require('dotenv').config();
(async ()=>{
    const uri=process.env.NEO4J_URI;
    const user=process.env.NEO4J_USERNAME;
    const password=process.env.NEO4J_PASSWORD;
    let driver=neo4j.driver(uri,neo4j.auth.basic(user,password));
    const serverInfo=await driver.getServerInfo();
    console.log("Connection established");
    console.log(serverInfo);
    const kategorije= [
        {
            "_id": "66530171700bb566ec6fa92a",
            "naziv": "Popravke i održavanje",
            "__v": 0,
            "podkategorije": [
                {
                    "_id": "66530171700bb566ec6fa92e",
                    "naziv": "Popravke nameštaja",
                    "kategorija": "66530171700bb566ec6fa92a",
                    "__v": 0
                },
                {
                    "_id": "66530171700bb566ec6fa92c",
                    "naziv": "Popravke vodovodnih instalacija",
                    "kategorija": "66530171700bb566ec6fa92a",
                    "__v": 0
                },
                {
                    "_id": "66530171700bb566ec6fa92b",
                    "naziv": "Popravke električnih uređaja",
                    "kategorija": "66530171700bb566ec6fa92a",
                    "__v": 0
                },
                {
                    "_id": "66530171700bb566ec6fa92f",
                    "naziv": "Održavanje filtera",
                    "kategorija": "66530171700bb566ec6fa92a",
                    "__v": 0
                },
                {
                    "_id": "66530171700bb566ec6fa92d",
                    "naziv": "Popravke kućnih aparata",
                    "kategorija": "66530171700bb566ec6fa92a",
                    "__v": 0
                }
            ]
        },
        {
            "_id": "66530171700bb566ec6fa930",
            "naziv": "Uređenje enterijera",
            "__v": 0,
            "podkategorije": [
                {
                    "_id": "66530171700bb566ec6fa932",
                    "naziv": "Nameštaj po meri",
                    "kategorija": "66530171700bb566ec6fa930",
                    "__v": 0
                },
                {
                    "_id": "66530171700bb566ec6fa933",
                    "naziv": "Postavljanje podova",
                    "kategorija": "66530171700bb566ec6fa930",
                    "__v": 0
                },
                {
                    "_id": "66530171700bb566ec6fa934",
                    "naziv": "Ugradnja kuhinja i kupatila",
                    "kategorija": "66530171700bb566ec6fa930",
                    "__v": 0
                },
                {
                    "_id": "66530171700bb566ec6fa935",
                    "naziv": "Osvetljenje i rasveta",
                    "kategorija": "66530171700bb566ec6fa930",
                    "__v": 0
                },
                {
                    "_id": "66530171700bb566ec6fa931",
                    "naziv": "Dekoracija",
                    "kategorija": "66530171700bb566ec6fa930",
                    "__v": 0
                }
            ]
        },
        {
            "_id": "6665cd86532bb2487350408f",
            "naziv": "Stolarske usluge",
            "podkategorije": [
                {
                    "_id": "6665cf42532bb24873504093",
                    "naziv": " Izrada drvenih ograda i terasa",
                    "kategorija": "6665cd86532bb2487350408f"
                },
                {
                    "_id": "6665d004532bb24873504095",
                    "naziv": "Pravljenje drvenih igračaka i ukrasnih predmeta",
                    "kategorija": "6665cd86532bb2487350408f"
                },
                {
                    "_id": "6665d03a532bb24873504096",
                    "naziv": "Popravke drvenih konstrukcija i delova nameštaja",
                    "kategorija": "6665cd86532bb2487350408f"
                },
                {
                    "_id": "6665ceb1532bb24873504092",
                    "naziv": "Restauracija starih komada nameštaja",
                    "kategorija": "6665cd86532bb2487350408f"
                },
                {
                    "_id": "6665ce1a532bb24873504091",
                    "naziv": " Izrada i montaža nameštaja",
                    "kategorija": "6665cd86532bb2487350408f"
                }
            ]
        },
        {
            "_id": "66530171700bb566ec6fa923",
            "naziv": "Građevinski radovi",
            "__v": 0,
            "podkategorije": [
                {
                    "_id": "66530171700bb566ec6fa924",
                    "naziv": "Zidarski radovi",
                    "kategorija": "66530171700bb566ec6fa923",
                    "__v": 0
                },
                {
                    "_id": "66530171700bb566ec6fa928",
                    "naziv": "Električarski radovi",
                    "kategorija": "66530171700bb566ec6fa923",
                    "__v": 0
                },
                {
                    "_id": "66530171700bb566ec6fa925",
                    "naziv": "Molerski radovi",
                    "kategorija": "66530171700bb566ec6fa923",
                    "__v": 0
                },
                {
                    "_id": "66530171700bb566ec6fa926",
                    "naziv": "Keramičarski radovi",
                    "kategorija": "66530171700bb566ec6fa923",
                    "__v": 0
                },
                {
                    "_id": "66530171700bb566ec6fa927",
                    "naziv": "Tesarski radovi",
                    "kategorija": "66530171700bb566ec6fa923",
                    "__v": 0
                },
                {
                    "_id": "66530171700bb566ec6fa929",
                    "naziv": "Vodoinstalaterski radovi",
                    "kategorija": "66530171700bb566ec6fa923",
                    "__v": 0
                }
            ]
        },
        {
            "_id": "66530171700bb566ec6fa93b",
            "naziv": "Remont i renoviranje",
            "__v": 0,
            "podkategorije": [
                {
                    "_id": "66530171700bb566ec6fa93d",
                    "naziv": "Renoviranje kuhinje",
                    "kategorija": "66530171700bb566ec6fa93b",
                    "__v": 0
                },
                {
                    "_id": "66530171700bb566ec6fa93e",
                    "naziv": "Adaptacija prostorija",
                    "kategorija": "66530171700bb566ec6fa93b",
                    "__v": 0
                },
                {
                    "_id": "66530171700bb566ec6fa93c",
                    "naziv": "Renoviranje kupatila",
                    "kategorija": "66530171700bb566ec6fa93b",
                    "__v": 0
                },
                {
                    "_id": "66530171700bb566ec6fa93f",
                    "naziv": "Zamena stolarije",
                    "kategorija": "66530171700bb566ec6fa93b",
                    "__v": 0
                },
                {
                    "_id": "66530171700bb566ec6fa940",
                    "naziv": "Fasaderski radovi",
                    "kategorija": "66530171700bb566ec6fa93b",
                    "__v": 0
                }
            ]
        },
        {
            "_id": "66530171700bb566ec6fa936",
            "naziv": "Spoljni radovi",
            "__v": 0,
            "podkategorije": [
                {
                    "_id": "66530171700bb566ec6fa938",
                    "naziv": "Postavljanje ograde",
                    "kategorija": "66530171700bb566ec6fa936",
                    "__v": 0
                },
                {
                    "_id": "66530171700bb566ec6fa939",
                    "naziv": "Izrada i održavanje bašti",
                    "kategorija": "66530171700bb566ec6fa936",
                    "__v": 0
                },
                {
                    "_id": "66530171700bb566ec6fa937",
                    "naziv": "Održavanje dvorišta",
                    "kategorija": "66530171700bb566ec6fa936",
                    "__v": 0
                },
                {
                    "_id": "66530171700bb566ec6fa93a",
                    "naziv": "Postavljanje terasa i staza",
                    "kategorija": "66530171700bb566ec6fa936",
                    "__v": 0
                }
            ]
        },
        {
            "_id": "6665cdac532bb24873504090",
            "naziv": "Auto-servis i popravke",
            "podkategorije": [
                {
                    "_id": "6665d19b532bb2487350409b",
                    "naziv": "Servisiranje klima uređaja u vozilu",
                    "kategorija": "6665cdac532bb24873504090"
                },
                {
                    "_id": "6665d097532bb24873504097",
                    "naziv": "Redovno održavanje vozila",
                    "kategorija": "6665cdac532bb24873504090"
                },
                {
                    "_id": "6665d0e2532bb24873504098",
                    "naziv": "Dijagnostika i popravka motora",
                    "kategorija": "6665cdac532bb24873504090"
                },
                {
                    "_id": "6665d135532bb24873504099",
                    "naziv": "Popravka sistema za kočenje i suspenziju",
                    "kategorija": "6665cdac532bb24873504090"
                },
                {
                    "_id": "6665d16b532bb2487350409a",
                    "naziv": "Električarski radovi na vozilu",
                    "kategorija": "6665cdac532bb24873504090"
                }
            ]
        }
    ];
    /*kategorije.forEach( async (element) => {
        let { records,summary}= await driver.executeQuery(`CREATE (k: KATEGORIJA{_id:$_id,naziv:$naziv}) RETURN k`,{_id:element._id,naziv:element.naziv});
        console.log(summary);
       element.podkategorije.forEach(async (e)=>{
            let {records,summary}= await driver.executeQuery(`CREATE (p: PODKATEGORIJA { _id: $_id ,naziv:$naziv}) MATCH(k : KATEGORIJA{_id:$kid}) CREATE (p)-[:BELONGS]->(k)`,{_id:e._id,naziv:e.naziv});
            console.log(summary);
       });
    });*/
    for (const cat of kategorije) {
        await driver.executeQuery(
          `MERGE (k:Kategorija {_id: $id})
           SET k.naziv = $naziv`,
          { id: cat._id, naziv: cat.naziv }
        );
  
        if (Array.isArray(cat.podkategorije)) {
          for (const sub of cat.podkategorije) {
            await driver.executeQuery(
              `MERGE (p:Podkategorija {_id: $pid})
               SET p.naziv = $pnaziv
               WITH p
               MATCH (k:Kategorija {_id: $kid})
               MERGE (p)-[:BELONGS]->(k)`,
              { pid: sub._id, pnaziv: sub.naziv, kid: cat._id }
            );
          }
        }
    }
    await driver.close();
})();