FROM microsoft/dotnet:2.1-sdk

# Configure users, environment, and working directory
RUN useradd --user-group --create-home --shell /bin/false app
ENV HOME=/home/app
WORKDIR $HOME/src

# Copy source code
COPY . .

CMD [ "dotnet","test","test/dotnetcore-base.Test/dotnetcore-base.Test.csproj" ]