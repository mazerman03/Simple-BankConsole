using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BankConsole;

public static class Storage
{
    static string filepath = AppDomain.CurrentDomain.BaseDirectory + @"\users.json";

    public static void AddUser(User user)
    {
        string json = "", usersInFile = "";

        if(File.Exists(filepath))
            usersInFile = File.ReadAllText(filepath);

        var listUsers = JsonConvert.DeserializeObject<List<User>>(usersInFile);

        if (listUsers == null)
            listUsers = new List<User>();

    
        listUsers.Add(user);

        JsonSerializerSettings settings = new JsonSerializerSettings{Formatting = Formatting.Indented};

        json = JsonConvert.SerializeObject(listUsers, settings);

        File.WriteAllText(filepath, json);
    }

    public static List<User> GetNewUsers()
    {
        string usersInFile = "";
        var listUsers = new List<User>();

        if(File.Exists(filepath))
            usersInFile = File.ReadAllText(filepath);

        var listObjects = JsonConvert.DeserializeObject<List<object>>(usersInFile);

        if (listObjects == null)
            return listUsers;

        foreach (object obj in listObjects)
        {
            User newUser;
            JObject user = (JObject)obj;

            if (user.ContainsKey("TaxRegime"))
                newUser = user.ToObject<Client>();
            else   
                newUser = user.ToObject<Employee>();
            listUsers.Add(newUser);
        }

        var newUsersList = listUsers.Where(user => user.GetRegisterDate().Date.Equals(DateTime.Today)).ToList();
        
        return newUsersList;
    }

    public static string DeleteUser(int ID)
    {
        string usersInFile = "";
        var listUsers = new List<User>();

        if(File.Exists(filepath))
            usersInFile = File.ReadAllText(filepath);

        var listObjects = JsonConvert.DeserializeObject<List<object>>(usersInFile);

        if (listObjects == null)
            return "There are no users in the file.";

        foreach (object obj in listObjects)
        {
            User newUser;
            JObject user = (JObject)obj;

            if (user.ContainsKey("TaxRegime"))
                newUser = user.ToObject<Client>();
            else   
                newUser = user.ToObject<Employee>();
            listUsers.Add(newUser);
        }
       

        if(!CheckUserInList(ID))
            return "N/A";

        var userToDelete = listUsers.Where(user => user.GetID() == ID).Single();
        
        listUsers.Remove(userToDelete);

        JsonSerializerSettings settings = new JsonSerializerSettings { Formatting = Formatting.Indented}; 
        string json = JsonConvert.SerializeObject(listUsers, settings);

        File.WriteAllText(filepath, json);

        return "Success";
    }

    public static bool CheckUserInList(int IDtoCheck)
    {
        string usersInFile = "";

        if(File.Exists(filepath))
            usersInFile = File.ReadAllText(filepath);

        var listUsers = JsonConvert.DeserializeObject<List<User>>(usersInFile);

        if (listUsers == null)
            listUsers = new List<User>();
        
        if(listUsers.Any(user => user.GetID() ==  IDtoCheck))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool IsEmailValid(string email)
    {
        string emailPattern = @"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

        return Regex.IsMatch(email, emailPattern);
    }
}
