# AgileWorks Testtöö

## Kaustade selgitus

Kuna see ülesanne tundus natuke liiga lihtne ning polnud öeldud kuidas täpselt tahetakse mis arhitektuuriga oleks ülesanne lahendatud siis otsustasin lahendada selle ülesande 3 erinevat viisi. Kõik lahendused töötavad samamoodi väikeste mööndustega

### Agileworks-front-end

Selles kaustas on front end mis on ehitatud kasutades Next.js ning see suhtleb TaskManagingWebAppMvc kaustas oleva RESTful Apiga.

### TaskManagingWebAppMvc

Siin on kaks controllerit. Esimene controller on loodud selleks et teha server side vaadete näitamine võimalikuks. Teine controller on API controller mis loob võimaluse suhelda siis Next.js Front endil. API controllerile on tehtud ka testid millega kontrollitakse kõiki meetodeid mis controlleril on.

### TaskManagingWebAppRazorPages

Server side veebirakenuds mis on loodud kasutades Razor pagesi kõige primitiivsem ja lihtsam lahendus antud ülesandele. Selle tegemine võttis väga vähe ning seda on raske unit testidega katta.
