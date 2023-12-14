using System;
using System.Buffers;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Numerics;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;


namespace Notebook
{
    internal class Employee
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Adress { get; set; }
        public string Note { get; set; }
        public Employee() { }

        //Метод записи в файл
        public void WriteEmployee()
        {
            using (StreamWriter sw = File.AppendText("Notebook.txt"))
            {
                sw.WriteLine($"{this.FirstName}|{this.LastName}|{this.Phone}|{this.Adress}|{this.Note}");
            }
        }

        //Метод вывода контакта
        private void PrintPerson()
        {
            Console.WriteLine($"  Имя: {FirstName}\n  Фамилия: {LastName}\n  Номер телефона: {Phone}\n  Адрес: {Adress}\n  Заметки: {Note}");
            Console.WriteLine(new string('-', 50));
        }


        //Метод чтения из файла
        private string[,] ExtractingFromFile()
        {
            using (StreamReader sr = File.OpenText("Notebook.txt"))
            {
                string[] lines = File.ReadAllLines("Notebook.txt");
                string[,] notebook = new string[lines.Length, lines[0].Split('|').Length];
                for (int i = 0; i < lines.Length; i++)
                {
                    string[] temp = lines[i].Split('|');
                    for (int j = 0; j < temp.Length; j++)
                        notebook[i, j] = (temp[j]);
                }
                return notebook;
            }
        }
        //Метод записи массива в файл
        public void SaveArrayToFile(string[,] array)
        {

            int rowCount = array.GetLength(0);
            int columnCount = array.GetLength(1);

            string[] lines = new string[rowCount];

            for (int i = 0; i < rowCount; i++)
            {
                string[] values = new string[columnCount];

                for (int j = 0; j < columnCount; j++)
                {
                    values[j] = array[i, j];
                }

                lines[i] = string.Join("|", values);
            }

            File.WriteAllLines("Notebook.txt", lines);
        }

        //Метод сортировки контактов
        private string[,] OrganiseContactsAlphabetically()
        {
            string[,] array = ExtractingFromFile();
            //столбец для сортировки 
            int column = 0;
            {
                int rows = array.GetLength(0);
                int cols = array.GetLength(1);

                for (int i = 0; i < rows; i++)
                {
                    for (int j = i + 1; j < rows; j++)
                    {
                        if (string.Compare(array[i, column], array[j, column], StringComparison.Ordinal) > 0)
                        {
                            // замена значений строк
                            for (int k = 0; k < cols; k++)
                            {
                                string temp = array[i, k];
                                array[i, k] = array[j, k];
                                array[j, k] = temp;
                            }
                        }
                    }
                }
            }

            return array;
        }


        //Метод вывода списка контактов 
        public void PrintArray()
        {
            Console.Clear();
            Console.WriteLine(new string('-', 50));
            Console.WriteLine("\tСписок контактов в алфавитном порядке");
            Console.WriteLine(new string('-', 50));

            string[,] array = OrganiseContactsAlphabetically();
            int rows = array.GetLength(0);

            for (int i = 0; i < rows; i++)
            {
                FirstName = array[i, 0];
                LastName = array[i, 1];
                Phone = array[i, 2];
                Adress = array[i, 3];
                Note = array[i, 4];
                PrintPerson();
            }
            Console.ReadKey();
        }


        //Поиск нужного контакта и вывод его
        public void FindContainingElement()
        {
            bool found = false;
            string[,] array = ExtractingFromFile();
            do
            {
                string searchValue = Console.ReadLine().ToLower();
                found = false;
                for (int i = 0; i < array.GetLength(0); i++)
                {
                    for (int j = 0; j < array.GetLength(1); j++)
                    {
                        if (array[i, j].ToLower().Contains(searchValue))
                        {
                            FirstName = array[i, 0];
                            LastName = array[i, 1];
                            Phone = array[i, 2];
                            Adress = array[i, 3];
                            Note = array[i, 4];
                            PrintPerson();
                            found = true;
                            Console.ReadKey();
                        }
                    }
                }
                if (!found)
                {
                    Console.WriteLine("Контакт не найден. Попробуйте снова.");
                }
            } while (!found);
        }



