using System;
using System.Runtime.Serialization;
namespace Personal_Bugeting
{
    [DataContract]
    public class UserDTO
    {
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]

        public string PasswordHash { get; set; }
    }
    [Serializable]
    public class IncomeDTO
    {
        public int Id { get; set; }
        public decimal amount { get; set; }
        public DateTime date { get; set; }
        public bool isRecurring { get; set; }
        public string Source { get; set; }
        public UserDTO User { get; set; }
    }
    [Serializable]
    public class ExpenseDTO
    {
        public int Id { get; set; }
        public string category { get; set; }
        public decimal amount { get; set; }
        public DateTime date { get; set; }
        public bool isRecurring { get; set; }

        public UserDTO User { get; set; }


    }
    [Serializable]
    public class BudgetDTO
    {
        public int Id { get; set; }
        public string category { get; set; }
        public decimal spent { get; set; }
        public decimal limit { get; set; }
        public DateTime periodStart { get; set; }
        public DateTime periodEnd { get; set; }
        public UserDTO User { get; set; }
    }
    [Serializable]
    public class ReminderDTO
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
        public UserDTO User { get; set; }

    }

}