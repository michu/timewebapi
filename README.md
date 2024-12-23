TimeWebApi is a simple API for registering work time by your employees.

# Download sources

`git clone https://github.com/michu/timewebapi.git`

# Installation instruction for testing

Warning: Please ensure that you don't have your own services running on 5433, 8080 and 8081 ports. If yes then you have to adjust docker-compose.yml and appsettings.json files.

`cd timewebapi`

`docker compose -f docker-compose.yml -f docker-compose.override.yml up`

Now you can test API at https://localhost:8081/swagger/index.html

# Installation instruction for production

TODO

# Run unit tests

`cd timewebapi`

`cd TimeWebApi.UnitTests`

`dotnet test`

# Run integration tests

Warning: Please ensure that you don't have your own services running on 5435 port.
If yes then you have to adjust docker-compose_test.yml and appsettings.json files.

On first console:

`cd timewebapi`

`docker compose -f docker-compose_test.yml up`

On second console:

`cd timewebapi`

`cd TimeWebApi.IntegrationTests`

`dotnet test`
