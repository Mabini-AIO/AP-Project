
using System;

namespace Main_Proj_rev_2
{
    public enum ReservationStatus { Pending, Fulfilled, Cancelled }

    public class Reservation
    {
        public User ReservingUser { get; private set; }
        public Equipment ReservedEquipment { get; private set; }
        public DateTime RequestDate { get; private set; }
        public DateTime RequiredDate { get; private set; }
        public int Priority { get; private set; }
        public ReservationStatus Status { get; private set; }

        public Reservation(User user, Equipment equipment, DateTime requestDate, DateTime requiredDate, int priority = 1)
        {
            ReservingUser = user;
            ReservedEquipment = equipment;
            RequestDate = requestDate;
            RequiredDate = requiredDate;
            Priority = priority;
            Status = ReservationStatus.Pending;
        }

        public void Fulfill()
        {
            if (Status == ReservationStatus.Pending)
                Status = ReservationStatus.Fulfilled;
        }
    }
}