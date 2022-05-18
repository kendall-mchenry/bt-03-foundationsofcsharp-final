using System.Linq;
using System.Globalization;

List<Employee> employeeList = new List<Employee>();

// READ FILE & POPULATE EMPLOYEE LIST ------------------------------------------------
// Should this go somewhere else, like in the program? OR is it good to stay at the beginning so the list is refreshed when the program runs and only runs once?
void convertFileToList()
{
    string[] employeesFile = File.ReadAllLines("employees.txt");

    foreach (string employeeInput in employeesFile)
    {
        employeeList.Add(convertStringToEmployee(employeeInput));
    }
}

convertFileToList();

// EMPLOYEE TO STRING (for file) -------------------------------------------------------
string convertEmployeeToString(Employee contact)
{
    string[] employeeArray = { $"{contact.ID}, {contact.FullName}, {contact.Title}, {contact.StartDate.ToString("MM/dd/yyyy")}" };
    string employeeInfo = string.Join(", ", employeeArray);

    return employeeInfo;
}

// STRING TO EMPLOYEE (for use in list) ------------------------------------------------
Employee convertStringToEmployee(string input)
{
    string[] splitEmployee = input.Split(", ");

    Employee newInput = new Employee(int.Parse(splitEmployee[0]), splitEmployee[1], splitEmployee[2], DateTime.Parse(splitEmployee[3]));

    return newInput;
}

// CHECKING FOR NULL STRING INPUT (to reuse throughout)-------------------------------
string getNotNullInput(string input, string customPrompt)
{
    while (string.IsNullOrEmpty(input))
    {
        Console.Write($"\nPlease enter a valid input ({customPrompt}): ");
        input = Console.ReadLine()!;
    }

    return input;
}

// ASKING FOR VALID INPUT-------------------------------------------------------------
string getValidInput(string input, string customPrompt)
{
    Console.Write($"\nPlease enter a valid input ({customPrompt}): ");
    input = Console.ReadLine()!;

    return input;
}


// DISPLAY MENU OPTIONS --------------------------------------------------------------
void displayOptions()
{
    Console.WriteLine("\n************************");
    Console.WriteLine("1. Create New Employee \n2. View All Employees \n3. Search Employees \n4. Delete Employee \n5. Quit");
    Console.WriteLine("************************\n");
    Console.Write("Enter your choice: ");
}

// CREATE NEW EMPLOYEE --------------------------------------------------------------
void createEmployee()
{
    Console.WriteLine("\n--------------------");
    Console.WriteLine("Create New Employee");
    Console.WriteLine("--------------------\n");

    Employee newEmployee = new Employee();

    string getInfo(string prompt0, string prompt1)
    {
        Console.Write($"{prompt0} ");
        string? input = Console.ReadLine();

        return getNotNullInput(input, prompt1);
    }

    string[] fullNamePrompt = { "What is the employee's full name?", "Full name" };
    newEmployee.FullName = getInfo(fullNamePrompt[0], fullNamePrompt[1]);

    string[] titlePrompt = { "What is the employee's title?", "Title" };
    newEmployee.Title = getInfo(titlePrompt[0], titlePrompt[1]);

    string[] startDatePrompt = { "What is the employees start date?", "format MM/DD/YYYY" };
    string? startDateInput = getInfo(startDatePrompt[0], startDatePrompt[1]);

    // Catches if a something other than a valid date is entered (including dates that don't exist, i.e. a day beyond the end of the month)
    while (!DateTime.TryParse(startDateInput, out DateTime result))
    {
        startDateInput = getValidInput(startDateInput, "format MM/DD/YYYY");
    }

    newEmployee.StartDate = Convert.ToDateTime(startDateInput);

    // OR is this better? (it also works): newEmployee.StartDate = DateTime.Parse(startDateInput); 

    newEmployee.ID = new Random().Next(1000, 10000);
    // this will restrict return values to 4-digits (1000 - 9999)

    employeeList.Add(newEmployee);

    Console.WriteLine($"\nNew Employee Added - Employee ID: {newEmployee.ID}\n");

    // File handling -- adding each new contact to the contact.txt file
    using (StreamWriter file = new StreamWriter("employees.txt", true))
    {
        file.WriteLine(convertEmployeeToString(newEmployee));
    }
}

// VIEW EMPLOYEE LIST IN CONSOLE -------------------------------------------------------
void viewEmployees()
{
    Console.WriteLine("\n------------------");
    Console.WriteLine("View All Employees");
    Console.WriteLine("------------------\n");

    if (employeeList.Count > 0)
    {
        foreach (Employee e in employeeList)
        {
            e.printEmployeeDetails();
        }
    }
    else
    {
        Console.WriteLine("The employee list is empty.");
    }
}

// CHOOSE SEARCH OPTION --------------------------------------------------------------
void searchEmployees()
{
    Console.WriteLine("\n--------------------");
    Console.WriteLine("Search Employees by:\n1. Employee ID \n2. Name");
    Console.WriteLine("--------------------\n");

    if (employeeList.Count > 0)
    {
        Console.Write("Enter your choice: ");

        string? searchInput = Console.ReadLine();

        bool continueSearch = true;
        while (continueSearch)
        {
            try
            {
                int searchChoice = int.Parse(searchInput);

                if (searchChoice == 1)
                {
                    searchById();
                    continueSearch = false;

                }
                else if (searchChoice == 2)
                {
                    searchByName();
                    continueSearch = false;
                }
                else
                {
                    searchInput = getValidInput(searchInput, "1 or 2");
                }
            }
            catch (System.FormatException)
            {
                searchInput = getValidInput(searchInput, "1 or 2");
            }
        }
    }
    else
    {
        Console.WriteLine("Unable to search. The employee list is empty.");
    }

}

