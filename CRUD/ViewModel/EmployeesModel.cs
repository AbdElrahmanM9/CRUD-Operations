using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRUD.ViewModel
{
    public class EmployeesModel
    {
        public string EmployeeName { get; set; }
        public decimal Salary { get; set; }
        public string NationalID { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string Governorate { get; set; }
        public int GovernorateId { get; set; }
        public string Center { get; set; }
        public int CenterId { get; set; }
        public int Village { get; set; }
        public string VillageName { get; set; }
    }
}