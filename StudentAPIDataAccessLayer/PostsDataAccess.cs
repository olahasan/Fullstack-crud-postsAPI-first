//using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Data;
using StudentAPIDataAccessLayer;
using Microsoft.IdentityModel.Protocols;

namespace StudentAPIDataAccessLayer
{

    public class PostDTO
    {
        public PostDTO(int id, string title, string description, string userId)
        {
            this.Id = id;
            this.Title = title;
            this.Description = description;
            this.UserId = userId;
        }


        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }

    }

    public class PostsDataAccess
    {
        private static readonly string _connectionString = "Server=.;Database=CrudApp_Kimz;trusted_connection=true;TrustServerCertificate=True;";
        //public PostsDataAccess(IConfiguration configuration)
        //{
        //  _connectionString = configuration.GetConnectionString("DefaultConnection");
        //}
        //static string _connectionString = "Server=localhost;Database=CrudApp_Kimz;User Id=sa;Password=sa123456;Encrypt=False;TrustServerCertificate=True;Connection Timeout=30;";

        public static List<PostDTO> GetAllPosts()
        {
            var PostList = new List<PostDTO>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_GetAllPosts", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    connection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PostList.Add(new PostDTO
                            (
                                reader.GetInt32(reader.GetOrdinal("Id")),
                                reader.GetString(reader.GetOrdinal("Title")),
                                reader.GetString(reader.GetOrdinal("Description")),
                                reader.GetString(reader.GetOrdinal("UserId")) 
                            ));
                        }
                    }
                }


                return PostList;
            }

        }


        public static bool DeletePost(int postId)
        {

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("SP_DeletePost", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PostId", postId);

                    connection.Open();

                    int rowsAffected = (int)command.ExecuteScalar();
                    //int rowsAffected = (int)command.ExecuteNonQuery();
                    return (rowsAffected == 1);
                }
            }
            
        }


        public static int AddPost(PostDTO PostDTO)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("SP_AddNewPost", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@Title", PostDTO.Title);
                command.Parameters.AddWithValue("@Description", PostDTO.Description);
                command.Parameters.AddWithValue("@UserId", PostDTO.UserId);
                var outputIdParam = new SqlParameter("@NewPostId", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(outputIdParam);

                connection.Open();
                command.ExecuteNonQuery();

                return (int)outputIdParam.Value;
            }
        }




        public static bool UpdatePost(PostDTO PostDTO)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("SP_UpdatePost", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@PostId", PostDTO.Id);
                command.Parameters.AddWithValue("@Title", PostDTO.Title);
                command.Parameters.AddWithValue("@Description", PostDTO.Description);
                command.Parameters.AddWithValue("@UserId", PostDTO.UserId);

                connection.Open();
                command.ExecuteNonQuery();
                return true;

            }
        }




        public static PostDTO GetPostById(int PostId)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("SP_GetPostById", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@PostId", PostId);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new PostDTO
                        (
                            reader.GetInt32(reader.GetOrdinal("Id")),
                            reader.GetString(reader.GetOrdinal("Title")),
                            reader.GetString(reader.GetOrdinal("Description")),
                            reader.GetString(reader.GetOrdinal("UserId"))
                        );
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
    }
}


