# Stage 1
FROM node:alpine AS build
WORKDIR /app
COPY package.json package-lock.json ./
RUN npm install
COPY . .
RUN npm run build

# Stage 2
FROM nginx:alpine
COPY --from=build /app/dist/client/browser /usr/share/nginx/html
COPY ./nginx.conf /etc/nginx/conf.d/default.conf
