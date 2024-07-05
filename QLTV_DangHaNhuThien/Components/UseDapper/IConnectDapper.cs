using QLTV_DangHaNhuThien.Components.Models;

namespace QLTV_DangHaNhuThien.Components.UseDapper
{
    public interface IConnectDapper
    {
        Task<List<BorrowingRecord>> GetLoanBookListAsync(string username);
        Task UpdateReturnedDateAsync(BorrowingRecord book, int id);

		Task AddBookLoanAsync(BorrowingRecord borrowingRecord, string username, int bookId);
        Task<List<Book>> GetBookListAvailableAsync();
        Task<IEnumerable<BorrowingRecord>> GetAllUsersAsync();
        Task<User> GetUserByUsernameAndPassword(string username, string password);
        Task AddNewUserAsync(User users);
        Task<List<User>> GetUserListAsync();
        Task UpdateNewUserAsync(User users, int id);

        Task UpdateNewUserOfuser(User users, int id);

        Task<User> GetUserByIdAsync(int id);
        Task DeleteNewUserAsync(int id);
        Task DeleteNewBookAsync(int id);
        Task<List<Book>> GetBookListAsync(int? id);
        Task UpdateNewBookAsync(Book book, int id);
        Task<List<Categorie>> GetCategoryListAsync();
        Task<Book> GetBookByIdAsync(int id);
        Task AddNewBookAsync(Book book);
        Task<IEnumerable<Book>> SearchBooksAsync(string keyword);
        Task<User?> GetUserByEmail(string email);
        Task UpdateUserPassword(User user);
        Task<IEnumerable<User>> SearchUsersAsync(string keyword);
    }
}