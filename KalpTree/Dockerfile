FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 5000
#EXPOSE 44313

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["KalpTree/KalpTree.csproj", "KalpTree/"]
RUN dotnet restore "KalpTree/KalpTree.csproj"
COPY . .
WORKDIR "/src/KalpTree"
RUN dotnet build "KalpTree.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "KalpTree.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
USER 1122
ENTRYPOINT ["dotnet", "KalpTree.dll"]