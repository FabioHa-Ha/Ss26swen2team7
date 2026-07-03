# Setup Database Docker

In Folder of Dockerfile:
(if previous build exists) docker rm databaseContainer
docker build -t database .
docker run -d --name databaseContainer -p 5432:5432 -it database

# Windows - Stop local postgres server

Win + R => services.msc
Find postgres and stop service

# Setup Database in Visual Studio

Extras -> NuGet-Package-Manager -> Paket-Manager-Console
In Console: Update-Database



