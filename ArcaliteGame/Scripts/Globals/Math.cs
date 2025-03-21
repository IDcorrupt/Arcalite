using Godot;
using System;

public partial class Math : Node
{
    public static bool RNG(int percentage)
    {
        //caps value
        if (percentage < 0)
            percentage = 0;
        else if (percentage > 100)
            percentage = 100;


        Random random = new Random();
        //generates random number between 0 and 100, then checks if percentage is above -> true / below -> false
        return random.Next(0, 101) < percentage;
    }
}
