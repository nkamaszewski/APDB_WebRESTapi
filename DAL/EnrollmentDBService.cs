﻿using APDB_WebRESTapi.DTOs;
using APDB_WebRESTapi.DTOs.Response;
using APDB_WebRESTapi.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace APDB_WebRESTapi.DAL
{
    public class EnrollmentDBService : IEnrollmentDBService
    {
        private string SqlConn = "Data Source=db-mssql;Initial Catalog=s16456;Integrated Security=True";
        public RegisterStudentStatus RegisterStudent(RegistrationStudentRequest registrationStudentRequest)
        {
            RegisterStudentStatus registerStudentStatus = new RegisterStudentStatus();
            using (var client = new SqlConnection(SqlConn))
            using (var command = new SqlCommand())
            {

                command.Connection = client;
                client.Open();

                var transaction = client.BeginTransaction();
                command.Transaction = transaction;
                try
                {


                    command.CommandText = $"SELECT IdStudy FROM STUDIES WHERE Name=@name";
                    command.Parameters.AddWithValue("name", registrationStudentRequest.Studies);

                    var dataReader = command.ExecuteReader();

                    // study doesn't exist
                    if (!dataReader.Read())
                    {
                        dataReader.Close();
                        transaction.Rollback();
                        registerStudentStatus.enrollment = null;
                        registerStudentStatus.Status = 400;
                        registerStudentStatus.Message = "No studies in dataBase";
                        return registerStudentStatus;
                    }

                    string IdStudy = dataReader["IdStudy"].ToString();

                    // selects latest Enrollment
                    dataReader.Close();
                    command.CommandText = $"SELECT IdEnrollment, StartDate FROM Enrollment WHERE Enrollment.Semester = 1  AND Enrollment.IdStudy = @IdStudy AND Enrollment.StartDate = (SELECT MAX(StartDate) FROM Enrollment WHERE Enrollment.Semester = 1  AND Enrollment.IdStudy = @IdStudy);";
                    command.Parameters.AddWithValue("IdStudy", IdStudy);

                    dataReader = command.ExecuteReader();
                    DateTime StartDate = DateTime.Now;
                    int IdEnrollemnt = -1;

                    // if Enrollment doesn't exist
                    if (!dataReader.Read())
                    {
                        command.CommandText = $"INSERT INTO Enrollment VALUES(SELECT MAX(IdEnrollment) + 1 FROM Enrollment WHERE Enrollment.IdStudy = @IdStudy) + 1, 1, @IdStudy, @StartDate)";
                        command.Parameters.AddWithValue("IdStudy", IdStudy);
                        command.Parameters.AddWithValue("StartDate", DateTime.Now);
                        command.ExecuteReader();

                        dataReader.Close();
                        command.CommandText = $"SELECT IdEnrollment, StartDate FROM Enrollment WHERE Enrollment.Semester = 1  AND Enrollment.IdStudy = @IdStudy AND Enrollment.StartDate = (SELECT MAX(StartDate) FROM Enrollment WHERE Enrollment.Semester = 1  AND Enrollment.IdStudy = @IdStudy);";
                        command.Parameters.AddWithValue("IdStudy", IdStudy);

                        dataReader = command.ExecuteReader();
                        dataReader.Read();
                    }
                    else
                    {
                        StartDate = DateTime.Parse(dataReader["StartDate"].ToString());
                    }

                    IdEnrollemnt = int.Parse(dataReader["IdEnrollment"].ToString());
                    // adding student
                    dataReader.Close();
                    command.CommandText = $"SELECT IndexNumber FROM Student WHERE IndexNumber = @IndexNumber";
                    command.Parameters.AddWithValue("IndexNumber", registrationStudentRequest.IndexNumber);
                    dataReader = command.ExecuteReader();

                    // student with that indexNumber already exists
                    if (dataReader.Read())
                    {
                        dataReader.Close();
                        transaction.Rollback();
                        registerStudentStatus.enrollment = null;
                        registerStudentStatus.Status = 400;
                        registerStudentStatus.Message = "Given IndexNumber is not unique! Student already exists in dataBase";
                        return registerStudentStatus;
                    }
                    dataReader.Close();


                    command.CommandText = $"INSERT INTO Student VALUES(@IndexNumber, @FirstName, @LastName, @BirthDate, @IdEnrollment)";
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("IndexNumber", registrationStudentRequest.IndexNumber);
                    command.Parameters.AddWithValue("FirstName", registrationStudentRequest.FirstName);
                    command.Parameters.AddWithValue("LastName", registrationStudentRequest.LastName);
                    command.Parameters.AddWithValue("BirthDate", registrationStudentRequest.BirthDate);
                    command.Parameters.AddWithValue("IdEnrollment", IdEnrollemnt);
                    //dataReader = command.ExecuteReader();
                    command.ExecuteNonQuery();
                    transaction.Commit();

                    RegistrationStudentEnrollmentResponse enrollment = new RegistrationStudentEnrollmentResponse()
                    {
                        IdEnrollment = IdEnrollemnt,
                        Semester = 1,
                        IdStudy = int.Parse(IdStudy),
                        StartDate = StartDate,
                    };

                    registerStudentStatus.enrollment = enrollment;
                    registerStudentStatus.Status = 201;
                    registerStudentStatus.Message = "Student added";

                }
                catch (SqlException exc)
                {
                    transaction.Rollback();
                    registerStudentStatus.enrollment = null;
                    registerStudentStatus.Status = 400;
                    registerStudentStatus.Message = "dataBase exception";

                }

            }
            return registerStudentStatus;
        }
    }
}
