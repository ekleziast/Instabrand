# Boxis.io - сервис для создания сайта на основе вашего Instagram профиля.

* [Server](/Server) - Backend на .NET 5.0
* [Client](/Client) - Frontend на React ([Maxim Serebryakov](https://github.com/StarkMP))
* [Docker](/Docker) - Docker конфигурация

## Run:

### Generate DH param for SSL
```
cd ~/home/remote/dhparam/
openssl dhparam -out ./dhparam.pem 4096
cd ..
```

### Pull and start Docker containers
```
cd Docker
docker-compose pull
docker-compose up -d
```
