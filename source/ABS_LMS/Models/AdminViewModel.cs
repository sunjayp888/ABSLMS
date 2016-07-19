using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using PagedList;

namespace ABS_LMS.Models
{
    public class RoleViewModel
    {
        public string Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "RoleName")]
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class AdminUserViewModel
    {
        public string AspNetUserId { get; set; }
        
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please Enter First Name")]
        [StringLength(50)]
        [Display(Name = "First Name")]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "First Name: Please enter letters only")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please Enter Last Name")]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Last Name: Please enter letters only")]
        public string LastName { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

     
        public IEnumerable<SelectListItem> RolesList { get; set; }
        public string SearchKeyword{ get; set; }
       
        public IPagedList<ApplicationUser> Users { get; set; } 
        public string Role { get; set; }
    }
}