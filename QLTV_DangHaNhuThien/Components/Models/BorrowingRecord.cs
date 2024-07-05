namespace QLTV_DangHaNhuThien.Components.Models
{
    public class BorrowingRecord
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? BookId { get; set; }
        public DateTime BorrowedDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnedDate { get; set; }
        public string? Status { get; set; }


        //connect to user and book table
        public User User { get; set; }
        public Book Book { get; set; }

    
    }
 

}
