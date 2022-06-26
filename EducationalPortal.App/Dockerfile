FROM node:alpine AS build
WORKDIR /app
RUN npm install -g @angular/cli
COPY package.json .
RUN npm install
COPY . .
RUN npm run build

FROM nginx:alpine
COPY --from=build /app/dist/educational-portal.app /usr/share/nginx/html
EXPOSE 80