// OPTION 1: SEARCH BY EMPLOYEE ID -----------------------------------------------------
void searchById()
{
    Console.Write("\nEnter the Employee ID: ");
    string? idInput = Console.ReadLine();

    bool invalidIdInput = true;
    while (invalidIdInput)
    {
        try
        {
            int newIdInput = int.Parse(idInput);

            if (newIdInput >= 1000 && newIdInput < 10000)
            {
                var foundID = employeeList.FirstOrDefault(e => e.ID == newIdInput);

                Console.WriteLine("\n-----------------");
                Console.WriteLine("Results:");
                Console.WriteLine("-----------------\n");

                if (foundID != null)
                {
                    foundID.printEmployeeDetails();
                }
                else
                {
                    Console.WriteLine("No employees found.");
                }

                invalidIdInput = false;
            }
            else
            {
                idInput = getValidInput(idInput, "4-digit number");
            }
        }
        catch (System.FormatException)
        {
            idInput = getValidInput(idInput, "4-digit number");
        }
    }
}

// OPTION 2: SEARCH BY EMPLOYEE NAME (partial & case insensitive -----------------------
void searchByName()
{
    Console.Write("\nEnter a Name: ");
    string? nameInput = Console.ReadLine();

    nameInput = getNotNullInput(nameInput, "Name or partial name");

    bool invalidNameInput = true;
    while (invalidNameInput)
    {
        try
        {
            var foundName = employeeList.Where(e => e.FullName.Contains(nameInput, StringComparison.OrdinalIgnoreCase) == true);

            Console.WriteLine("\n-----------------");
            Console.WriteLine("Results:");
            Console.WriteLine("-----------------\n");

            if (foundName.Any())
            {
                foreach (Employee e in foundName)
                {
                    e.printEmployeeDetails();
                }
            }
            else
            {
                Console.WriteLine("No employees found.");
            }

            invalidNameInput = false;
        }
        catch (System.FormatException)
        {
            nameInput = getValidInput(nameInput, "Name or partial name");
        }
    }
}

// DELETE EMPLOYEE --------------------------------------------------------------
void deleteEmployee()
{
    Console.WriteLine("\n--------------------");
    Console.WriteLine("Delete an Employee");
    Console.WriteLine("--------------------\n");

    if (employeeList.Count > 0)
    {
        Console.Write("Enter Employee ID to delete: ");
        string? idInput = Console.ReadLine();

        bool invalidIdInput = true;
        while (invalidIdInput)
        {
            try
            {
                int newIdInput = int.Parse(idInput);

                if (newIdInput >= 1000 && newIdInput < 10000)
                {
                    var foundID = employeeList.FirstOrDefault(e => e.ID == newIdInput);

                    if (foundID != null)
                    {
                        Console.WriteLine($"\nEmployee ID {foundID.ID} has been deleted.");
                        employeeList.Remove(foundID);
                        refreshEmployeeList();
                    }
                    else
                    {
                        Console.WriteLine("\nNo employees found.");
                    }

                    invalidIdInput = false;
                }
                else
                {
                    idInput = getValidInput(idInput, "4-digit number");
                }
            }
            catch (System.FormatException)
            {
                idInput = getValidInput(idInput, "4-digit number");
            }
        }
    }
    else
    {
        Console.WriteLine("The employee list is empty; there are no employees to delete.");
    }
}

// REFRESH CONTACT LIST FILE ----------------------------------------------------------
// When should I use this -- when the program exits OR everytime an employee is removed from the list? Theoretically, either would work. For now, I chose to put it when an employee is deleted.
void refreshEmployeeList()
{
    // this should clear the list so it can be refreshed
    File.WriteAllText("employees.txt", "");

    foreach (Employee e in employeeList)
    {
        using (StreamWriter file = new StreamWriter("employees.txt", true))
        {
            file.WriteLine(convertEmployeeToString(e));
        }
    }

    if (employeeList.Count > 0)
    {
        Console.WriteLine("\nThe contact list has been refreshed.");
    }
    else
    {
        Console.WriteLine("\nThe contact list has been refreshed and is now empty.");
    }
}


// RUN PROGRAM --------------------------------------------------------------
bool continueProgram = true;
while (continueProgram)
{
    displayOptions();

    try
    {
        int choice = int.Parse(Console.ReadLine()!);

        if (choice >= 1 && choice <= 5)
        {
            switch (choice)
            {
                case 1:
                    createEmployee();
                    break;

                case 2:
                    viewEmployees();
                    break;

                case 3:
                    searchEmployees();
                    break;

                case 4:
                    deleteEmployee();
                    break;

                default:
                    Console.WriteLine("\nEmployees Saved. Goodbye!\n");
                    continueProgram = false;
                    break;
            }
        }
        else
        {
            Console.WriteLine("\n!! -- Please enter a valid menu option. -- !!");
        }
    }
    catch (System.FormatException)
    {
        Console.WriteLine("\n!! -- Please enter a valid menu option. -- !!");
    }
}