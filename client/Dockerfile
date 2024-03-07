FROM node:18 as build

WORKDIR /app

COPY package*.json ./

RUN npm i 

COPY . .

RUN npm run build

FROM nginx:alpine

COPY --from=build /app/dist/client /usr/share/nginx/html

EXPOSE 100

CMD ["nginx", "-g", "daemon off;"]