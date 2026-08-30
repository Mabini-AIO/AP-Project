using System;

namespace Main_Proj_rev_2
{
    public interface Display
    {
        void DisplayDetails();
    }

    public enum EquipmentStatus
    {
        Available,
        Borrowed,
        UnderMaintenance,
        Disabled
    }

    public abstract class Equipment : Display
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public EquipmentStatus Status { get; protected set; }
        public DateTime RegistrationDate { get; private set; }
        public int BorrowCount { get; protected set; }

        public Equipment(string id, string name)
        {


            if (id.StartsWith("EQ-1187"))
            {
                Id = id;
            }
            else
            {
                Id = "EQ-1187-" + id;
            }

            Name = name;
            Status = EquipmentStatus.Available;
            RegistrationDate = DateTime.Now;
            BorrowCount = 0;
        }

        public virtual void IncrementBorrowCount()
        {
            BorrowCount++;
            if (BorrowCount % 6 == 0)
            {
                Status = EquipmentStatus.UnderMaintenance;
            }
            else
            {
                Status = EquipmentStatus.Borrowed;
            }
        }

        public void MakeAvailable()
        {
            if (Status != EquipmentStatus.UnderMaintenance)
            {
                Status = EquipmentStatus.Available;
            }
        }

        public void FinishMaintenance()
        {
            if (Status == EquipmentStatus.UnderMaintenance)
            {
                Status = EquipmentStatus.Available;
            }
        }

        public abstract void DisplayDetails();
        public abstract int CalculateMaximumLoanDays();
    }

    public class Camera : Equipment
    {
        public string Resolution { get; set; }
        public Camera(string id, string name, string resolution) : base(id, name) { Resolution = resolution; }
        public override void DisplayDetails() => Console.WriteLine($"[Camera] ID: {Id} | Name: {Name} | Res: {Resolution} | Status: {Status} | Total Borrows: {BorrowCount}");
        public override int CalculateMaximumLoanDays() { return 12; }
    }

    public class Microphone : Equipment
    {
        public string PolarPattern { get; set; }
        public Microphone(string id, string name, string pattern) : base(id, name) { PolarPattern = pattern; }
        public override void DisplayDetails() => Console.WriteLine($"[Microphone] ID: {Id} | Name: {Name} | Pattern: {PolarPattern} | Status: {Status} | Total Borrows: {BorrowCount}");
        public override int CalculateMaximumLoanDays() { return 12; }
    }

    public class Tripod : Equipment
    {
        public double MaxHeight { get; set; }
        public Tripod(string id, string name, double height) : base(id, name) { MaxHeight = height; }
        public override void DisplayDetails() => Console.WriteLine($"[Tripod] ID: {Id} | Name: {Name} | Height: {MaxHeight}cm | Status: {Status} | Total Borrows: {BorrowCount}");
        public override int CalculateMaximumLoanDays() { return 12; }
    }
}