namespace Week_10;

class Program
{
    static void Main(string[] args)
    {
        ImageFileWorker imageWorker = new ImageFileWorker { MaxFileSize = 50 };
        imageWorker.Read();
        imageWorker.Write();
        imageWorker.Edit();
        imageWorker.Delete();
        
        Bank bank = new Bank();
        if (bank.CheckUserHistory())
        {
            Console.WriteLine($"Bank loan total: {bank.CalculateLoanPercent(12, 200)}$");
        }
        else
        {
            Console.WriteLine("Bank: user history not approved!");
        }

        MicroFinance micro = new MicroFinance();
        if (micro.CheckUserHistory())
        {
            Console.WriteLine($"MicroFinance loan total: {micro.CalculateLoanPercent(12, 200)}$");
        }
    }
    
    
    abstract class  FileWorker
    {
      
        public int MaxFileSize { get; set; }
        public abstract string FileExtension { get;  }

        public virtual void Read()
        {
            Console.WriteLine($"I can read {FileExtension} files with max storage {MaxFileSize} MB");
        }

        public virtual void Write()
        {
            Console.WriteLine($"I can write {FileExtension} files with max storage {MaxFileSize} MB");
        }

        public virtual void Delete()
        {
            Console.WriteLine($"I can delete {FileExtension} files with max storage {MaxFileSize} MB");
        }

        public virtual void Edit()
        {
            Console.WriteLine($"I can edit {FileExtension} files with max storage {MaxFileSize} MB");
        }
        
        
    }
    
    
    
    class ImageFileWorker : FileWorker
    {
        public override string FileExtension => ".jpg";

        public override void Read()
        {
            Console.WriteLine($"Opening an image up to {MaxFileSize} MB...");
        }

        public override void Write()
        {
            Console.WriteLine($"Saving a {FileExtension} image (limit {MaxFileSize} MB).");
        }

        public override void Edit()
        {
            Console.WriteLine($"Editing image   (max {MaxFileSize} MB).");
        }

        public override void Delete()
        {
            Console.WriteLine($"Deleting an image file up to {MaxFileSize} MB.");
        }
    }
    
    
    interface IFinanceOperations
    {
       double  CalculateLoanPercent(int month, double AmountPerMonth);
       bool  CheckUserHistory();
    }
    
    class Bank:IFinanceOperations
    {
        public bool CheckUserHistory()
        {
            var random = new Random();
            return random.Next(0, 2) == 1;
        }

        public double CalculateLoanPercent(int month, double AmountPerMonth)
        {
            double total = month * AmountPerMonth;
            return total * 1.05;  
        }
    }

    class MicroFinance:IFinanceOperations
    {
        public bool CheckUserHistory()
        {
            return true;
        }

        public double CalculateLoanPercent(int month, double AmountPerMonth)
        {
            double total = month * AmountPerMonth;    
            double commission = total * 0.10;         
            double serviceFee = month * 4;            
            double fullPayment = total + commission + serviceFee; 
            return fullPayment;   
        }
    }
    
    
}