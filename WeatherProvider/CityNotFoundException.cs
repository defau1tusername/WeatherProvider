using System;

public class CityNotFoundException : Exception
{
    public CityNotFoundException() 
        : base("Ошибка: город не найден") 
    { }
}

