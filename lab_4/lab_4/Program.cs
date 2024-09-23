using System;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
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

    static void ShowAllUsers()
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            var users = db.Users.ToList();
            Console.WriteLine("\n---------------------------");
            Console.WriteLine("Список студентов:");
            foreach (var user in users)
            {
                Console.WriteLine($"{user.Id}. {user.Name} - Возраст: {user.Age}, Комната: {user.Room}");
            }
        }
    }

    static void AddUser()
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            Console.Write("\nВведите имя студента: ");
            string? name = Console.ReadLine();
            Console.Write("Введите возраст: ");
            int age = int.Parse(Console.ReadLine() ?? "0");
            Console.Write("Введите номер комнаты: ");
            string? room = Console.ReadLine();

            var user = new User { Name = name, Age = age, Room = room };
            db.Users.Add(user);
            db.SaveChanges();
            Console.WriteLine("-> Студент добавлен");
        }
    }

    static void UpdateUser()
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            Console.Write("\nВведите ID студента для обновления: ");
            int id = int.Parse(Console.ReadLine() ?? "0");
            var user = db.Users.Find(id);

            if (user != null)
            {
                Console.Write("\nВведите новое имя: ");
                user.Name = Console.ReadLine();
                Console.Write("Введите новый возраст: ");
                user.Age = int.Parse(Console.ReadLine() ?? "0");
                Console.Write("Введите новый номер комнаты: ");
                user.Room = Console.ReadLine();

                db.SaveChanges();
                Console.WriteLine("-> Данные обновлены");
            }
            else
            {
                Console.WriteLine("-> Студент с таким ID не найден");
            }
        }
    }

    static void DeleteUser()
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            Console.Write("\nВведите ID студента для удаления: ");
            int id = int.Parse(Console.ReadLine() ?? "0");
            var user = db.Users.Find(id);

            if (user != null)
            {
                db.Users.Remove(user);
                db.SaveChanges();
                Console.WriteLine("-> Студент удален");
            }
            else
            {
                Console.WriteLine("-> Студент с таким ID не найден");
            }
        }
    }

    static void FilterUsers()
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            Console.Write("\nВведите минимальный возраст для фильтрации: ");
            int age = int.Parse(Console.ReadLine() ?? "0");

            var users = db.Users.Where(u => u.Age >= age).ToList();
            Console.WriteLine("Результаты фильтрации:");
            foreach (var user in users)
            {
                Console.WriteLine($"{user.Id}. {user.Name} - Возраст: {user.Age}, Комната: {user.Room}");
            }
        }
    }

    static void SortUsers()
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            Console.WriteLine("\nСортировка по:");
            Console.WriteLine("1. Имя");
            Console.WriteLine("2. Возраст");
            Console.Write("Выберите опцию: ");
            string? option = Console.ReadLine();

            var users = option switch
            {
                "1" => db.Users.OrderBy(u => u.Name).ToList(),
                "2" => db.Users.OrderBy(u => u.Age).ToList(),
                _ => db.Users.ToList()
            };

            Console.WriteLine("\nРезультаты сортировки:");
            foreach (var user in users)
            {
                Console.WriteLine($"{user.Id}. {user.Name} - Возраст: {user.Age}, Комната: {user.Room}");
            }
        }
    }
}
