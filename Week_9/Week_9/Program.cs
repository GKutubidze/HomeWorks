namespace Week_9;

class Program
{
    static void Main(string[] args)
    {
        
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        // === Company Example ===
        // Company localCompany = new Company(true);
        // Console.WriteLine( localCompany.CalculateSalarY(2000));
        //
        // Company foreignCompany = new Company(false);
        // Console.WriteLine( foreignCompany.CalculateSalarY(2000));


        // === Employee Example ===
        Employee employee = new Employee("John", "Doe", 35, "manager", new int[] { 9, 8, 7, 10, 8, 6, 5 });
        Console.WriteLine(employee.CalculateWeeklySalary());


        // === Student Example ===
        // Student student = new Student("student1", 20, 2023);
        // Console.WriteLine( student.GetRandomSubject());
        // Console.WriteLine( student.YearsLeft()  );
        //
        //
        // // === Teacher Example ===
        // Teacher teacher = new Teacher("teacher1", true);
        // teacher.PrintInfo("ქიმია");
        // teacher.PrintInfo("მათემატიკა");
        // teacher.PrintInfo("ინგლისური");
        // teacher.PrintInfo("ისტორია");
        //
        //
        // // === Student2 / GoodStudent / LazyStudent Example ===
        // GoodStudent good = new GoodStudent("goodStudent");
        // LazyStudent lazy = new LazyStudent("lazyStudent");
        //
        // Console.WriteLine("\n--- Good Student ---");
        // good.Study();
        // good.Read();
        // good.Write();
        // good.Relax();
        //
        // Console.WriteLine("\n--- Lazy Student ---");
        // lazy.Study();
        // lazy.Read();
        // lazy.Write();
        // lazy.Relax();
        //
        //
        // // === Classroom Example ===
        // Student2[] group = { good, lazy, new GoodStudent("Nino"), new LazyStudent("Saba") };
        // ClassRoom classRoom = new ClassRoom(group);
        //
        //
        // classRoom.PrintStudents();
    }



    class Company
    {
        public bool IsLocalCompany { get; }

        public Company(bool isLocalCompany)
        {
            this.IsLocalCompany = isLocalCompany;
        }


        public double CalculateSalarY(double totalSalary)
        {
            
            return IsLocalCompany ? totalSalary * 0.18 : totalSalary *0.05;
        }
    }


    class Employee
    {
        public string firstName {get; set;}
        public string lastName {get; set;}
        public int age {get; set;}
        public string position {get; }
        public int[] weekHours { get;  }

        public Employee(string firstName, string lastName, int age, string position, int[] weekHours)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.age = age;
            this.position = position;
            this.weekHours = weekHours;
        }
        public double CalculateWeeklySalary()
        {
            double sum = 0;
            double baseRate;

            switch (position.ToLower())
            {
                case "manager":
                    baseRate = 40;
                    break;
                case "developer":
                    baseRate = 30;
                    break;
                case "tester":
                    baseRate = 20;
                    break;
                default:
                    baseRate = 10;
                    break;
            }

             for (int i = 0; i < 5; i++)
            {
                if (weekHours[i] <= 8)
                {
                    sum += weekHours[i] * baseRate;
                }
                else
                {
                    sum += 8 * baseRate + (weekHours[i] - 8) * (baseRate + 5);
                }
            }

            sum += weekHours[5] * baseRate * 2;
            sum += weekHours[6] * baseRate * 2;

            int totalHours = weekHours.Sum();
            if (totalHours > 50)
            {
                sum *= 1.2;
            }

            return sum;
        }

        }

    class Student
    {
        private string Name { get; set; }
        private int Age { get; set; }
        private int EnrollmentYear { get;  }


        public Student(string name, int age, int enrollmentYear)
        {
            Name = name;
            Age = age;
            EnrollmentYear = enrollmentYear;
        }

        public string GetRandomSubject()
        {
            string[] subjects = new string[]{"მათემატიკა","ქიმია","ფიზიკა","ინგლისური"};
            var random = new Random();
            return subjects[random.Next(0, subjects.Length)];

        }

        public int YearsLeft()
        {
            var date = DateTime.Now.Year - EnrollmentYear;
            int yearsLeft = 4 - date;
            return yearsLeft > 0 ? yearsLeft : 0;
        }
    }

    class Teacher
    {
        public string Name { get; set; }
        public bool IsCertified { get; set; }

        public Teacher(string name, bool isCertified)
        {
            Name = name;
            IsCertified = isCertified;
        }

        public void PrintInfo(string subject){

            if (subject.ToLower() == "მათემატიკა")
            {
                var rand = new Random();
                
                Console.WriteLine(rand.NextInt64()+rand.NextInt64());
            }
            else if (subject.ToLower() == "ქიმია")
            {
                Console.WriteLine("H2SO4");
            }
            else if (subject == "ინგლისური")
            {
                Console.WriteLine("Hello");
            }
            else
            {
                Console.WriteLine($"ამ საგანში {subject} მასწავლებელი არ არის კომპეტენტური.");
            }
        }
    }
    
    class Student2
    {
        protected string Name { get; set; }

        public Student2(string name)
        {
            Name = name;
        }

        public virtual void Study()
        {
            Console.WriteLine("Base Study");
        }

        public virtual void Read()
        {
            Console.WriteLine("Base Read");
        }

        public virtual void Write()
        {
            Console.WriteLine("Base Write");
        }

        public virtual void Relax()
        {
            Console.WriteLine("Base Relax");
        }
    }

    class GoodStudent : Student2
    {
        public GoodStudent(string Name) : base(Name){}

        public override void Study()
        {
            Console.WriteLine("Good - Study");

        }
        public override void Read()
        {
            Console.WriteLine("Good - Read");
        }

        public override void Write()
        {
            Console.WriteLine("Good - Write");
        }

        public override void Relax()
        {
            Console.WriteLine("Good - Relax");
        }
        
    }
    class LazyStudent : Student2
    {
        public LazyStudent(string name) : base(name) { }

        public override void Study()
        {
            Console.WriteLine("Lazy - Study");
        }

        public override void Read()
        {
            Console.WriteLine("Lazy - Read");
        }

        public override void Write()
        {
            Console.WriteLine("Lazy - Write");
        }

        public override void Relax()
        {
            Console.WriteLine("Lazy - Relax");
        }
    }

    class ClassRoom
    {
        private  List<Student2> students;

        public ClassRoom(Student2[] students)
        {
             this.students = new List<Student2>(students);
        }

        public void PrintStudents()
        { 
            foreach (var student in students)
            { 
                student.Study(); 
                student.Read();
                student.Write();
                student.Relax();
             }
        }
    }
    
    }







 
 
 