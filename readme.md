dotnetcore-base

a simple project for starting a dotnet docker service.

## Includes

1. healthcheck endpoint at /Health
2. an example of dependency injection using Castle Windsor
    1. Uses reflection to make it easier but does suffer a performance hit
    2. suggest implementation of LAzy if performance hit on startup is too high
    3. integration test of connecting to redis, rabbit, and mssql server


## Installing

```shell
# clone the repo and install dependencies
$ git clone git@github.com:NamelessHH/dotnetcore-base.git
```

## Testing

This project uses docker and docker-compose for unit and integration testing
```shell
# view all commands
$ docker-compose run dotnetcore-base
```