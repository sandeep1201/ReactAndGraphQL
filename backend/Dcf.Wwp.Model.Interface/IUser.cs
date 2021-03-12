using System;

namespace Dcf.Wwp.Model.Interface
{
    public interface IUser
    {
        Int32 Id { get; set; }                // WorkProgramLogin.Id
        String UserName { get; set; }       // WorkProgramLogin.WamsId
        String OfficeName { get; set; }     // WorkProgramLogin.OfficeName
        Int32 OfficeNumber { get; set; }       // WorkProgramLogin.OfficNumber
        Int32? CountyNumber { get; set; }      // WorkProgramLogin.CountyNumber
        String FirstName { get; set; }     // WorkerDetail.WorkerFirstName
        String LastName { get; set; }     // WorkerDetail.WorkerLastName
        String WorkerRole1 { get; set; }   //WorkerDetail.Role1
        String WorkerRole2 { get; set; }   //WorkerDetail.Role2
    }
}