devserver:
	@DOTNET_ENVIRONMENT=Development dotnet watch run --project src/

devdb:
	@docker run --name postgres --detach \
		--publish 127.0.0.1:5432:5432 \
		--env POSTGRES_USER=hellonet \
		--env POSTGRES_PASSWORD=admin \
		postgres:17.1-alpine3.20

ef/update:
	@cd src && \
		DOTNET_ENVIRONMENT=Development \
		dotnet ef database update
