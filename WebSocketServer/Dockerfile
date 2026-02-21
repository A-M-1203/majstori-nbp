FROM node:20-alpine
WORKDIR /app
COPY package.json /app
RUN npm install
RUN npm install -g nodemon
COPY . /app
CMD ["nodemon","index.js"]