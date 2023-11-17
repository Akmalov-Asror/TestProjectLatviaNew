# Project Name

## Description

Briefly describe your project, its purpose, and key features.

## Prerequisites


- [.NET SDK 7.0](https://dotnet.microsoft.com/download)
- [PostgreSQL](https://www.postgresql.org/download/)


## Getting Started

1. **Clone the repository:**

    ```bash
    git clone https://github.com/your-username/your-project.git
    ```

2. **Navigate to the project folder:**

    ```bash
    cd your-project
    ```
3. **Write Terminal this key words :**
    ```bash
    dotnet build
    dotnet restore
    ```  
4. **Database Setup:**

    - Create a PostgreSQL database for the project.

    - Update the `appsettings.json` file with your database connection string:

        ```json
        {
          "ConnectionStrings": {
            "DefaultConnection": "Host=localhost;Port=5432;Database=YourDatabase;Username=YourUsername;Password=YourPassword"
          },
          // other settings...
        }
        ```

5. **Run Migrations:**

    ```Package manager console
    add-migration "some words"

    update-database
    ```

6. **Run the Application:**

    ```bash
    dotnet run
    ```
    
7.**Default Admin and User: **

    Admin 
        name : Jenny@gmail.com; 
        password: A0601221a_;
    Manager 
        name : Coma@gmail.com; 
        password: B0601221b_;
    User 
        name : Vin@gmail.com;
        password: C0601221c_;

  

## Contributing

If you would like to contribute to the project, please follow the [contribution guidelines](CONTRIBUTING.md).

## License

This project is licensed under the [MIT License](LICENSE).

## Acknowledgments

Mention any libraries, tools, or people you want to give credit to.
