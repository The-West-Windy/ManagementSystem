using BCrypt.Net;
var hash = BCrypt.Net.BCrypt.HashPassword("User123!");
Console.WriteLine(hash);
