FROM node:alpine
WORKDIR /var/www/boxis

COPY ./nginx.conf /etc/nginx/conf.d/default.conf
COPY package.json /var/www/boxis
COPY package-lock.json /var/www/boxis

RUN npm install

COPY . /var/www/boxis

EXPOSE 80/tcp 443/tcp

CMD "npm" "start"