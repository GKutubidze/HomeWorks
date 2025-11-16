namespace Week_19.Domain;

public class Person
{
    public int Id { get; set; }
    public DateTime CreateDate { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string JobPosition { get; set; }
    public double Salary { get; set; }
    public double WorkExperience { get; set; }
    public Adress Adress { get; set; }

}