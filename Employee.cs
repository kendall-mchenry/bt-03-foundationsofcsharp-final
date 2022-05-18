public class Employee
{
    // Properties
    protected string? fullName;
    protected int id;
    protected string? title;
    protected DateTime startDate;


    // Default Constructor
    public Employee()
    {
        fullName = "";
        id = 0;
        title = "";
        startDate = new DateTime();
    }

    // Parameterized constructor
    public Employee(int eId, string eFullName, string eTitle, DateTime eStartDate)
    {
        id = eId;
        fullName = eFullName;
        title = eTitle;
        startDate = eStartDate;
    }

    // Methods
    public void printEmployeeDetails()
    {
        Console.WriteLine($"{fullName}, {title}\nEmployee ID: {id}\nStart Date: {startDate.ToString("MM/dd/yyyy")}\n");
    }

    // Accessors
    public string? FullName
    {
        get { return fullName; }
        set { fullName = value; }
    }

    public string? Title
    {
        get { return title; }
        set { title = value; }
    }

    public int ID
    {
        get { return id; }
        set { id = value; }
    }

    public DateTime StartDate
    {
        get { return startDate; }
        set { startDate = value; }
    }

}