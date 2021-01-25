## Tried this out in Ubuntu

### Azure Cli
sudo apt install curl
curl -sL https://aka.ms/InstallAzureCLIDeb | sudo bash
sudo apt-get update
az --version

### Git
sudo apt install git
git --version

### SQL Server
[Quickstart: Install SQL Server and create a database on Ubuntu](https://docs.microsoft.com/en-us/sql/linux/quickstart-install-connect-ubuntu?view=sql-server-ver15)

1. Import the public repository GPG keys:

    * wget -qO- https://packages.microsoft.com/keys/microsoft.asc | sudo apt-key add -

2. Register the Microsoft SQL Server Ubuntu repository for SQL Server 2019

    * wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb

3. Run the following commands to install SQL Server:

    * sudo apt-get update
    * sudo apt-get install -y mssql-server

4. After the package installation finishes, run mssql-conf setup and follow the prompts to set the SA password and choose your edition.

    * sudo /opt/mssql/bin/mssql-conf setup
    * sudo dpkg -i packages-microsoft-prod.deb

5. Once the configuration is done, verify that the service is running:
    * systemctl status mssql-server --no-pager

### Azure Data Studio for Linux

[Azure Data Studio for Linux](https://docs.microsoft.com/en-us/sql/azure-data-studio/download-azure-data-studio?view=sql-server-2017#get-azure-data-studio-for-linux
)


##### Ubuntu dependencies
* sudo apt-get install libxss1
* sudo apt-get install libgconf-2-4
* sudo apt-get install libunwind8

##### Install
* sudo dpkg -i ./Downloads/azuredatastudio-linux-1.25.1.deb
* azuredatastudio




### Dot Net Core
sudo apt-get update;   sudo apt-get install -y 
apt-transport-https &&   sudo apt-get update &&   sudo apt-get install -y dotnet-sdk-3.1
sudo apt-get update;   sudo apt-get install -y apt-transport-https &&   sudo apt-get update &&   sudo apt-get install -y dotnet-sdk-5.0

dotnet --list-sdks
dotnet --version

sudo apt-get update
sudo apt-get upgrade

## Snap Store Installs
If the Ubuntu Software Snap-in is partially populated or shows erros, look here: https://status.snapcraft.io/

### Visual Studio Code
https://snapcraft.io/code (sudo snap install code --classic)

### Postman
https://snapcraft.io/postman (sudo snap install postman)

### PowerShell
https://snapcraft.io/powershell (sudo snap install powershell --classic)

## Coding side
The code makes syncronous calls which is fine for UI based APIs I reckon. For microservioces, all data access should be async I think based on M$ guidance: https://docs.microsoft.com/en-us/aspnet/core/performance/performance-best-practices?view=aspnetcore-3.1

* dotnet add package Microsoft.EntityFrameworkCore
* dotnet add package Microsoft.EntityFrameworkCore.Design
* dotnet add package Microsoft.EntityFrameworkCore.SqlServer
* dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection
* dotnet add package Microsoft.AspNetCore.JsonPatch
* dotnet add package Microsoft.AspNetCore.Mvc.NewtonsoftJson


* dotnet tool install --global dotnet-ef

* dotnet ef migrations add InitialMigration
* dotnet ef database update
* dotnet ef migrations remove


        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddDbContext<CommanderContext>(opt => opt.UseSqlServer
                (Configuration.GetConnectionString("CommanderConnection")));
            
            services.AddControllers().AddNewtonsoftJson(s => s.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver());

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<ICommanderRepo, SqlCommanderRepo>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Commander", Version = "v1" });
            });
        }
