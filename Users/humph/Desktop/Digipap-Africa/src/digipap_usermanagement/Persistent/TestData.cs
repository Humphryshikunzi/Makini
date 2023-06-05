using Digipap.Models.Identity.DbEntity; 

namespace Digipap.Persistent
{
    public static class TestUser
    {
        public static List<User> TestUsers = new List<User>()
        {
            // This is the default Admin
            
            new  User()
            {
                PasswordHash = "123456",
                UserName = "testUser",
                Email = "humphry.shikunzi@digipapafrica.com",
                CreatedBy = "Seeded",
                NationalId = "36058390",
                FirstName = "Humphry",
                LastName = "Shikunzi",              
            }
        };
      
    }
}



