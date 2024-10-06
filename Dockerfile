# Используем официальный образ с базовой ОС
FROM ubuntu:22.04 AS build

# Устанавливаем необходимые зависимости
RUN apt-get update && apt-get install -y wget tar libicu70

# Скачиваем и устанавливаем .NET SDK
ENV DOTNET_FILE=dotnet-sdk-8.0.100-linux-x64.tar.gz
ENV DOTNET_ROOT=/usr/share/dotnet
RUN mkdir -p "$DOTNET_ROOT" && \
    wget -O "$DOTNET_FILE" https://dotnetcli.azureedge.net/dotnet/Sdk/8.0.100/$DOTNET_FILE && \
    tar zxf "$DOTNET_FILE" -C "$DOTNET_ROOT" && \
    rm "$DOTNET_FILE"

# Добавляем путь для использования dotnet CLI
ENV PATH="$PATH:$DOTNET_ROOT:$DOTNET_ROOT/tools"

# Устанавливаем рабочую директорию в контейнере
WORKDIR /dotnet/app/statics

# Копируем файлы проекта
COPY . ./

# Восстанавливаем зависимости и публикуем проект
RUN dotnet restore
RUN dotnet publish -c Release -o out

# Проверяем содержимое директории после публикации
RUN ls -la out

# Финальный этап
FROM ubuntu:22.04 AS final

# Устанавливаем необходимые зависимости для работы .NET Runtime
RUN apt-get update && apt-get install -y libicu70 libssl-dev libkrb5-3 zlib1g

# Копируем файлы .NET Runtime из предыдущей стадии
COPY --from=build /usr/share/dotnet /usr/share/dotnet

# Устанавливаем рабочую директорию для финальной стадии
WORKDIR /dotnet/app/statics

# Копируем опубликованный проект из стадии сборки
COPY --from=build /dotnet/app/statics/out .

# Добавляем путь для использования dotnet
ENV PATH="$PATH:/usr/share/dotnet"

# Проверяем содержимое финальной директории
RUN ls -la

# Открываем порт для доступа к приложению
EXPOSE 5000

# Запуск приложения с выводом отладочной информации
# ENTRYPOINT ["sh"]
CMD ["sh"]
# ENTRYPOINT ["./WebApplication1"]