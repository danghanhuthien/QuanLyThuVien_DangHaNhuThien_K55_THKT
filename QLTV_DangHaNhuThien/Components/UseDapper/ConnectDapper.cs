using Dapper;
using System.Data.SqlClient;
using QLTV_DangHaNhuThien.Components.Models;

namespace QLTV_DangHaNhuThien.Components.UseDapper
{
    public class ConnectDapper : IConnectDapper
    {
        private readonly string _connectionString;

        public ConnectDapper(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }
        public async Task<IEnumerable<BorrowingRecord>> GetAllUsersAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var sql = "SELECT * FROM BorrowingRecord"; // Lấy tất cả các dòng từ bảng User
                var result = await connection.QueryAsync<BorrowingRecord>(sql); // Sử dụng QueryAsync để lấy danh sách
                return result;
            }
        }

        public async Task<User> GetUserByUsernameAndPassword(string username, string password)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = "SELECT * FROM [User] WHERE Username = @Username AND Password = @Password";
            return await connection.QueryFirstOrDefaultAsync<User>(query, new { Username = username, Password = password });
        }
        // add user new
        public async Task AddNewUserAsync(User users)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "SELECT * FROM [User] WHERE Username = @Username";
                var existingUser = await connection.QueryFirstOrDefaultAsync<User>(query, new { Username = users.Username });

                if (existingUser != null)
                {
                    // User already exists, handle accordingly
                }
                else
                {
                    var insertQuery = "INSERT INTO [User] (Username, Password,FullName, Email,Role) " +
                        "VALUES (@Username, @Password, @FullName, @Email,@Role)";
                    await connection.ExecuteAsync(insertQuery, new { users.Username, users.Password, users.FullName, users.Email , users.Role});
                }
            }
        }
        //get user by id
        public async Task<User> GetUserByIdAsync(int id)
        {

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "SELECT * FROM [User] WHERE Id = @Id";
                var user = await connection.QueryFirstOrDefaultAsync<User>(query, new { Id = id });

                return user;
            }
        }
        //Get uesr list
        public async Task<List<User>> GetUserListAsync()
        {

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "SELECT * FROM [User] ORDER BY Id DESC";
                var result = await connection.QueryAsync<User>(query);

                return result.ToList();
            }
        }
        // update user
        public async Task UpdateNewUserAsync(User users, int id)
        {

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "SELECT * FROM [User] WHERE Id = @Id";
                var resultUpdate = await connection.QueryFirstOrDefaultAsync<User>(query, new { Id = id });

                if (resultUpdate != null)
                {
                    var updateQuery = @"
                    UPDATE [User]
                    SET Username = @Username,
                    Password = @Password,
                    FullName = @FullName,
                    Email = @Email,
                    Role = @Role
                    WHERE Id = @Id";

                    await connection.ExecuteAsync(updateQuery, new
                    {
                        users.Username,
                        users.Password,
                        users.FullName,
                        users.Email,
                        users.Role,
                        Id = id
                    });
                }
            }
        }
        public async Task UpdateNewUserOfuser(User users, int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    // Kiểm tra trước khi cập nhật
                    var checkUserQuery = "SELECT * FROM [User] WHERE Id = @Id";
                    var userToUpdate = await connection.QueryFirstOrDefaultAsync<User>(checkUserQuery, new { Id = id });

                    if (userToUpdate != null)
                    {
                        var updateQuery = @"
                UPDATE [User]
                SET Username = @Username,
                    Password = @Password,
                    FullName = @FullName,
                    Email = @Email,
                    Role = @Role
                WHERE Id = @Id";

                        var result = await connection.ExecuteAsync(updateQuery, new
                        {
                            users.Username,
                            users.Password,
                            users.FullName,
                            users.Email,
                            users.Role,
                            Id = id
                        });

                        if (result == 0)
                        {
                            throw new Exception("Không có bản ghi nào được cập nhật.");
                        }
                    }
                    else
                    {
                        throw new Exception("Người dùng không tồn tại.");
                    }
                }
                catch (Exception ex)
                {
                    // Log hoặc xử lý lỗi thích hợp
                    throw new Exception("Đã xảy ra lỗi khi cập nhật người dùng.", ex);
                }
            }
        }

        public async Task DeleteNewUserAsync(int id)
        {

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "DELETE FROM [User] WHERE Id = @Id";
                await connection.ExecuteAsync(query, new { Id = id });
            }
        }
        // get book list
        public async Task<List<Book>> GetBookListAsync(int? id)
        {

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query;
                if (id.HasValue && id.Value != 0)
                {
                    query = "SELECT * FROM Book WHERE Id = @Id ORDER BY Id DESC";
                    var result = await connection.QueryAsync<Book>(query, new { Id = id.Value });
                    return result.AsList();
                }
                else
                {
                    query = "SELECT * FROM Book ORDER BY Id DESC";
                    var result = await connection.QueryAsync<Book>(query);
                    return result.AsList();
                }
            }
        }
        // delete book
        public async Task DeleteNewBookAsync(int id)
        {

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var deleteQuery = "DELETE FROM Book WHERE Id = @Id";
                await connection.ExecuteAsync(deleteQuery, new { Id = id });
            }
        }
        public async Task<Book> GetBookByIdAsync(int id)
        {

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "SELECT * FROM Book WHERE Id = @Id";
                var book = await connection.QueryFirstOrDefaultAsync<Book>(query, new { Id = id });

                return book;
            }
        }
        public async Task<List<Categorie>> GetCategoryListAsync()
        {

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "SELECT * FROM Categorie ORDER BY Id DESC";
                var result = await connection.QueryAsync<Categorie>(query);

                return result.AsList();
            }
        }
        public async Task AddNewBookAsync(Book book)
        {

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var insertQuery = @"
            INSERT INTO Book (Title, Author, Publisher, PublishedDate, Category, Description, Available,ImagePath)
            VALUES (@Title, @Author, @Publisher, @PublishedDate, @Category, @Description, @Available ,@ImagePath)";

                await connection.ExecuteAsync(insertQuery, new
                {
                    book.Title,
                    book.Author,
                    book.Publisher,
                    book.PublishedDate,
                    book.Category,
                    book.Description,
                    book.Available,
                    book.ImagePath
                });
            }
        }
        public async Task UpdateNewBookAsync(Book book, int id)
        {

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var updateQuery = @"
            UPDATE Book
            SET Title = @Title,
                Author = @Author,
                Publisher = @Publisher,
                PublishedDate = @PublishedDate,
                Category = @Category,
                Description = @Description,
                Available = @Available,
                ImagePath = @ImagePath
            WHERE Id = @Id";

                await connection.ExecuteAsync(updateQuery, new
                {
                    book.Title,
                    book.Author,
                    book.Publisher,
                    book.PublishedDate,
                    book.Category,
                    book.Description,
                    book.Available,
                    book.ImagePath,
                    Id = id
                });
            }
        }
        public async Task<List<Book>> GetBookListAvailableAsync()
        {

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "SELECT * FROM Book WHERE Available = 1 ORDER BY Id DESC"; // 1 là giá trị true trong SQL Server
                var result = await connection.QueryAsync<Book>(query);

                return result.AsList();
            }
        }
        public async Task AddBookLoanAsync(BorrowingRecord borrowingRecord, string username, int bookId)
        {

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Lấy thông tin người dùng
                var getUserQuery = "SELECT Id FROM [User] WHERE Username = @Username";
                var userId = await connection.QueryFirstOrDefaultAsync<int?>(getUserQuery, new { Username = username });

                if (!userId.HasValue)
                {
                    throw new ArgumentException($"Không tìm thấy người dùng có Username là '{username}'.");
                }

                // Lấy thông tin sách
                var getBookQuery = "SELECT Id FROM Book WHERE Id = @BookId";
                var book = await connection.QueryFirstOrDefaultAsync<int?>(getBookQuery, new { BookId = bookId });

                if (!book.HasValue)
                {
                    throw new ArgumentException($"Không tìm thấy sách có Id là '{bookId}'.");
                }

                // Tạo bản ghi mới
                var newRecord = new
                {
                    UserId = userId.Value,
                    BookId = book.Value,
                    BorrowedDate = DateTime.Today,
                    borrowingRecord.DueDate,
                    ReturnedDate = (DateTime?)null,
                    Status = "Đang mượn"
                };

                // Thực hiện chèn vào cơ sở dữ liệu
                var insertQuery = @"
            INSERT INTO BorrowingRecord (UserId, BookId, BorrowedDate, DueDate, ReturnedDate, Status)
            VALUES (@UserId, @BookId, @BorrowedDate, @DueDate, @ReturnedDate, @Status)";

                await connection.ExecuteAsync(insertQuery, newRecord);
            }
        }
		public async Task UpdateReturnedDateAsync(BorrowingRecord book, int id)
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();

				var updateQuery = @"
            UPDATE BorrowingRecord
            SET ReturnedDate = @ReturnedDate,
                Status = @Status
            WHERE Id = @Id";

				await connection.ExecuteAsync(updateQuery, new
				{
					book.ReturnedDate,
					Status = "Đã trả",
					Id = id
				});
			}
		}
        public async Task UpdatRequestDateAsync(BorrowingRecord book, int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var updateQuery = @"
            UPDATE BorrowingRecord
            SET ReturnedDate = @ReturnedDate,
                Status = @Status
            WHERE Id = @Id";

                await connection.ExecuteAsync(updateQuery, new
                {
                    book.ReturnedDate,
                    Status = "Đang yêu cầu",
                    Id = id
                });
            }
        }
        public async Task<List<BorrowingRecord>> GetLoanBookListAsync(string username)
        {

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                if (username == "admin")
                {
                    var query = @"
                SELECT br.*, b.*, u.*
                FROM BorrowingRecord br
                INNER JOIN Book b ON br.BookId = b.Id
                INNER JOIN [User] u ON br.UserId = u.Id
                WHERE br.Status = 'Đang yêu cầu'
                ORDER BY br.Id DESC";

                    var result = await connection.QueryAsync<BorrowingRecord, Book, User, BorrowingRecord>(
                        query,
                        (br, b, u) =>
                        {
                            br.Book = b;
                            br.User = u;
                            return br;
                        },
                        splitOn: "Id"
                    );

                    return result.AsList();
                }
                else
                {
                    var getUserQuery = "SELECT Id FROM [User] WHERE Username = @Username";
                    var userId = await connection.QueryFirstOrDefaultAsync<int?>(getUserQuery, new { Username = username });

                    if (!userId.HasValue)
                    {
                        throw new ArgumentException($"Không tìm thấy người dùng có Username là '{username}'.");
                    }

                    var query = @"
                SELECT br.*, b.*, u.*
                FROM BorrowingRecord br
                INNER JOIN Book b ON br.BookId = b.Id
                INNER JOIN [User] u ON br.UserId = u.Id
                WHERE br.UserId = @UserId AND br.Status = 'Đang yêu cầu'
                ORDER BY br.Id DESC";

                    var result = await connection.QueryAsync<BorrowingRecord, Book, User, BorrowingRecord>(
                        query,
                        (br, b, u) =>
                        {
                            br.Book = b;
                            br.User = u;
                            return br;
                        },
                        new { UserId = userId.Value },
                        splitOn: "Id"
                    );

                    return result.AsList();
                }
            }
        }

        // seach book
        public async Task<IEnumerable<Book>> SearchBooksAsync(string keyword)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var sql = @"SELECT * FROM Book
                    WHERE Title LIKE @Keyword 
                       OR Author LIKE @Keyword 
                       OR Publisher LIKE @Keyword 
                       OR Category LIKE @Keyword 
                       OR Description LIKE @Keyword";

                return await connection.QueryAsync<Book>(sql, new { Keyword = "%" + keyword + "%" });
            }
        }

        public async Task<IEnumerable<User>> SearchUsersAsync(string keyword)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var sql = @"SELECT * FROM [User]
                    WHERE Username LIKE @Keyword 
                       OR FullName LIKE @Keyword 
                       OR Email LIKE @Keyword 
                       OR Role LIKE @Keyword";

                return await connection.QueryAsync<User>(sql, new { Keyword = "%" + keyword + "%" });
            }
        }
        // quên mk
        public async Task<User?> GetUserByEmail(string email)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT * FROM [User] WHERE Email = @Email";
                return await connection.QueryFirstOrDefaultAsync<User>(query, new { Email = email });
            }
        }

        public async Task UpdateUserPassword(User user)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "UPDATE [User] SET Password = @Password WHERE Id = @Id";
                await connection.ExecuteAsync(query, new { Password = user.Password, Id = user.Id });
            }
        }
    }
}