using System.Diagnostics.CodeAnalysis;

namespace WorkforceManagement.Models.User
{
    [ExcludeFromCodeCoverage]
    public class ViewUserModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
