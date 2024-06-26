﻿using aitus.Models;

namespace aitus.Interfaces
{
    public interface IStudentRepository
    {
        ICollection<Student> GetStudents();
        Student GetStudent(int Id);
        int GetStudentBarcode(string Email);
        bool StudentExists(int Id);
        Task<Student> GetStudentByEmailAndPasswordAsync(string email, string password);
    }
}
