# 🍺 UBB-SE-2025-Beer

## Running instructions
### 1. Create the Database

Open a terminal and run the following commands:

```bash
cd './Class Library/'
dotnet ef migrations add InitialCreate --startup-project ../ServerAPI
```
### 2. Make sure your server is running

you can start it by going to ServerAPI folder and running 
dotnet run 

### 3. Start either the web or the desktop application