DEL "./docker/build" /S /F /Q
xcopy build\*.* "./docker/build/" /E
docker build ./docker -t boxis-client
docker tag boxis-client ekleziast56/boxis-client:latest
docker push ekleziast56/boxis-client:latest
DEL "./docker/build" /S /F /Q