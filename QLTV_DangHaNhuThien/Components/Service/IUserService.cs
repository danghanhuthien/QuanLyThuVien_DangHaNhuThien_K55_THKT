using QLTV_DangHaNhuThien.Components.Models;

namespace QLTV_DangHaNhuThien.Components.Service
{
    public interface IUserService
    {

        //book funtion
        //get all book
        Task<List<Book>> GetBookListAsync(int? id);
        Task AddNewBookAsync(Book books);
        Task<Book> GetBookByIdAsync(int id);
        Task UpdateNewBookAsync(Book book, int id);
        Task DeleteNewBookAsync(int id);

        //get Category
        Task<List<Categorie>> GetCategoryListAsync();


        //user funtion
        Task<List<User>> GetUserListAsync();
        Task AddNewUserAsync(User user);
        Task<User> GetUserByIdAsync(int id);
        Task UpdateNewUserAsync(User users, int id);
        Task DeleteNewUserAsync(int id);
        Task<List<Book>> GetBookListAvailableAsync();
        Task AddBookLoanAsync(BorrowingRecord borrowingRecord, String Username,int BookId);


        //BorrowingRecords function
        Task<List<BorrowingRecord>> GetLoanBookListAsync(String Username);
        Task UpdateReturnedDateAsync(BorrowingRecord book, int id);
        Task UpdateRequestDateAsync(BorrowingRecord Book, int id);

        // get đang mượn
        Task AddBookRequestLoanAsync(BorrowingRecord BorrowingRecord, String Username, int BookId);
        Task<List<BorrowingRecord>> GetLoanBookRequestListAsync(String Username);
        Task<List<BorrowingRecord>> GetLoanBookReturnListAsync(String Username);
        Task<User?> GetUserByEmail(string email);
        Task DeleteBorrowingRecordAsync(int userId, int bookId);

    }
}
