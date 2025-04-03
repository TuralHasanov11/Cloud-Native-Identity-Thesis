### CI/CD
```sh
docker build -t <user_name_>/<image_name_>:latest -f ./src/<ServiceName>/Dockerfile .
```

```sh
docker push <user_name_>/<image_name_>:latest
```

### Application Inspector

```sh
appinspector analyze -s ./ -o appinspector.html -g **/.git/** -l ./appinspector.log
```
```sh
appinspector analyze -s ./ -o appinspector.sarif -f sarif -g **/.git/** -l ./appinspector.log
```
```sh
appinspector analyze -s ./ -o appinspector.json -f json -g **/.git/** -l ./appinspector.log
```

### Code Coverage
```sh
dotnet test --collect:"XPLat Code Coverage;Format=json"

reportgenerator -reports:".\TestResults\<Id>\coverage.json" -targetdir:"coverageresults" -reporttypes:Html

dotnet stryker
```

### Create Migration
```sh	
dotnet ef migrations add <migration_name> --project ../<TargetService> --startup-project . --output-dir Data/Migrations
```

```sh	
dotnet ef migrations add <migration_name> --output-dir Data/Migrations
```

### Format
```sh
dotnet format ./CloudNativeIdentityThesis.sln
```

### Testing
```sh
dotnet test --filter <ServiceName>.UnitTests --no-build --verbosity normal
```



