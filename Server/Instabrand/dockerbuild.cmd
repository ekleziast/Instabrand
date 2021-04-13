dotnet restore
dotnet build -c release /nowarn:CS1591
dotnet publish -c release -o ./docker/build
rmdir "./docker/build" /S /Q
docker build ./docker -t boxis
docker tag boxis ekleziast56/boxis:latest
docker push ekleziast56/boxis:latest