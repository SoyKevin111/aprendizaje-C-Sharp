using Humanizer;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Ingresa nombre: ");
var nombre = Console.ReadLine();

Console.WriteLine("Ingresa cargo: ");
var cargo = Console.ReadLine();

Console.WriteLine("Ingresa edad: ");
var edad = int.Parse(Console.ReadLine());



Console.WriteLine($"Hola, mi nombre es: {nombre}, mi cargo es: {cargo} y tengo {edad.ToWords(new System.Globalization.CultureInfo("es"))} years.");

