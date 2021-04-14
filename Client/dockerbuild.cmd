docker build ./ -t boxis-client
docker tag boxis-client ekleziast56/boxis-client:latest
docker push ekleziast56/boxis-client:latest
DEL "../docker/build" /S /Q