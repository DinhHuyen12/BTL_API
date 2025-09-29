using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using DAL.Helper;
using DAL.Interfaces;
using Model;

namespace DAL
{
    public partial class AuthRepository : IAuthRepository
    {
        private readonly IDataHelper _dbHelper;

        public AuthRepository(IDataHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public Users Login(string username)
        {
            try
            {
                // input params
                var inputParams = new Dictionary<string, object>
                {
                    { "@username", username }
                };

                // Gọi procedure Pro_Login
                var dt = _dbHelper.ExcuteReader("Pro_Login", inputParams);

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    return new Users
                    {
                        UserId = Convert.ToInt32(row["UserId"]),
                        Username = row["Username"].ToString(),
                        FullName = row["FullName"].ToString(),
                        Email = row["Email"].ToString(),
                        RoleId = Convert.ToInt32(row["RoleId"]),
                        RoleName = row["RoleName"].ToString()
                    };
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Login error: " + ex.Message);
                return null;
            }
        }

        public Dictionary<string, object> CreateUser(Users user)
        {
            var response = new Dictionary<string, object>
            {
                { "Success", false },
                { "ErrorCode", 0 },
                { "Message", "" }
            };

            try
            {
                var inputParams = new Dictionary<string, object>
                {
                    { "@userName", user.Username },
                    { "@password", user.PasswordHash },
                    { "@fullName", user.FullName },
                    { "@email", user.Email },
                    { "@roleId", user.RoleId }
                };

                var outputParams = new List<SqlParameter>
                {
                    new SqlParameter("@p_error_code", SqlDbType.Int) { Direction = ParameterDirection.Output },
                    new SqlParameter("@p_error_message", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output }
                };

                var result = _dbHelper.ExcuteNonQuery("Pro_Register", inputParams, outputParams);
                foreach (var param in outputParams)
                {
                    Console.WriteLine($"{param.ParameterName} = {param.Value}");
                }

                // Hoặc log toàn bộ dictionary result
                Console.WriteLine("Result dictionary:");
                foreach (var kv in result)
                {
                    Console.WriteLine($"{kv.Key} = {kv.Value}");
                }
                if (result != null && result.ContainsKey("@p_error_code"))
                {
                    int errorCode = Convert.ToInt32(result["@p_error_code"]);
                    string errorMsg = result["@p_error_message"]?.ToString() ?? "";

                    response["ErrorCode"] = errorCode;
                    response["Message"] = errorMsg;

                    if (errorCode == 0)
                    {
                        response["Success"] = true;
                        response["Message"] = "Tạo user thành công";
                    }
                }
            }
            catch (Exception ex)
            {
                response["ErrorCode"] = -1;
                response["Message"] = $"Exception: {ex.Message}";
            }

            return response;
        }

        public bool UpdateUser(Users user)
        {
            // TODO: viết proc update và call tương tự CreateUser
            return true;
        }

        public bool DeleteUser(Users user)
        {
            // TODO: viết proc delete và call tương tự
            return true;
        }
    }
}
