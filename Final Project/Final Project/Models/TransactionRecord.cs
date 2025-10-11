namespace Final_Project.Models;

 public class TransactionRecord
{
    public DateTime transactionDate { get; set; }     
    public string transactionType { get; set; }      
    public double amountGEL { get; set; }            
    public double amountUSD { get; set; }          
    public double amountEUR { get; set; }            
}