# Этап сборки
FROM node:18 AS build

# Устанавливаем рабочую директорию
WORKDIR /app

# Копируем package.json и устанавливаем зависимости
COPY Commentium.Client/package*.json ./
RUN npm install

# Копируем остальные файлы проекта и собираем его
COPY Commentium.Client/. ./
RUN npm run build --prod

# Этап запуска
FROM node:18 AS runtime

# Копируем установленные зависимости и сборку из предыдущего этапа
WORKDIR /app
COPY --from=build /app /app

# Открываем порт 4200 для Angular-приложения
EXPOSE 4200

# Запускаем приложение
CMD ["npm", "run", "start", "--", "--host", "0.0.0.0"]