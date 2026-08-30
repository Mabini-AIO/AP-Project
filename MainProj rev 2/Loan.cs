using Main_Proj_rev_2;
using System;

namespace Main_Proj_rev_2
{
    public enum LoanStatus { Active, Returned, Overdue }

    public class Loan
    {
        public User Borrower { get; private set; }
        public Equipment LoanedEquipment { get; private set; }
        public DateTime BorrowDate { get; private set; }
        public DateTime DueDate { get; private set; }
        public DateTime? ReturnDate { get; private set; }
        public LoanStatus Status { get; private set; }
        public double Fine { get; private set; }

        public Loan(User user, Equipment equipment, DateTime borrowDate)
        {
            Borrower = user;
            LoanedEquipment = equipment;
            BorrowDate = borrowDate;
            DueDate = borrowDate.AddDays(equipment.CalculateMaximumLoanDays());
            Status = LoanStatus.Active;
            Fine = 0;


        }

        public void ReturnEquipment(DateTime returnDate)
        {
            ReturnDate = returnDate;
            Status = LoanStatus.Returned;
            Borrower.ReturnEquipment();

            LoanedEquipment.MakeAvailable();

            Fine = CalculateFine(returnDate);
        }

        public double CalculateFine(DateTime currentDate)
        {
            if (currentDate <= DueDate) return 0;
            int overdueDays = (currentDate - DueDate).Days;
            return overdueDays * 30;
        }
    }
}