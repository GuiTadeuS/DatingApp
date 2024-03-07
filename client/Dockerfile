FROM node:21-alpine as builder

WORKDIR /app

COPY package*.json  ./

RUN npm i --legacy-peer-deps

COPY . .

RUN npm run build

FROM node:21-alpine

COPY --from=builder /app /app

WORKDIR /app

EXPOSE 4000

USER node

CMD ["node", "dist/client/server/main.js"]