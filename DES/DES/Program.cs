﻿using System;
//разработать неизменяемый массив

//нужно поместить файл с именем метода, содержащий таблицу метода в директорию с приложением и запустить
//в файле может содержаться таблица любого другого s-стадийного ЯМРК вместе с числом стадий и порядка
//при желании можно изменить условия системы уравнений, но система должна быть первого порядка,
//все начальные условия должны быть заданы

namespace DES
{
    class Program
    {
        static void Main()
        {
            //_2_18.Solve("Felberg.txt", 4, 0.2, 0.5);
            //_2_19.Solve("Felberg.txt", 4, -2, -1);
            _2_2.Solve(0.1, 4, "Felberg.txt", "DormanPrince.txt");
            //_2_4.Solve(4, "DormanPrince.txt");
            Console.ReadKey();
        }
    }
}
