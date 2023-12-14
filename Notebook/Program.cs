namespace Notebook
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string file_name = "Notebook.txt";
            Console.Clear();
            Console.WriteLine("Запускаем простую записную книжку . .");
            if (System.IO.File.Exists(Convert.ToString(Path.GetFullPath(file_name))) == false)
                Console.WriteLine("Не найден файл Notebook.txt. . он будет создан автоматически при добавлении первой записи . .");
            Console.WriteLine("Для продолжения нажмите любую клавишу");
            Console.ReadKey();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Выберите действие");
                Console.WriteLine(new string('-', 50));
                Console.WriteLine(" '1' Добавить запись в записную книжку");
                Console.WriteLine(" '2' Изменить контакт в записной книжке");
                Console.WriteLine(" '3' Удалить контакт");
                Console.WriteLine(" '4' Вывести все контакты в алфавитном порядке");
                Console.WriteLine(" '5' Найти контакт в записной книжке");
                Console.WriteLine(new string('-', 50));

                ConsoleKey key = Console.ReadKey().Key;
                switch (key)
                {
                    case ConsoleKey.NumPad1:

                        var person = new Employee();
                        Console.Write("\nВведите имя: ");
                        person.FirstName = Console.ReadLine().Trim();

                        Console.Write("Введите фамилимю: ");
                        person.LastName = Console.ReadLine().Trim();

                        Console.Write("Введите номер телефона: ");
                        string inputNuber = Console.ReadLine();
                        person.GetPhoneNumber(inputNuber);

                        Console.Write("Введите адрес: ");
                        person.Adress = Console.ReadLine();

                        Console.Write("Заметки: ");
                        string notePersom = Console.ReadLine();
                        if (notePersom.Length > 100) { person.Note = notePersom.Remove(100); }
                        else { person.Note = notePersom; }

                        person.WriteEmployee(); //Записать контакт в файл

                        Console.WriteLine("Запись добавлена. Нажмите любую клавишу");
                        Console.ReadKey();
                        break;
                    case ConsoleKey.NumPad2:
                        var editPerson = new Employee();
                        editPerson.SearchEditCurrentPerson();
                        break;
                    case ConsoleKey.NumPad3:
                        var deletePerson = new Employee();
                        deletePerson.DeletePerson();
                        //удалить запись
                        break;


                    case ConsoleKey.NumPad4:
                        //Вывести все контакты в алфавитном порядке
                        var display = new Employee();
                        display.PrintArray();
                        break;


                    case ConsoleKey.NumPad5:
                        Console.Write("\nВведите имя для поиска по записной книге: ");
                        var searchPerson = new Employee();
                        searchPerson.FindContainingElement();
                        break;


                    case ConsoleKey.NumPad0:
                        Environment.Exit(1);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
