### CI/CD
```sh
docker tag <image_name_> <user_name_>/thesis/<image_name_>
docker build -t <user_name_>/thesis/<image_name_>:latest -f ./src/<ServiceName>/Dockerfile .
```

```sh
docker push <user_name_>/<image_name_>:latest
```

```sh
docker-compose build 
docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d
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
dotnet ef migrations remove --output-dir Data/Migrations
```

### Format
```sh
dotnet format ./CloudNativeIdentityThesis.sln
```

### Testing
```sh
dotnet test --filter <ServiceName>.UnitTests --no-build --verbosity normal
dotnet test –collect:”XPlat Code Coverage”
```

### Kubernetes
```sh
kubectl apply -f k8s/thesis-namespace.yaml

kubectl get namespaces

kubectl get deployments -n thesis

kubectl apply -f k8s/pgadmin-deployment.yaml

kubectl apply -f k8s/catalog-database-secret.yaml
kubectl apply -f k8s/catalog-database-pvc.yaml
kubectl apply -f k8s/catalog-database-configmap.yaml
kubectl apply -f k8s/catalog-database-deployment.yaml
```


### Azure
#### Services
rg-thesis Resource Group
app-webappbff-web App Service
asp-thesis App Service plan
turalhasanovthesis AD B2c

```sh

```