using BankConsole;
/* Client max  = new Client(1, "Max", "maxzermeno03@gmail.com", 4000, 'M');


Console.WriteLine(max.ShowData());

Employee jose = new Employee(3, "Jose", "jose01@hotmail.com", 4000, "IT");


Console.WriteLine(jose.ShowData());

Storage.AddUser(max);
Storage.AddUser(jose); 
 */

if (args.Length == 0)
    EmailService.SendMail();
else
    ShowMenu();


void ShowMenu()
{
    Console.Clear();
    Console.WriteLine("Selecciona una opción:");
    Console.WriteLine("1- Crear un usuario nuevo.");
    Console.WriteLine("2- Eliminar un usuario existente.");
    Console.WriteLine("3- Salir.");

    int option = 0;

    do
    {
        string input = Console.ReadLine();
        if (!int.TryParse(input, out option))
            Console.WriteLine("Debes ingresar un número (1, 2 o 3).");
        else if (option>3)
            Console.WriteLine("Debes ingresar un número válido (1, 2 o 3).");    

    }while(option == 0 || option > 3);

    switch(option)
    {
        case 1:
            CreateUser();
            break;
        case 2:
            DeleteUser();
            break;
        case 3:
            Environment.Exit(0);
            break;
    }

}

void CreateUser()
{
    Console.Clear();
    Console.WriteLine("Ingresa la información del usuario:");
    
    Console.Write("ID: ");
    int ID = -1;
    do
    {
        string input = Console.ReadLine();
        if (!int.TryParse(input, out ID))
            Console.WriteLine("Debes ingresar un número.");
        else if (ID<=0)
            Console.WriteLine("Debes ingresar un número entero positivo mayor a 0.");    
        else if(Storage.CheckUserInList(ID))
        {
            Console.WriteLine("Ese ID ya esta siendo utilizado, eliga otro porfavor.");
        }
    }while(ID <= 0 || Storage.CheckUserInList(ID));

    Console.Write("Nombre: ");
    string name = Console.ReadLine();

    Console.Write("Email: ");
    string email;
    do
    {
        email = Console.ReadLine();
        if (!Storage.IsEmailValid(email))
            Console.WriteLine("Debes ingresar un mail correcto.");  

    }while(!Storage.IsEmailValid(email));

    Console.Write("Saldo: ");
    decimal balance = -1;
    do
    {
        string input = Console.ReadLine();
        if (!decimal.TryParse(input, out balance))
            Console.WriteLine("Debes ingresar un número.");
        else if (balance<0)
            Console.WriteLine("Debes ingresar un número positivo mayor a 0.");    

    }while(balance < 0);
   

    Console.Write("Escribe 'c' si el usuario es un Cliente, 'e' si el usuario es un Empleado: ");
    char userType;
    do
    {
        string input = Console.ReadLine();
        if (!char.TryParse(input, out userType))
            Console.WriteLine("Debes ingresar un caracter.");
        else if (!userType.Equals('c') && !userType.Equals('e'))
            Console.WriteLine("Debes ingresar un caracter válido ('c' para cliente y 'e' para empleado).");    

    }while(!userType.Equals('c') && !userType.Equals('e'));


    User newUser;

    if (userType.Equals('c'))
    {
        Console.WriteLine("Regimen Fiscal: ");
        char taxRegime = char.Parse(Console.ReadLine());

        newUser = new Client(ID, name, email, balance, taxRegime);
    }
    else
    {
        Console.WriteLine("Departamento: ");
        string department = Console.ReadLine();

        newUser = new Employee(ID, name, email, balance, department);
    }

    Storage.AddUser(newUser);

    Console.WriteLine("Usuario creado.");
    Thread.Sleep(2000);
    ShowMenu();
}

void DeleteUser()
{
    Console.Clear();

    Console.WriteLine("Ingresa el ID del usuario que desea eliminar: ");
    int ID = -1;
    do
    {
        string input = Console.ReadLine();
        if (!int.TryParse(input, out ID))
            Console.WriteLine("Debes ingresar un número.");
        else if (ID<=0)
            Console.WriteLine("Debes ingresar un número entero positivo mayor a 0.");    
        else if(!Storage.CheckUserInList(ID))
        {
            Console.WriteLine("Ese ID no existe, eliga otro porfavor.");
        }
    }while(ID <= 0 || !Storage.CheckUserInList(ID));

    string result = Storage.DeleteUser(ID);

    if (result.Equals("Success"))
    {
        Console.WriteLine("Usuario Eliminado.");
        Thread.Sleep(2000);
        ShowMenu();
    }
    else if(result.Equals("There are no users in the file."))
    {
        Console.WriteLine("No existen usuarios en el registro.");
        Thread.Sleep(2000);
        ShowMenu();
    } 
    else if (result.Equals("N/A"))
    {
        Console.WriteLine("Usuario No Encontrado.");
        Thread.Sleep(2000);
        ShowMenu();
    }
}
