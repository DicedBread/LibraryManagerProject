# Stack
* React
    * Mui components 

* C# 
    * ASP.net MVC
    * EF core
    * NUnit
    * [Testcontainers](https://dotnet.testcontainers.org/) for integration testing

* Postgres
* Docker


# Setup
need following enviroment files then run 
`docker compose up`

`/.env`
```
TEST_PASSWORD= database password
HOST_CERT_PATH= path to folder containing self signed cert in hosting machine 
CONT_CERT_PATH= path to cert file on container 
CERT_PASSWORD= password used to create cert file 
```

`/library_manager_client/.env`
```
- VITE_SERVER_DOMAIN=localhost:8080
```

---

### Swagger documentation screenshot
![ui](/images/endpoints.png)
### example of ui 
![ui](/images/UI.png)
