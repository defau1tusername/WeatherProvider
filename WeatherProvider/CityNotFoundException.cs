﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CityNotFoundException : Exception
{
    public CityNotFoundException() 
        : base("Ошибка: город не найден") 
    { }
}
