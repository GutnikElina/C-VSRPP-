using Microsoft.Data.SqlClient;
using System;
using System.Threading.Tasks;

internal static class Program
{
    static string connectionString = "Server=(localdb)\\mssqllocaldb;Database=dormitory; Trusted_Connection=True;";

    static async Task Main(string[] args)
    {
        await CreateDatabase();
        await CreateTable();

        while (true)
        {
            Console.WriteLine("\n+-------------------------+");
            Console.WriteLine("|          МЕНЮ           |");
            Console.WriteLine("1) Показать всех студентов");
            Console.WriteLine("2) Добавить студента");
            Console.WriteLine("3) Обновить студента");
            Console.WriteLine("4) Удалить студента");
            Console.WriteLine("5) Фильтрация студентов");
            Console.WriteLine("6) Сортировка студентов");
            Console.WriteLine("0) Выход");
            Console.Write("Выберите опцию: ");

            string? option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    ShowAllUsers();
                    break;
                case "2":
                    AddUser();
                    break;
                case "3":
                    UpdateUser();
                    break;
                case "4":
                    DeleteUser();
                    break;
                case "5":
                    FilterUsers();
                    break;
                case "6":
                    SortUsers();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("-!!!- Неверная опция. Попробуйте снова -!!!-");
                    break;
            }
        }
    }

    static async Task CreateDatabase()
    {
        string masterConnectionString = "Server=(localdb)\\mssqllocaldb;Database=master;Trusted_Connection=True;";
        using (SqlConnection connection = new SqlConnection(masterConnectionString))
        {
            await connection.OpenAsync();
            SqlCommand command = new SqlCommand("IF DB_ID('dormitory') IS NULL CREATE DATABASE dormitory", connection);
            await command.ExecuteNonQueryAsync();
        }
    }

    static async Task CreateTable()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            await connection.OpenAsync();
            SqlCommand command = new SqlCommand(@"
                IF OBJECT_ID('Users', 'U') IS NULL
                CREATE TABLE Users (
                    Id INT PRIMARY KEY IDENTITY,
                    Name NVARCHAR(100) NOT NULL,
                    Age INT NOT NULL,
                    Room NVARCHAR(10) NOT NULL
                )", connection);
            await command.ExecuteNonQueryAsync();
            Console.WriteLine("Таблица Users создана");
        }
    }

    static void ShowAllUsers()
    {
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string sql = "SELECT * FROM Users";
            SqlCommand command = new SqlCommand(sql, connection);
            SqlDataReader reader = command.ExecuteReader();

            Console.WriteLine("\n---------------------------");
            Console.WriteLine("Список студентов:");
            while (reader.Read())
            {
                Console.WriteLine($"{reader["Id"]}). {reader["Name"]}, {reader["Age"]} лет, {reader["Room"]} комната");
            }
        }
    }

    static void AddUser()
    {
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();

            Console.Write("\nВведите имя студента: ");
            string? name = Console.ReadLine();
            Console.Write("Введите возраст: ");
            if (!int.TryParse(Console.ReadLine(), out int age))
            {
                Console.WriteLine("Некорректный ввод возраста.");
                return;
            }
            Console.Write("Введите номер комнаты: ");
            string? room = Console.ReadLine();

            string sql = "INSERT INTO Users (Name, Age, Room) VALUES (@name, @age, @room)";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@age", age);
            command.Parameters.AddWithValue("@room", room);
            command.ExecuteNonQuery();

            Console.WriteLine("-> Студент добавлен");
        }
    }

    static void UpdateUser()
    {
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();

            Console.Write("\nВведите ID студента для обновления: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Некорректный ввод ID.");
                return;
            }
            Console.Write("Введите новое имя: ");
            string? name = Console.ReadLine();
            Console.Write("Введите новый возраст: ");
            if (!int.TryParse(Console.ReadLine(), out int age))
            {
                Console.WriteLine("Некорректный ввод возраста.");
                return;
            }
            Console.Write("Введите новый номер комнаты: ");
            string? room = Console.ReadLine();

            string sql = "UPDATE Users SET Name = @name, Age = @age, Room = @room WHERE Id = @id";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@age", age);
            command.Parameters.AddWithValue("@room", room);
            command.ExecuteNonQuery();

            Console.WriteLine("-> Данные обновлены");
        }
    }

    static void DeleteUser()
    {
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();

            Console.Write("\nВведите ID студента для удаления: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Некорректный ввод ID.");
                return;
            }

            string sql = "DELETE FROM Users WHERE Id = @id";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();

            Console.WriteLine("-> Студент удален");
        }
    }

    static void FilterUsers()
    {
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();

            Console.Write("\nВведите минимальный возраст для фильтрации: ");
            if (!int.TryParse(Console.ReadLine(), out int age))
            {
                Console.WriteLine("Некорректный ввод возраста.");
                return;
            }

            string sql = "SELECT * FROM Users WHERE Age >= @age";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@age", age);
            SqlDataReader reader = command.ExecuteReader();

            Console.WriteLine("Результаты фильтрации:");
            while (reader.Read())
            {
                Console.WriteLine($"{reader["Id"]}. {reader["Name"]} - Возраст: {reader["Age"]}, Комната: {reader["Room"]}");
            }
        }
    }

    static void SortUsers()
    {
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();

            Console.WriteLine("\nСортировка по:");
            Console.WriteLine("1. Имя");
            Console.WriteLine("2. Возраст");
            Console.Write("Выберите опцию: ");
            string? option = Console.ReadLine();

            string sql = option switch
            {
                "1" => "SELECT * FROM Users ORDER BY Name",
                "2" => "SELECT * FROM Users ORDER BY Age",
                _ => "SELECT * FROM Users"
            };

            SqlCommand command = new SqlCommand(sql, connection);
            SqlDataReader reader = command.ExecuteReader();

            Console.WriteLine("\nРезультаты сортировки:");
            while (reader.Read())
            {
                Console.WriteLine($"{reader["Id"]}. {reader["Name"]} - Возраст: {reader["Age"]}, Комната: {reader["Room"]}");
            }
        }
    }
}