        //Метод проверки номера
        public void GetPhoneNumber(string input)
        {
            while (true)
            {
                bool isValid = true;
                foreach (char c in input)
                {
                    if (!char.IsDigit(c) && c != '+' && c != '-' && c != '(' && c != ')' && c != ' ')
                    {
                        isValid = false;
                    }
                }
                if (isValid)
                {
                    Phone = input;
                    break;
                }
                else
                {
                    Console.WriteLine("Номер содержит недопустимые символы. Пожалуйста, введите еще раз.");
                }
            }
        }


        //Метод редактирования контакта из записной книжки
        public string[,] SearchEditCurrentPerson()
        {
            string[] columns = { "Имя", "Фамилия", "Номер телефона", "Адрес", "Заметки" };
            string[,] array = OrganiseContactsAlphabetically();
            Console.Write("Введите информацию содержащуюся в контакте: ");
            string searchValue = Console.ReadLine().ToLower();

            bool found = false;

            // Поиск частичного или полного совпадения
            for (int i = 0; i < array.GetLength(0); i++)
            {
                string rowString = "";

                for (int j = 0; j < array.GetLength(1); j++)
                {
                    if (array[i, j].ToLower().Contains(searchValue))
                    {
                        found = true;
                        FirstName = array[i, 0];
                        LastName = array[i, 1];
                        Phone = array[i, 2];
                        Adress = array[i, 3];
                        Note = array[i, 4];
                        break;
                    }
                }
                if (found)
                {
                    Console.WriteLine("Найден контакт");
                    PrintPerson();
                    Console.WriteLine("Хотите внести изменения в этот контакт? (да/нет)");
                    string editChoice = Console.ReadLine();

                    if (editChoice.ToLower() == "да")
                    {
                        EditRow(array, i, columns);
                    }
                    break;
                }
            }
            if (!found)
            {
                Console.WriteLine("Контакт с такими данными не найден");
                Console.ReadKey();
            }
            SaveArrayToFile(array);
            return array;
        }


        // Метод для редактирования строки в двумерном массиве
        private void EditRow(string[,] array, int rowIndex, string[] columns)
        {
            Console.WriteLine("Введите новые данные или нажмите Enter чтобы не менять их");
            for (int j = 0; j < array.GetLength(1); j++)
            {
                Console.Write($"{columns[j]}: ");
                string newValue = Console.ReadLine();

                if (!string.IsNullOrEmpty(newValue))
                {
                    array[rowIndex, j] = newValue;
                }
            }
            Console.WriteLine("Контакт отредактирован.");
        }


        //Удаление строки из массива
        public void DeletePerson()
        {
            string[,] array = OrganiseContactsAlphabetically();
            Console.Write("Введите значение для поиска: ");
            string searchValue = Console.ReadLine().ToLower();
            int rows = array.GetLength(0);
            int cols = array.GetLength(1);
            int foundRowIndex = -1;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (array[i, j].ToLower().Contains(searchValue))
                    {
                        foundRowIndex = i;
                        break;
                    }
                }
                if (foundRowIndex != -1)
                {
                    break;
                }
            }
            if (foundRowIndex == -1)
            {
                Console.WriteLine("Совпадений не найдено.");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("Найден контакт:");
            for (int j = 0; j < cols; j++)
            {
                FirstName = array[foundRowIndex, 0];
                LastName = array[foundRowIndex, 1];
                Phone = array[foundRowIndex, 2];
                Adress = array[foundRowIndex, 3];
                Note = array[foundRowIndex, 4];
            }
            PrintPerson();
            Console.WriteLine();
            Console.WriteLine("Хотите удалить этот контакт (да/нет)");
            string deleteChoice = Console.ReadLine();

            if (deleteChoice.ToLower() == "да")
            {
                string[,] newArray = new string[rows - 1, cols];

                int newRowIndex = 0;
                for (int i = 0; i < rows; i++)
                {
                    if (i != foundRowIndex)
                    {
                        for (int j = 0; j < cols; j++)
                        {
                            newArray[newRowIndex, j] = array[i, j];
                        }
                        newRowIndex++;
                    }
                }

                array = newArray;
                SaveArrayToFile(array);
                Console.WriteLine("Контакт успешно удален.");
                Console.ReadKey();
            }


        }

    }

}



