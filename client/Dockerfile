FROM node:21-alpine

WORKDIR /app

COPY package*.json ./

RUN npm i --legacy-peer-deps

COPY . .

RUN npm run build

EXPOSE 4000

USER node

CMD ["node", "dist/client/server/main.js"]