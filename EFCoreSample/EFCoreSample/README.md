## 建立本地Docker SQL Server
```
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Aa123456" -p 1433:1433 --name sql1 --hostname sql1 -d mcr.microsoft.com/mssql/server:2022-latest`
```

或在專案資料夾使用
```
docker compose up -d
```
## 加入Migration指令
- `dotnet ef migrations add InitialCreate`

## 更新Migration到DataBase
- `dotnet ef database update`
