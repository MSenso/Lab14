using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonLibrary;

namespace Lab14
{
    public class Program
    {
        const int faculty_size = 7;
        const int university_size = 3;
        static Random random = new Random();
        static List<List<Person>> university = null;
        static int InputNumber(string ForUser, int left, int right)
        {
            bool ok;
            int number = 0;
            do
            {
                Console.WriteLine(ForUser);
                try
                {
                    string buf = Console.ReadLine();
                    number = Convert.ToInt32(buf);
                    if (number >= left && number <= right) ok = true;
                    else
                    {
                        Console.WriteLine("Неверный ввод числа!");
                        ok = false;
                    }
                }
                catch (OverflowException)
                {
                    Console.WriteLine("Неверный ввод числа!");
                    ok = false;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Неверный ввод числа!");
                    ok = false;
                }
            }
            while (!ok);
            return number;
        }

        #region Make
        static List<Person> Make_Faculty()
        {
            List<Person> faculty = new List<Person>(faculty_size);
            for(int i = 0; i < faculty_size; i++)
            {
                int case_value = random.Next(4);
                switch(case_value)
                {
                    case 0:
                        {
                            faculty.Add(new Person());
                            break;
                        }
                    case 1:
                        {
                            faculty.Add(new Student());
                            break;
                        }
                    case 2:
                        {
                            faculty.Add(new Researcher());
                            break;
                        }
                    case 3:
                        {
                            faculty.Add(new Professor());
                            break;
                        }
                }
            }
            return faculty;
        }
        public static void Make_University()
        {
            university = new List<List<Person>>(university_size);
            for(int i = 0; i < university_size; i++)
            {
                university.Add(Make_Faculty());
            }
            Show_University();
        }
        #endregion
        #region Show
        static void Show_List(List<Person> list)
        {
            foreach (Person person in list)
            {
                person.Show();
            }
            Console.WriteLine();
        }
        static void Show_University()
        {
            foreach(List<Person> faculty in university)
            {
                Show_List(faculty);
            }
        }
        #endregion
        #region Select
        public static void Select_Students(int course)
        {
            List<Person> specific_students = university.SelectMany(faculty => faculty).Where(student => (student is Student)).Where(student => (student as Student).Course == course).ToList();
            Console.WriteLine("С помощью методов расширения: ");
            Show_List(specific_students);
            specific_students = (from faculty in university
                                 from person in faculty
                                 where person is Student
                                 where (person as Student).Course == course
                                 select person).ToList();
            Console.WriteLine("С помощью LINQ запроса: ");
            Show_List(specific_students);
        }
        #endregion
        #region Count
        public static void Count_People_By_Gender(string gender)
        {
            switch (gender)
            {
                case "Мужской":
                    {
                        int count_gender = university.SelectMany(faculty => faculty).Count(person => person.Is_male);
                        Console.WriteLine("С помощью методов расширения: " + count_gender.ToString());
                        count_gender = (from faculty in university
                                           from person in faculty
                                               where person.Is_male
                                               select person).Count();
                        Console.WriteLine("С помощью LINQ запроса: " + count_gender.ToString());
                        break;
                    }
                case "Женский":
                    {
                        int count_gender = university.SelectMany(faculty => faculty).Count(person => !person.Is_male);
                        Console.WriteLine("С помощью методов расширения: " + count_gender.ToString());
                        count_gender = (from faculty in university
                                        from person in faculty
                                        where !person.Is_male
                                        select person).Count();
                        Console.WriteLine("С помощью LINQ запроса: " + count_gender.ToString());
                        break;
                    }
            }
        }
        #endregion
        #region Sets
        public static void Persons_From_Two_Faculty()
        {
            List<Person> unique_names = university[0].Union(university[1]).ToList();
            Console.WriteLine("С помощью методов расширения: ");
            Show_List(unique_names);
            unique_names = (from person1 in university[0] select person1).Union(from person2 in university[1] select person2).ToList();
            Console.WriteLine("С помощью LINQ запроса: ");
            Show_List(unique_names);
        }
        #endregion
        #region Aggregation
        public static void Average_Students_Score()
        {
            double average_score = university.SelectMany(faculty => faculty).Where(person => person is Student).Average(person => (person as Student).Rating);
            Console.WriteLine("С помощью методов расширения: " + average_score.ToString());
            average_score = (from faculty in university
                             from person in faculty
                             where person is Student
                             select (person as Student).Rating).Average();
            Console.WriteLine("С помощью LINQ запроса: " + average_score.ToString());
        }
        #endregion
        #region Group
        public static void Group_Students()
        {
            var group1 = university.SelectMany(faculty => faculty).Where(person => person is Student).GroupBy(student => (student as Student).Course).OrderBy(pair => pair.Key);
            Console.WriteLine("С помощью методов расширения:");
            foreach(var pair in group1)
            {
                Console.WriteLine($"Курс {pair.Key}:");
                foreach(Person person in pair)
                {
                    person.Show();
                }
            }
            var group2 = from faculty in university
                    from person in faculty
                    where person is Student
                    orderby (person as Student).Course
                    group person by (person as Student).Course;
            Console.WriteLine("С помощью LINQ запроса:");
            foreach (var pair in group2)
            {
                Console.WriteLine($"Курс {pair.Key}:");
                foreach (Person person in pair)
                {
                    person.Show();
                }
            }
        }
        #endregion
        static void Print_Menu()
        {
            Console.WriteLine(@"1. Создать список
2. Вывести список
3. Найти всех студентов указанного курса
4. Найти количество людей определенного пола
5. Вывести список людей двух факультетов
6. Вычислить средний рейтинг студентов
7. Сгруппировать студентов по курсу
8. Выход");
        }
        static void Menu()
        {
            int choice = 0;
            do
            {
                Print_Menu();
                choice = InputNumber("", 1, 8);
                switch (choice)
                {
                    case 1:
                        {
                            Make_University();
                            break;
                        }
                    case 2:
                        {
                            Show_University();
                            break;
                        }
                    case 3:
                        {
                            int course = InputNumber("Введите номер курса: ", 1, 5);
                            Select_Students(course);
                            break;
                        }
                    case 4:
                        {
                            bool is_correct_input = true;
                            do
                            {
                                Console.WriteLine(@"Введите ""Мужской"" или ""Женский""");
                                string gender = Console.ReadLine();
                                if (gender == "Мужской" || gender == "Женский")
                                    Count_People_By_Gender(gender);
                                else
                                {
                                    Console.WriteLine("Некорректный ввод!");
                                    is_correct_input = false;
                                }
                            } while (!is_correct_input);
                            break;
                        }
                    case 5:
                        {
                            Persons_From_Two_Faculty();
                            break;
                        }
                    case 6:
                        {
                            Average_Students_Score();
                            break;
                        }
                    case 7:
                        {
                            Group_Students();
                            break;
                        }
                    default: break;
                }
            } while (choice < 8);
        }
        static void Main(string[] args)
        {
            Menu();
        }
    }
}
