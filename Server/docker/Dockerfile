FROM mcr.microsoft.com/dotnet/aspnet:5.0

COPY ./build /app

WORKDIR /app

HEALTHCHECK \
    --interval=30s \
    --timeout=10s \
    --start-period=10s \
    --retries=3 \
    CMD curl -f https://boxis.io/api/v1/hc 2>&1 || exit 1

ENTRYPOINT ["dotnet", "Instabrand.dll"]