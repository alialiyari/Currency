using Models;
using System;
using System.Collections.Generic;

namespace Userr
{

    public class SignInRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }

        public string Role { get; set; }
    }
    public class SignInResponse
    {
        public string Token { get; set; }
        public DateTime ExpireDate { get; set; }
    }


    public class SmsPassModel
    {
        public string Mobile { get; set; }
        public string SmsCode { get; set; }
        public string NationalCode { get; set; }
    }

    public class InsertSaveModel
    {
        public long? RoleId { get; set; }
        public string RoleObjectId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NationalCode { get; set; }


        public string UserName { get; set; }
        public string Password { get; set; }

        public string ShSh { get; set; }
        public DateTime? BirthDate { get; set; }

        public string Mobile { get; set; }
        public bool MobileConfirmed { get; set; }



        public string Email { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }


        public long? IntroducerUserId { get; set; }


        public ActiveStatusEnum ActiveStatus { get; set; }
        public VerifyStatusEnum VerifyStatus { get; set; }

        public FileDto ImageFile { get; set; }
        public FileDto NationalCardFile { get; set; }

    }

    public class SaveModel
    {
        public long Id { get; set; }
        //public long? IntroducerUserId { get; set; }


        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NationalCode { get; set; }


        public string UserName { get; set; }
        public string Password { get; set; }


        public string Mobile { get; set; }
        public bool MobileConfirmed { get; set; }

        public string ShSh { get; set; }
        public DateTime? BirthDate { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }
        public string PostalCode { get; set; }


        public FileDto ImageFile { get; set; }
        public FileDto NationalCardFile { get; set; }


        public ActiveStatusEnum ActiveStatus { get; set; }
        public VerifyStatusEnum VerifyStatus { get; set; }







        public string WorkPhone { get; set; }


        public GenderEnum Gender { get; set; }
        public string FatherName { get; set; }


    }

    public class ViewModel
    {
        public long Id { get; set; }




        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NationalCode { get; set; }

        public string UserName { get; set; }


        public DateTime RegisterDate { get; set; }
        public DateTime? LastLoginDate { get; set; }


        public string Mobile { get; set; }
        public bool MobileConfirmed { get; set; }



        public string Address { get; set; }
        public string PostalCode { get; set; }

        public string ShSh { get; set; }
        public DateTime? BirthDate { get; set; }

        public string ImageUrl { get; set; }
        public string NationalCardUrl { get; set; }

        public ActiveStatusEnum ActiveStatus { get; set; }
        public VerifyStatusEnum VerifyStatus { get; set; }






        public long? IntroducerUserId { get; set; }
        public string IntroducerUserFullName { get; set; }








        public string WorkPhone { get; set; }

        public string VerifierUserId { get; set; }
        public string VerifyDate { get; set; }


        public string Email { get; set; }
        public GenderEnum Gender { get; set; }
        public string FatherName { get; set; }
    }


    public class ListModel
    {
        public long Id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string NationalCode { get; set; }
        public DateTime RegisterDate { get; set; }
        public ActiveStatusEnum ActiveStatus { get; set; }
        public VerifyStatusEnum VerifyStatus { get; set; }
    }







    public class RolesSaveModel
    {
        public class UserRole
        {
            public bool IsGrant { get; set; }
            public string RoleName { get; set; }

        }
        public long UserId { get; set; }
        public List<UserRole> Roles { get; set; }
    }
    public class RolesViewModel
    {
        public List<string> Roles { get; set; }
    }
    public class UserRoleModel
    {
        public long? Id { get; set; }

        public long? RoleId { get; set; }
        public string RoleTitle { get; set; }
        public string RoleName { get; set; }

        public long? UserId { get; set; }
        public string UserFullName { get; set; }


    }
}
