﻿using CourseFlow.Models;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Net;

namespace CourseFlow.Repositories
{
    public class UserRepository : RepositoryBase, IUserRepository
    {
        public void Add(UserModel userModel)
        {
            throw new NotImplementedException();
        }

        public bool AuthenticateUser(NetworkCredential credential)
        {
            bool validUser;
            using (var connection = new OleDbConnection(_connectionString))
            using (var command = new OleDbCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM [User] WHERE Username = @Username AND [Password] = @Password";
                command.Parameters.AddWithValue("@Username", credential.UserName);
                command.Parameters.AddWithValue("@Password", credential.Password);
                validUser = command.ExecuteScalar() != null;
            }
            return validUser;
        }

        public void Edit(UserModel userModel)
        {
            AcademicCourseModel model = new AcademicCourseModel();
            model.AcademicYear.ToString();
            throw new NotImplementedException();
        }

        public IEnumerable<UserModel> GetByAll()
        {
            throw new NotImplementedException();
        }

        public UserModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public UserModel GetByUsername(string UserName)
        {
            UserModel user = null;
            using (var connection = new OleDbConnection(_connectionString))
            using (var command = new OleDbCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM [User] WHERE Username = @Username";
                command.Parameters.AddWithValue("@Username", UserName);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = new UserModel()
                        {
                            Id = reader[0].ToString(),
                            Username = reader[1].ToString(),
                            Password = string.Empty,
                            FirstName = reader[3].ToString(),
                            LastName = reader[4].ToString(),
                            Email = reader[5].ToString()
                        };
                    }
                }
            }
            return user;
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }
    }
}
