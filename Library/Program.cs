/*
 * Декабрь 9, 2017
 * Описать класс "Домашняя библиотека". Предусмотреть возможности работы с произвольным числом книг, поиска книги по какому-то признаку (например, по автору
 * или по году издания), добавление книг в библиотеку, удаления книг из неё, сортировка книг по разным полям.
 */

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Library
{
    /*
     * Демонстрация наследования через включение. Есть два класса: Library и Book. Книга должна находится в библиотеке. Использование классического наследования
     * немного нелогичное, поскольку библиотека не является книгой, а книга не является библиотекой. Эти два класса между собой нужно как-то связать, чтобы по смыслу
     * книга располагалась в библиотеке. Поэтому, удобно всего использовать наследование через включение, то есть, класс Library будет включать в себя класс Book.
     */

    #region Class Library

    public static class Library
    {
        private static Book[] Books { get; set; } // Массив книг. Приватное свойство класса Library.
        public static int Count { get; set; } // Количество книг в библиотеке.

        #region Class Book

        private class Book
        {
            public string Name { get; } // Название книги. Свойство класса Book.
            public string Genre { get; } // Жанр книги. Свойство класса Book.
            public string Author { get; } // Автор книги. Свойство класса Book.
            public int Year { get; } // Год издания книги. Свойство класса Book.

            public Book(string name, string genre, string author, int year) // Конструктор по умполчанию класса Book.
            {
                if (name == null)
                {
                    throw new ArgumentNullException(nameof(name));
                }
                if (string.IsNullOrWhiteSpace(name))
                {
                    throw new ArgumentException(nameof(name));
                }

                if (genre == null)
                {
                    throw new ArgumentNullException(nameof(genre));
                }
                if (string.IsNullOrWhiteSpace(genre))
                {
                    throw new ArgumentException(nameof(genre));
                }

                if (author == null)
                {
                    throw new ArgumentNullException(nameof(author));
                }
                if (string.IsNullOrWhiteSpace(author))
                {
                    throw new ArgumentException(nameof(author));
                }

                if (year <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(year));
                }

                // Блок кода инициализации свойств класса Book.
                Name = name;
                Genre = genre;
                Author = author;
                Year = year;
            }

            public void OutputInfoBook() // Метод класса Book. Вывод всей информации о данной книги.
            {
                Console.WriteLine("Основная информация:");
                Console.WriteLine($"Название книги: {Name}\nЖанр книги: {Genre}\nАвтор книги: {Author}\nГод издания: {Year}\n");
            }
        }

        #endregion

        #region Add Book(s) in Library

        public static void AddBook(int count) // Метод класса Library. Добавление одной или несколько книг в библиотеку.
        {
            if (Count == 0)
            {
                InitializationLibrary(count);
            }
            else
            {
                Book[] tempBooks = Books;
                InitializationLibrary(count);
                Book[] newBooks = new Book[Count + tempBooks.Length];
                for (int i = 0; i < newBooks.Length; i++)
                {
                    if (i < tempBooks.Length)
                    {
                        newBooks[i] = tempBooks[i];
                    }
                    else
                    {
                        newBooks[i] = Books[i - tempBooks.Length];
                    }
                }
                Books = newBooks;
                Count = newBooks.Length;
            }
        }

        private static void InitializationLibrary(int count) // Метод класса Library. Инициализация библиотеки.
        {
            Count = count;
            Books = new Book[Count];
            for (int i = 0; i < Books.Length; i++)
            {
                Console.WriteLine($"\n{i + 1} книга.");
                string name = InputNameBook();
                string genre = InputGenreBook();
                string author = InputAuthorBook();
                int year = InputYearBook();
                Books[i] = new Book(name, genre, author, year);
            }
        }

        private static string InputNameBook() // Приватный метод класса Library. Ввод название книги.
        {
            while (true)
            {
                Regex pattern = new Regex(@"(\s*[А-ЯЁа-яё])+"); // Регулярное выражение: ввод текста с неограниченным количеством слов без использовании точек, запятых и т.д..
                Console.Write("Название книги: ");
                string name = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(name))
                {
                    MatchCollection matches = pattern.Matches(name);
                    if (matches.Count == 1)
                    {
                        return name;
                    }
                }
                Console.WriteLine("Некорретные данные. Значение должно соответствовать маске ввода: слова, состояющие только из русских букв любого регистра " +
                                  "(например, Мастер и Маргарита). Повторите ввод.");
            }
        }

        private static string InputGenreBook() // Приватный метод класса Library. Ввод жанра книги.
        {
            while (true)
            {
                Regex pattern = new Regex(@"[А-Яа-я]{4,}"); // Регулярное выражение: ввод одного русского слова в любом регистре, минимум символов в слове — 4.
                Console.Write("Жанр книги: ");
                string genre = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(genre))
                {
                    MatchCollection matches = pattern.Matches(genre);
                    if (matches.Count == 1)
                    {
                        return genre;
                    }
                }
                Console.WriteLine("Некорретные данные. Значение должно соответствовать маске ввода: слово, состояющее только из русских букв, минимум 4 символа в слове " +
                                  "(например, Роман). Повторите ввод.");
            }
        }

        private static string InputAuthorBook() // Приватный метод класса Library. Ввод автора книги.
        {
            while (true)
            {
                Regex pattern = new Regex(@"[А-Я][а-я]+\s[А-Я]\.[А-Я]\."); // Регулярное выражение: ввод автора книги по такой маске: Маяковский В.В..
                Console.Write("Автор книги: ");
                string author = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(author))
                {
                    MatchCollection matches = pattern.Matches(author);
                    if (matches.Count == 1)
                    {
                        return author;
                    }
                }
                Console.WriteLine("Некорретные данные. Значение должно соответствовать маске ввода: слово (состоящее только из русских букв) + большие буквенные инициалы " +
                                  "с точкой (например, Маяковский В.В.). Повторите ввод.");
            }
        }

        private static int InputYearBook() // Приватный метод класса Library. Ввод года издания книги, количество цифр в числе — 4, цифры от 0 до 9.
        {
            while (true)
            {
                Regex pattern = new Regex(@"[0-9]{4}");
                Console.Write("Год издания книги: ");
                string year = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(year))
                {
                    MatchCollection matches = pattern.Matches(year);
                    if (matches.Count == 1 && int.TryParse(year, out var yearResult) && yearResult > 0)
                    {
                        return yearResult;
                    }
                    Console.WriteLine("Значение должно быть пложительным и должно соответствовать маске ввода: число, состоящее из 4 цифр, цифры от 0 до 9. Повторите ввод.");
                }
                else
                {
                    Console.WriteLine("Значение должно быть целым числом. Повторите ввод.");
                }
            }
        }

        #endregion

        #region Delete Book(s) from Library

        public static void DeleteBook() // Метод класса Library. Удаление одной или несколько книг из библиотеки по заданному критерию (название, жанр, автор, год издания).
        {
            int type = DeleteType();
            switch (type)
            {
                case 1:
                    if (Books != null)
                    {
                        string name = InputNameBook();
                        Console.Write("\nСтарый вариант списка книг в библиотеке до удаления по названию.");
                        OutputLibrary();
                        for (var i = 0; i < Books.Length; i++)
                        {
                            if (String.Compare(Books[i].Name, name, StringComparison.OrdinalIgnoreCase) == 0)
                            {
                                Delete(i);
                                i--;
                            }
                        }
                        Console.Write("Новый вариант списка книг в библиотеке после удаления по названию.");
                        OutputLibrary();
                    }
                    else
                    {
                        Console.WriteLine("\nВ библиотеке нет книг! Удаление было прервано.");
                    }
                    break;
                case 2:
                    if (Books != null)
                    {
                        string genre = InputGenreBook();
                        Console.Write("\nСтарый вариант списка книг в библиотеке до удаления по жанру.");
                        OutputLibrary();
                        for (var i = 0; i < Books.Length; i++)
                        {
                            if (String.Compare(Books[i].Genre, genre, StringComparison.OrdinalIgnoreCase) == 0)
                            {
                                Delete(i);
                                i--;
                            }
                        }
                        Console.Write("Новый вариант списка книг в библиотеке после удаления по жанру.");
                        OutputLibrary();
                    }
                    else
                    {
                        Console.WriteLine("\nВ библиотеке нет книг! Удаление было прервано.");
                    }
                    break;
                case 3:
                    if (Books != null)
                    {
                        string author = InputAuthorBook();
                        Console.Write("\nСтарый вариант списка книг в библиотеке до удаления по автору.");
                        OutputLibrary();
                        for (var i = 0; i < Books.Length; i++)
                        {
                            if (String.Compare(Books[i].Author, author, StringComparison.OrdinalIgnoreCase) == 0)
                            {
                                Delete(i);
                                i--;
                            }
                        }
                        Console.Write("Новый вариант списка книг в библиотеке после удаления по автору.");
                        OutputLibrary();
                    }
                    else
                    {
                        Console.WriteLine("\nВ библиотеке нет книг! Удаление было прервано.");
                    }
                    break;
                case 4:
                    if (Books != null)
                    {
                        int year = InputYearBook();
                        Console.Write("\nСтарый вариант списка книг в библиотеке до удаления по году издания.");
                        OutputLibrary();
                        for (int i = 0; i < Books.Length; i++)
                        {
                            if (Books[i].Year == year)
                            {
                                Delete(i);
                                i--;
                            }
                        }
                        Console.Write("Новый вариант списка книг в библиотеке после удаления по году издания.");
                        OutputLibrary();
                    }
                    else
                    {
                        Console.WriteLine("\nВ библиотеке нет книг! Удаление было прервано.");
                    }
                    break;
            }
        }

        private static int DeleteType() // Приватный метод класса Library. Выбор критерия удаления книг(и) из библиотеки.
        {
            Console.WriteLine("\nОсновные критерии удаление книг из библиотеки:\n1. Удаление по названию:\n2. Удаление по жанру.\n3. Удаление по автору.\n" +
                              "4. Удаление по году издания.\n");
            Console.Write("Введите номер типа, по которому будет процесс удаления книг из библиотеки: ");
            while (true)
            {
                string type = Console.ReadLine();
                if (int.TryParse(type, out var typeResult))
                {
                    if (typeResult > 0 && typeResult < 5)
                    {
                        return typeResult;
                    }
                    Console.Write("Значение должно находится в диапазоне от 1 до 4. Повторите ввод: ");
                }
                else
                {
                    Console.Write("Значение должно быть целым числом. Повторите ввод: ");
                }
            }
        }

        private static void Delete(int index) // Приватный метод класса Library. Процесс удаления одной книги из библиотеки (удаление одного элемента из массива типа класса Book).
        {
            Book[] newBooks = new Book[Books.Length - 1];
            for (int i = 0; i < newBooks.Length; i++)
            {
                if (i >= index)
                {
                    newBooks[i] = Books[i + 1];
                }
                else
                {
                    newBooks[i] = Books[i];
                }
            }
            Books = newBooks;
            Count = newBooks.Length;
        }

        public static void DeleteAllBooks() // Метод класса Library. Удаление всех существующих книг в библиотеке.
        {
            Books = null;
            Count = 0;
            Console.WriteLine("\nУдалены все существующие книги в библиотеке!");
        }

        #endregion

        #region Search Book(s) in Library

        public static void SearchBook()
        {
            int type = SearchType();
            switch (type)
            {
                case 1:
                    if (Books != null)
                    {
                        SearchBookByName();
                    }
                    else
                    {
                        Console.WriteLine("\nВ библиотеке нет книг! Поиск по названию был прерван.");
                    }
                    break;
                case 2:
                    if (Books != null)
                    {
                        SearchBookByGenre();
                    }
                    else
                    {
                        Console.WriteLine("\nВ библиотеке нет книг! Поиск по жанру был прерван.");
                    }
                    break;
                case 3:
                    if (Books != null)
                    {
                        SearchBookByAuthor();
                    }
                    else
                    {
                        Console.WriteLine("\nВ библиотеке нет книг! Поиск по автору был прерван.");
                    }
                    break;
                case 4:
                    if (Books != null)
                    {
                        SearchBookByYear();
                    }
                    else
                    {
                        Console.WriteLine("\nВ библиотеке нет книг! Поиск по году издания был прерван.");
                    }
                    break;
            }
        }

        private static int SearchType()
        {
            Console.WriteLine("\nОсновные критерии поиска книг в библиотеке:\n1. Поиск по названию:\n2. Поиск по жанру.\n3. Поиск по автору.\n" +
                              "4. Поиск по году издания.\n");
            Console.Write("Введите номер типа, по которому будет процесс поиска книг в библиотеке: ");
            while (true)
            {
                string type = Console.ReadLine();
                if (int.TryParse(type, out var typeResult))
                {
                    if (typeResult > 0 && typeResult < 5)
                    {
                        return typeResult;
                    }
                    Console.Write("Значение должно находится в диапазоне от 1 до 4. Повторите ввод: ");
                }
                else
                {
                    Console.Write("Значение должно быть целым числом. Повторите ввод: ");
                }
            }
        }

        private static void SearchBookByName() // Приватный метод класса Library. Поиск книги по названию.
        {
            string name = InputNameBook();
            bool flag = false;
            foreach (Book book in Books)
            {
                if (String.Compare(book.Name, name, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    Console.WriteLine("\nРезультат поиска: книга найдена.\n");
                    book.OutputInfoBook();
                    flag = true;
                }
            }
            if (!flag)
            {
                Console.WriteLine("\nРезультат поиска: книга не найдена.");
            }
        }

        private static void SearchBookByGenre() // Приватный метод класса Library. Поиск книг(и) по жанру.
        {
            string genre = InputGenreBook();
            bool flag = false;
            for (var i = 0; i < Books.Length; i++)
            {
                Book book = Books[i];
                if (String.Compare(book.Genre, genre, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    if (!flag)
                    {
                        Console.WriteLine("\nРезультат поиска: книги(а) найдены(а).\n");
                    }
                    Console.WriteLine($"{i + 1} книга:");
                    book.OutputInfoBook();
                    flag = true;
                }
            }
            if (!flag)
            {
                Console.WriteLine("\nРезультат поиска: книги(а) не найдены(а).\n");
            }
        }

        private static void SearchBookByAuthor() // Приватный метод класса Library. Поиск книг(и) по автору.
        {
            string author = InputAuthorBook();
            bool flag = false;
            for (var i = 0; i < Books.Length; i++)
            {
                Book book = Books[i];
                if (String.Compare(book.Author, author, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    if (!flag)
                    {
                        Console.WriteLine("\nРезультат поиска: книги(а) найдены(а).\n");
                    }
                    Console.WriteLine($"{i + 1} книга:");
                    book.OutputInfoBook();
                    flag = true;
                }
            }
            if (!flag)
            {
                Console.WriteLine("\nРезультат поиска: книги(а) не найдены(а).");
            }
        }

        private static void SearchBookByYear() // Приватный метод класса Library. Поиск книг(и) по году издания.
        {
            int year = InputYearBook();
            bool flag = false;
            for (var i = 0; i < Books.Length; i++)
            {
                Book book = Books[i];
                if (book.Year == year)
                {
                    if (!flag)
                    {
                        Console.WriteLine("\nРезультат поиска: книги(а) найдены(а).\n");
                    }
                    Console.WriteLine($"{i + 1} книга:");
                    book.OutputInfoBook();
                    flag = true;
                }
            }
            if (!flag)
            {
                Console.WriteLine("\nРезультат поиска: книги(а) не найдены(а).");
            }
        }

        #endregion

        #region Sorting Books

        public static void SortingBooks() // Метод класса Library. Сортировка книг в библиотеке по заданному критерию (название, жанр, автор, год издания).
        {
            int type = SortingType();
            switch (type)
            {
                case 1:
                    if (Books != null)
                    {
                        Console.Write("\nСтарый вариант списка книг в библиотеке до сортировки по названию.");
                        OutputLibrary();
                        Array.Sort(Books, new BooksComparerByName());
                        Console.Write("Новый вариант списка книг в библиотеке после сортировки по названию.");
                        OutputLibrary();
                    }
                    else
                    {
                        Console.WriteLine("\nВ библиотеке нет книг! Сортировка по названию была прервана.");
                    }
                    break;
                case 2:
                    if (Books != null)
                    {
                        Console.Write("\nСтарый вариант списка книг в библиотеке до сортировки по жанру.");
                        OutputLibrary();
                        Array.Sort(Books, new BooksComparerByGenre());
                        Console.Write("Новый вариант списка книг в библиотеке после сортировки по жанру.");
                        OutputLibrary();
                    }
                    else
                    {
                        Console.WriteLine("\nВ библиотеке нет книг! Сортировка по жанру была прервана.");
                    }
                    break;
                case 3:
                    if (Books != null)
                    {
                        Console.Write("\nСтарый вариант списка книг в библиотеке до сортировки по автору.");
                        OutputLibrary();
                        Array.Sort(Books, new BooksComparerByAuthor());
                        Console.Write("Новый вариант списка книг в библиотеке после сортировки по автору.");
                        OutputLibrary();
                    }
                    else
                    {
                        Console.WriteLine("\nВ библиотеке нет книг! Сортировка по автору была прервана.");
                    }
                    break;
                case 4:
                    if (Books != null)
                    {
                        Console.Write("\nСтарый вариант списка книг в библиотеке до сортировки по году издания.");
                        OutputLibrary();
                        Array.Sort(Books, new BooksComparerByYear());
                        Console.Write("Новый вариант списка книг в библиотеке после сортировки по году издания.");
                        OutputLibrary();
                    }
                    else
                    {
                        Console.WriteLine("\nВ библиотеке нет книг! Сортировка по году издания была прервана.");
                    }
                    break;
            }
        }

        private static int SortingType() // Приватный метод класса Library. Выбор критерия сортировки книг(и) в библиотеке.
        {
            Console.WriteLine("\nОсновные критерии сортировки книг в библиотеке:\n1. Сортировка по названию.\n2. Сортировка по жанру.\n3. Сортировка по автору.\n" +
                              "4. Сортировка по году издания.\n");
            Console.Write("Введите номер типа, по которому будет процесс сортировки книг в библиотеке: ");
            while (true)
            {
                string type = Console.ReadLine();
                if (int.TryParse(type, out var typeResult))
                {
                    if (typeResult > 0 && typeResult < 5)
                    {
                        return typeResult;
                    }
                    Console.Write("Значение должно находится в диапазоне от 1 до 4. Повторите ввод: ");
                }
                else
                {
                    Console.Write("Значение должно быть целым числом. Повторите ввод: ");
                }
            }
        }

        #endregion

        #region Editing information about Book(s) in Library 

        public static void EditingInformationAboutBook(int count) // Метод класса Library. Редактирование информации одной или несколько книг в библиотеке.
        {
            if (Books != null)
            {
                OutputLibrary();
                for (int i = 0; i < count; i++)
                {
                    Console.Write($"{i + 1} редактирование информации. Укажите нужный номер книги из списка: ");
                    int number = InputBookNumberForEditing(Books.Length);

                    string name = null, genre = null, author = null;
                    int year = 0;

                    Console.Write("Редактировать название книги? (Y/N) ");
                    char key = AnswerTheQuestionLibrary();
                    if (key == 'Y' || key == 'y')
                    {
                        Console.Write("\n");
                        name = InputNameBook();
                    }
                    else
                    {
                        Console.Write("\n");
                        name = Books[number - 1].Name;
                    }

                    Console.Write("Редактировать жанр книги? (Y/N) ");
                    key = AnswerTheQuestionLibrary();
                    if (key == 'Y' || key == 'y')
                    {
                        Console.Write("\n");
                        genre = InputGenreBook();
                    }
                    else
                    {
                        Console.Write("\n");
                        genre = Books[number - 1].Genre;
                    }

                    Console.Write("Редактировать автора книги? (Y/N) ");
                    key = AnswerTheQuestionLibrary();
                    if (key == 'Y' || key == 'y')
                    {
                        Console.Write("\n");
                        author = InputAuthorBook();
                    }
                    else
                    {
                        Console.Write("\n");
                        author = Books[number - 1].Author;
                    }

                    Console.Write("Редактировать год издания книги? (Y/N) ");
                    key = AnswerTheQuestionLibrary();
                    if (key == 'Y' || key == 'y')
                    {
                        Console.Write("\n");
                        year = InputYearBook();
                    }
                    else
                    {
                        Console.Write("\n");
                        year = Books[number - 1].Year;
                    }

                    Books[number - 1] = new Book(name, genre, author, year);
                }
            }
            else
            {
                Console.WriteLine("\nВ библиотеке нет книг! Редактирование книг(и) было прервано.");
            }
        }

        private static int InputBookNumberForEditing(int length) // Приватный метод класса Library. Ввод номера одной книги из списка для редактирвоания.
        {
            while (true)
            {
                string number = Console.ReadLine();
                if (int.TryParse(number, out int numberResult))
                {
                    if (numberResult > 0 && numberResult <= length)
                    {
                        return numberResult;
                    }
                    Console.Write("Значение должно совпадать с номерами книг из списка в библиотеке. Повторите ввод: ");
                }
                else
                {
                    Console.Write("Значение должно быть целым числом. Повторите ввод: ");
                }
            }
        }

        private static char AnswerTheQuestionLibrary() // Приватный метод класса Library. Ответ на вопрос о редактировании полей одной книги.
        {
            do
            {
                char key = Console.ReadKey().KeyChar;
                if (key == 'Y' || key == 'y' || key == 'N' || key == 'n')
                {
                    return key;
                }
                Console.Write("\n");
            } while (true);
        }

        #endregion

        #region Output Library Class

        public static void OutputLibrary() // Метод класса Library. Вывод всей информации о библиотеке (вывод всей информации по всем книгам).
        {
            Console.WriteLine("\nВывод коллекции книг в библиотеке:\n");
            if (Books == null)
            {
                Console.WriteLine("Библиотека пустая.");
            }
            else
            {
                for (var i = 0; i < Books.Length; i++)
                {
                    Console.WriteLine($"{i + 1} книга.");
                    Books[i].OutputInfoBook();
                }
            }
        }

        #endregion

        #region Classes BooksComparer 

        private class BooksComparerByName : IComparer<Book> // Приватный класс-компоратор для сравнения двух книг по названию.
        {
            public int Compare(Book x, Book y)
            {
                return String.Compare(x.Name, y.Name, StringComparison.OrdinalIgnoreCase);
            }
        }

        private class BooksComparerByGenre : IComparer<Book> // Приватный класс-компоратор для сравнения двух книг по жанру.
        {
            public int Compare(Book x, Book y)
            {
                return String.Compare(x.Genre, y.Genre, StringComparison.OrdinalIgnoreCase);
            }
        }

        private class BooksComparerByAuthor : IComparer<Book> // Приватный класс-компоратор для сравнения двух книг по автору.
        {
            public int Compare(Book x, Book y)
            {
                return String.Compare(x.Author, y.Author, StringComparison.OrdinalIgnoreCase);
            }
        }

        private class BooksComparerByYear : IComparer<Book> // Приватный класс-компоратор для сравнения двух книг по году издания.
        {
            public int Compare(Book x, Book y)
            {
                return x.Year.CompareTo(y.Year);
            }
        }

        #endregion
    }

    #endregion

    #region Class Program

    internal class Program
    {
        #region Several auxiliary functions

        private static int InputCountAddBook() // Приватный метод класса Program. Ввод количества книг для добавления новых книг в библиотеку.
        {
            while (true)
            {
                Console.Write("\nСколько книг хотите добавить в библиотеку: ");
                string count = Console.ReadLine();
                if (int.TryParse(count, out int countResult))
                {
                    if (countResult > 0)
                    {
                        return countResult;
                    }
                    Console.Write("Значение должно быть положительным. Повторите ввод: ");
                }
                else
                {
                    Console.Write("Значение должно быть целым числом. Повторите ввод: ");
                }
            }
        }

        private static int SelectionOfOptionItem() // Приватный метод класса Program. Выбор одной из опции консольного приложения.
        {
            Console.Write("Выберите один из пунктов опции: ");
            while (true)
            {
                string item = Console.ReadLine();
                if (int.TryParse(item, out int itemResult))
                {
                    if (itemResult > 0 && itemResult < 8)
                    {
                        return itemResult;
                    }
                    Console.Write("Значение должно совпадать с одним из пунктов опции. Повторите ввод: ");
                }
                else
                {
                    Console.Write("Значение должно быть целым числом. Повторите ввод: ");
                }
            }
        }

        private static int InputCountEditingBook() // Приватный метод класса Program. Ввод количества книг для добавления новых книг в библиотеку.
        {
            Console.Write("\nСколько книг нужно для редактирования информации: ");
            while (true)
            {
                string count = Console.ReadLine();
                if (int.TryParse(count, out int countResult))
                {
                    if (Library.Count == 0)
                    {
                        return countResult;
                    }
                    if (countResult > 0)
                    {
                        return countResult;
                    }
                    Console.Write("Значение должно совпадать с количеством книг в библиотеке. Повторите ввод: ");
                }
                else
                {
                    Console.Write("Значение должно быть целым числом. Повторите ввод: ");
                }
            }
        }

        private static char AnswerTheQuestionProgram() // Приватный метод класса Library. Ответ на вопрос о продолжении работы консольного приложения.
        {
            do
            {
                char key = Console.ReadKey().KeyChar;
                if (key == 'Y' || key == 'y' || key == 'N' || key == 'n')
                {
                    return key;
                }
                Console.Write("\n");
            } while (true);
        }

        #endregion

        public static void Main()
        {
            while (true)
            {
                Console.Write("Домашняя библиотека. Консольное приложение.\n\n\n");
                Console.Write($"Всего книг в библиотеке: {Library.Count}\n\n");
                Console.WriteLine("Основные пункты опции консольного приложения:");
                Console.Write("1. Добавление новых(ой) книг(и) в библиотеку.\n2. Редактирование информации одной или нескольких книг в библиотеке.\n" +
                                "3. Поиск новых(ой) книг(и) в библиотеке.\n4. Удаление старых(ой) книг(и) из библиотеки.\n5. Сортировка книг в библиотеке.\n" +
                                "6. Вывод всей информации о библиотеке.\n7. Уничтожение библиотеки.\n\n");

                int item = SelectionOfOptionItem();

                switch (item)
                {
                    case 1:
                        int countAddBook = InputCountAddBook();
                        Library.AddBook(countAddBook);
                        break;
                    case 2:
                        int countEditingBook = InputCountEditingBook();
                        Library.EditingInformationAboutBook(countEditingBook);
                        break;
                    case 3:
                        Library.SearchBook();
                        break;
                    case 4:
                        Library.DeleteBook();
                        break;
                    case 5:
                        Library.SortingBooks();
                        break;
                    case 6:
                        Library.OutputLibrary();
                        break;
                    case 7:
                        Library.DeleteAllBooks();
                        break;
                }

                Console.Write("\nПродолжить работу в консольном приложении? (Y/N) ");
                char key = AnswerTheQuestionProgram();

                if (key == 'N' || key == 'n')
                {
                    Console.Write("\n");
                    break;
                }
                Console.Clear();
            }
        }
    }

    #endregion
}