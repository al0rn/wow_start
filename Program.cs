#define WITHSOUNDS

using System;



namespace WoW_Auto_Login
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Для настройки запусти c параметром командной строки \"-config\"");

            string password = "", wow_exe = "";
            int delay_launch = 0, delay_login = 0;
            bool entring_world = false;

            #region Настройка
            if (args.Length != 0)
                if (args[0] == "-config")
                {
                    Console.WriteLine("Режим настройки.");

                    Console.WriteLine(@"Введи путь к исполняемому файлу вова (например, C:\G\wotlk\WoW.exe)");
                    wow_exe = Console.ReadLine().Trim();
                    Console.WriteLine("Введи пароль, который будет использоваться для авторизации.");
                    password = Console.ReadLine().Trim();
                    Console.WriteLine("Введи время в секундах, которое требуется выждать для запуска клиента.");
                    delay_launch = Convert.ToInt32(Console.ReadLine()) * 1000;
                    Console.WriteLine("Нужно ли входить в мир? Нажми Y/N.");
                    if (Console.ReadKey(true).Key == ConsoleKey.Y)
                    {
                        entring_world = true;
                        Console.WriteLine("Введи время в секундах, которое требуется выждать для входа в меню выбора персонажей.");
                        delay_login = Convert.ToInt32(Console.ReadLine()) * 1000;
                    }
                    else
                    {
                        entring_world = false;
                        delay_login = 0;
                    }

                    System.IO.StreamWriter writer = new System.IO.StreamWriter(System.Environment.CurrentDirectory + @"/config_wal.txt", false);
                    writer.AutoFlush = true;
                    writer.WriteLine("wow_exe=" + wow_exe);
                    writer.WriteLine("password=" + password);
                    writer.WriteLine("delay_launch=" + delay_launch);
                    writer.WriteLine("delay_login=" + delay_login);
                    writer.WriteLine("entring_world=" + entring_world);
                    writer.Close();

                    Console.WriteLine("Настройка завершена. Выход через 5 секунд.");
#if WITHSOUNDS
                    Console.Beep();
#endif
                    System.Threading.Thread.Sleep(5000);
                    return;
                }
            #endregion

            #region Загрузка настроек
            Console.WriteLine("Попытка загрузить настройки из \"" + System.Environment.CurrentDirectory + @"\" + "config_wal.txt\"");

            if (System.IO.File.Exists(System.Environment.CurrentDirectory + @"/config_wal.txt"))
            {
                Console.WriteLine("Успешно.");

                System.IO.StreamReader reader = new System.IO.StreamReader(System.Environment.CurrentDirectory + @"/config_wal.txt");

                while (!reader.EndOfStream)
                {
                    string line, key, value;
                    line = reader.ReadLine();
                    key = line.Split('=')[0];
                    value = line.Split('=')[1];

                    switch (key)
                    {
                        case "wow_exe": wow_exe = value; break;
                        case "password": password = value; break;
                        case "delay_launch": delay_launch = Convert.ToInt32(value); break;
                        case "delay_login": delay_login = Convert.ToInt32(value); break;
                        case "entring_world": entring_world = Convert.ToBoolean(value); break;
                    }
                }
                reader.Close();
            }
            else
            {
                Console.WriteLine("Невозможно открыть файл настроек. Выход через 5 секунд.");
#if WITHSOUNDS
                Console.Beep();
#endif
                System.Threading.Thread.Sleep(5000);
                return;
            }
            #endregion

            #region Авторизация и вход в мир

            Console.WriteLine("Запуск клиента.");
            System.Diagnostics.Process.Start(wow_exe);

            Console.WriteLine("Ожидание запуска WoW.");
            System.Threading.Thread.Sleep(delay_launch);

            Console.WriteLine("Ввод пароля.");
            System.Windows.Forms.SendKeys.SendWait(password);
            System.Windows.Forms.SendKeys.SendWait("{ENTER}");

            if (entring_world)
            {
                Console.WriteLine("Ожидание входа в меню выбора персонажа.");
                System.Threading.Thread.Sleep(delay_login);

                Console.WriteLine("Вход в мир.");
                System.Windows.Forms.SendKeys.SendWait("{ENTER}");
            }

            Console.WriteLine("Авторизация произведена, выход через 5 секунд.");
#if WITHSOUNDS
            Console.Beep();
#endif
            System.Threading.Thread.Sleep(5000);
            #endregion
        }
    }
}