﻿namespace EmployeeMgmt.Application.DTOs
{
    public class EmployeeDTO
    {
        public int EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime HireDate { get; set; }
        public int DepartmentId { get; set; }

        public  DepartmentDTO? Department { get; set; }
    }

}
