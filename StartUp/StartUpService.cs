using Home_Security.Encryption;
using Home_Security.Entities.Identity;
using Home_Security.Interfaces.Repositories;
using Home_Security.Interfaces.Services;
using Home_Security.Models.DTOs;
using Home_Security.Models.Enums;

namespace Home_Security.StartUp;
public class StartUpService : IStartUpService
{
    IPersonService _personService;
    IUserRepo _userRepo;
    IEncryptionService _encryptionService;
    public StartUpService(IUserRepo userRepo, IPersonService personService, IEncryptionService encryptionService)
    {
        _userRepo = userRepo;
        _personService = personService;
        _encryptionService = encryptionService;
    }
    public async Task InitateStartUp()
    {
        var userData = await _userRepo.GetAllUsers();
        try
        {
            if (userData?.FirstOrDefault(x => x.UserRole.Role == Role.Owner) == null)
            {
                var person = await _encryptionService.DecryptObjectAsync();
                if(person != null){
                    await _personService.CreatePerson(person);
                    _encryptionService.DeleteFile();
                }
                else
                {
                    var personStartUp = Menu();
                    if(personStartUp.Option == 1 && personStartUp.EncryptedKey != null)
                    {
                        var decryptedKey = _encryptionService.DecryptEncryptionKeyAsync(personStartUp.EncryptedKey);
                        var response = await _personService.CreatePerson(decryptedKey);
                        if(response.Status == true)
                            Console.WriteLine("\n✅ Person created successfully!");
                        else Console.WriteLine("\n Error unable to create Person!");
                    }
                    if(personStartUp.Option == 2 && personStartUp.Person != null)
                    {
                        await _encryptionService.EncryptFileAsync(personStartUp.Person);
                        var response = await _personService.CreatePerson(personStartUp.Person);
                        if(response.Status == true)
                            Console.WriteLine("\n✅ Person created successfully!");
                        else Console.WriteLine("\n Error unable to create Person!");
                    }
                }
            }
            else Console.WriteLine("\n✅ Users Exist!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"StartupInitializer error: {ex.Message}");
        }
    }
    public MenuResult Menu()
    {
        bool op = true;
        while (op)
        {
            Console.WriteLine("Home Security Initialization\n 1.Enter Encrypted User Key \n 2. Enter Person Details");
            var input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input) && input.Length == 1 && input.All(char.IsDigit))
            {
                switch (int.Parse(input))
                {
                    case 1:
                        Console.WriteLine();
                        Console.Write("Enter Encrypted User Key: ");
                        return new MenuResult {
                            Option = 1,
                            EncryptedKey = Console.ReadLine()
                        };
                    case 2:
                        return new MenuResult {
                        Option = 2,
                        Person = CreatePerson()
                    };
                }
                op = false;
            }
        }
        return new MenuResult();
    }
    public CreatePersonDto CreatePerson()
    {
        Console.WriteLine("Please provide the following details to create a new person:");
        return new CreatePersonDto
        {
            CreatePersonDetailsDto = new CreatePersonDetailsDto
            {
                FirstName = ReadRequiredString("First Name"),
                LastName = ReadRequiredString("Last Name"),
                DateOfBirth = ReadRequiredDate("Date of Birth (yyyy-MM-dd)"),
                Gender = ReadRequiredEnum<Gender>("Gender")
            },
            CreateUserDto = new CreateUserDto
            {
                UserName = ReadRequiredString("Username"),
                Password = ReadRequiredString("Password"),
                AuthorizationCode = ReadSixDigitCode("Authorization Code (6 digits)"),
                Role = Role.Owner
            },
            Relation = ReadRequiredString("Relation"),
        };
    }
    private string ReadSixDigitCode(string fieldName)
    {
        while (true)
        {
            Console.Write($"{fieldName}: ");
            var input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input) && input.Length == 6 && input.All(char.IsDigit)) return input;
            Console.WriteLine("❌ Authorization code must be exactly 6 digits (0-9). Try again.");
        }
    }
    private string ReadRequiredString(string fieldName)
    {
        while (true)
        {
            Console.Write($"{fieldName}: ");
            var input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input))return input;
            Console.WriteLine("❌ Value cannot be empty. Try again.");
        }
    }
    private DateOnly ReadRequiredDate(string fieldName)
    {
        while (true)
        {
            Console.Write($"{fieldName}: ");
            var input = Console.ReadLine();
            if (DateOnly.TryParse(input, out var date)) return date;
            Console.WriteLine("❌ Invalid date format. Use yyyy-MM-dd.");
        }
    }
    private T ReadRequiredEnum<T>(string fieldName) where T : struct, Enum
    {
        while (true)
        {
            Console.WriteLine($"{fieldName}:");
            foreach (var value in Enum.GetValues<T>()) Console.WriteLine($"{Convert.ToInt32(value)} - {value}");
            Console.Write("Select option: ");
            var input = Console.ReadLine();
            if (int.TryParse(input, out int intValue) && Enum.IsDefined(typeof(T), intValue)) return (T)Enum.ToObject(typeof(T), intValue);
            Console.WriteLine("❌ Invalid selection. Try again.");
        }
    }
}
public class MenuResult
{
    public int Option { get; set; }

    public string? EncryptedKey { get; set; }

    public CreatePersonDto? Person { get; set; }
}