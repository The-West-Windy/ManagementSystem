using BCrypt.Net;
var hash = BCrypt.Net.BCrypt.HashPassword("NewPass123!");
Console.WriteLine(hash);
