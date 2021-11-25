using AccountManagers.DataAccess.Data;
using AccountManagers.Models.User;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountManagers.DataAccess.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private string _connectionString = GetConnectionString.ConnectionValue("DbConn");

        public IEnumerable<User> GetAll()
        {
            List<User> users = new List<User>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand("UserGetAll", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlDataReader dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {
                        var user = new User();

                        user.Id = int.Parse(dataReader["Id"].ToString());
                        user.Name = dataReader["Name"].ToString();
                        user.Email = dataReader["Email"].ToString();
                        user.CNP = dataReader["CNP"].ToString();

                        int.TryParse(dataReader["NoOfClients"].ToString(),out int noOfClients);
                        user.NoOfClients = noOfClients;

                        users.Add(user);

                    }
                    dataReader.Close();
                }
            }

            return users;
        }

        public int InsertUser(User user)
        {
            int id;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("UserInsert", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                //IN Parameters
                cmd.Parameters.Add("@Name", SqlDbType.NVarChar);
                cmd.Parameters["@Name"].Value = user.Name;
                cmd.Parameters.Add("@Email", SqlDbType.NVarChar);
                cmd.Parameters["@Email"].Value = user.Email;
                cmd.Parameters.Add("@CNP", SqlDbType.NVarChar);
                cmd.Parameters["@CNP"].Value = user.CNP;
                cmd.Parameters.Add("@NoOfClients", SqlDbType.Int);
                cmd.Parameters["@NoOfClients"].Value = user.NoOfClients;


                //OUTPUT Parameters
                cmd.Parameters.Add("@Id", SqlDbType.Int);
                cmd.Parameters["@Id"].Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();

                id = Convert.ToInt32(cmd.Parameters["@Id"].Value);              

            }
            return id;
        }

        public void UpdateUser(User user)
        {           
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("UserUpdate", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@Id", SqlDbType.NVarChar);
                cmd.Parameters["@Id"].Value = user.Id;
                cmd.Parameters.Add("@Name", SqlDbType.NVarChar);
                cmd.Parameters["@Name"].Value = user.Name;
                cmd.Parameters.Add("@Email", SqlDbType.NVarChar);
                cmd.Parameters["@Email"].Value = user.Email;
                cmd.Parameters.Add("@CNP", SqlDbType.NVarChar);
                cmd.Parameters["@CNP"].Value = user.CNP;
                cmd.Parameters.Add("@NoOfClients", SqlDbType.Int);
                cmd.Parameters["@NoOfClients"].Value = user.NoOfClients;         

                cmd.ExecuteNonQuery();
            }          
        }

        public void DeleteUser(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("UserDelete", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@Id", SqlDbType.Int);
                cmd.Parameters["@Id"].Value = id;

                cmd.ExecuteNonQuery();
            }
        }
    }
}
