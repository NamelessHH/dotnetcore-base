FROM microsoft/dotnet:2.1-sdk AS build

# Configure users, environment, and working directory
RUN useradd --user-group --create-home --shell /bin/false app
ENV HOME=/home/app
WORKDIR $HOME/src

# Copy source code
COPY . .

#build the project
RUN dotnet publish src/dotnetcore-base/dotnetcore-base.csproj -c Release -o ../../bin

FROM microsoft/dotnet:2.1-aspnetcore-runtime

# Configure users, environment, and working directory
RUN useradd --user-group --create-home --shell /bin/false app
ENV HOME=/home/app
WORKDIR $HOME/bin

# Copy compiled dll
COPY --from=build /home/app/src/bin /home/app/bin
RUN chown -R app:app $HOME/*
USER app

ENTRYPOINT ["dotnet", "dotnetcore-base.dll"]
