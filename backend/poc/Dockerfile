#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM microsoft/dotnet:2.1-aspnetcore-runtime-nanoserver-sac2016 AS base
WORKDIR /app
EXPOSE 54872
EXPOSE 44395

FROM microsoft/dotnet:2.1-sdk-nanoserver-sac2016 AS build
WORKDIR /src
COPY ["poc/poc.csproj", "poc/"]
RUN dotnet restore "poc/poc.csproj"
COPY . .
WORKDIR "/src/poc"
RUN dotnet build "poc.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "poc.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "poc.dll"]