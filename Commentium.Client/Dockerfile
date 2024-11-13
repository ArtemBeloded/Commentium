FROM node:18 AS build

WORKDIR /app

COPY Commentium.Client/package*.json ./
RUN npm install

COPY Commentium.Client/. ./
RUN npm run build --prod

FROM node:18 AS runtime

WORKDIR /app
COPY --from=build /app /app

EXPOSE 4200

CMD ["npm", "run", "start", "--", "--host", "0.0.0.0"]