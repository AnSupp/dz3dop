using System;
using static System.Console;

namespace dz3dop
{
    class Program
    {
    //Написать игру, в которою могут играть два игрока.
    //При старте, игрокам предлагается ввести свои никнеймы.
    //Никнеймы хранятся до конца игры.
    //Программа загадывает случайное число gameNumber от 12 до 120 сообщая это число игрокам.
    //Игроки ходят по очереди(игра сообщает о ходе текущего игрока)
    //Игрок, ход которого указан вводит число userTry, которое может принимать значения 1, 2, 3 или 4,
    //введенное число вычитается из gameNumber
    //Новое значение gameNumber показывается игрокам на экране.
    //Выигрывает тот игрок, после чьего хода gameNumber обратилась в ноль.
    //Игра поздравляет победителя, предлагая сыграть реванш
    //         * Бонус:
    //         Подумать над возможностью реализации разных уровней сложности.
    //         В качестве уровней сложности может выступать настраиваемое, в начале игры,
    //         значение userTry, изменение диапазона gameNumber, или указание большего количества игроков (3, 4, 5...)
    
    //         *** Сложный бонус
    //         Подумать над возможностью реализации однопользовательской игры
    //         т е игрок должен играть с компьютером

        delegate void DelegateRestart();
        delegate void DelegateDuo();
        static void Main(string[] args)
        {
            byte i = 0;
            WriteLine("Добро пожаловать в игру!\nВведите номер пункта для выбора");
            do
            {          
                WriteLine("1. Одиночная игра\n2. Многопользовательская игра\n3. Выход из игры");

                i = byte.Parse(ReadLine());

                for (byte j = 0; j < 30; j++)
                {
                    Write("=");
                }
                WriteLine();

                switch (i)
                {
                    case 1:
                        VsBotGame();
                        break;
                    case 2:
                        DuoGame();
                        break;
                    default:
                        WriteLine("Ошибка выбора");
                        break;
                }              
            } while (i != 3);
        }

        private static void DuoGame()
        {
            DelegateRestart funcdel = new DelegateRestart(DuoGame);

            string userNameFirst;
            string userNameSecond;
            WriteLine("Игрок 1, введите свое имя");
            userNameFirst = ReadLine();
            WriteLine("Игрок 2, введите свое имя");
            userNameSecond = ReadLine();

            var rnd = new Random();
            int gameNumber = rnd.Next(12, 121);
            WriteLine($"Исходное число: {gameNumber}");

            do
            {
                gameNumber = PlayerTurn(gameNumber, userNameFirst);
                if (gameNumber <= 0)
                {
                    WriteLine($"Игрок {userNameFirst} победил!");
                }
                gameNumber = PlayerTurn(gameNumber, userNameSecond);
                if (gameNumber <= 0)
                {
                    WriteLine($"Игрок {userNameSecond} победил!");
                }

            } while (gameNumber >= 1);

            RestartGame(DuoGame);
        }

        private static void VsBotGame()
        {
            DelegateRestart funcdel = new DelegateRestart(VsBotGame);

            WriteLine("Выберите уровень сложности\n1. Легко\n2. Средне\n3. Сложно");
            byte difficulty = byte.Parse(ReadLine());

            string userName;
            WriteLine("Игрок, введите свое имя");
            userName = ReadLine();

            var rnd = new Random();
            int gameNumber = rnd.Next(12, 121);
            WriteLine($"Исходное число: {gameNumber}");

            do
            {
                gameNumber = PlayerTurn(gameNumber, userName);
                if (gameNumber <= 0)
                {
                    WriteLine($"Игрок {userName} победил!");
                }
                gameNumber = BotTurn(gameNumber, difficulty);
                if (gameNumber <= 0)
                {
                    WriteLine("Компьютер победил!");
                }

            } while (gameNumber >= 1);

            RestartGame(VsBotGame);
        }

        private static int PlayerTurn(int gameNumber, string user)
        {
            byte userTry = 0;
            WriteLine($"{user} введите число от 1 до 4");
            do
            {
                try
                {
                    userTry = byte.Parse(ReadLine());
                    if ((userTry < 1) || (userTry > 4))
                    {
                        WriteLine("Ошибка ввода! Введите целое число от 1 до 4.");
                    }
                    if (userTry > gameNumber)
                    {
                        WriteLine("Ошибка ввода! Введенное число больше игрового");
                    }
                }
                catch
                {
                    WriteLine("Ошибка ввода! Введите целое число от 1 до 4.");
                }
            }
            while (((userTry < 1) || (userTry > 4)) || (userTry > gameNumber));

            gameNumber -= userTry;
            WriteLine($"Текущее игровое число: {gameNumber}");
            return gameNumber;
        }

        private static int BotTurn(int gameNumber, int difficulty)
        {
            int botTry = botAlgarithm(gameNumber, difficulty);

            WriteLine($"Бот выбирает число {botTry}");
            gameNumber -= botTry;
            WriteLine($"Текущее игровое число: {gameNumber}");
            return gameNumber;
        }

        private static int botAlgarithm(int gameNumber, int difficulty)
        {
            int botTry = 0;

            //алгоритм выбора ботом числа
            //для победы бота ему необходимо выбирать числа так,
            //чтобы игроку доставались числа кратные 5.
            //чтобы бот не выигрывал постоянно предусмотрена рандомность ходов,
            //зависящая от выбранного уровня сложности
            //Легко - полностью рандомные ходы бота, кроме [1,9]
            //Средне - шанс "правильного" хода бота 50%
            //Тяжело - шанс "правильного хода бота 75% 

            if (difficulty == 3)
            {
                difficulty += 1;
            }

            if (gameNumber > 9)
            {
                int rndDiff;
                var rnd = new Random();
                rndDiff = rnd.Next(1, difficulty + 1);

                if (rndDiff == 1)
                {
                    var rndTry = new Random();
                    botTry = rndTry.Next(1, 5); 
                }
                else
                {
                    botTry = BotNumberCheck(gameNumber);
                }
            }
            else
            {
                botTry = BotNumberCheck(gameNumber);
            }

            return botTry;
        }

        private static int BotNumberCheck(int gameNumber)
        {
            int botTry = 0;

            if (gameNumber % 5 == 0)
            {
                var rnd = new Random();
                botTry = rnd.Next(1, 5);
            }
            else if (gameNumber % 5 == 1)
            {
                botTry = 1;
            }
            else if (gameNumber % 5 == 2)
            {
                botTry = 2;
            }
            else if (gameNumber % 5 == 3)
            {
                botTry = 3;
            }
            else if (gameNumber % 5 == 4)
            {
                botTry = 4;
            }

            return botTry;
        }

        private static void RestartGame(DelegateRestart delFunc)
        {
            for (byte j = 0; j < 30; j++)
            {
                Write("=");
            }
            WriteLine();
            WriteLine("Хотите реванш?\n1. Да\n2. Нет");
            byte i = byte.Parse(ReadLine());
            switch (i)
            {
                case 1:
                    for (byte j = 0; j < 30; j++)
                    {
                        Write("=");
                    }
                    WriteLine();
                    delFunc.Invoke();
                    break;
                case 2:
                    for (byte j = 0; j < 30; j++)
                    {
                        Write("=");
                    }
                    WriteLine();
                    break;
                default:
                    WriteLine("Ошибка выбора");
                    break;
            }
        }
    }
}





