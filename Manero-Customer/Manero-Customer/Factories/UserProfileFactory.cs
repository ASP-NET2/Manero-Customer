using Manero_Customer.Data.Models;
using Microsoft.Extensions.Azure;

namespace Manero_Customer.Factories;

public class UserProfileFactory
{
    public static void UpdateUserProfile(ProfileModel userProfile, ProfileModel user)
    {
        user.FirstName = userProfile.FirstName;
        user.LastName = userProfile.LastName;
        user.Email = userProfile.Email;
        user.IdentityUserId = userProfile.IdentityUserId;
        user.PhoneNumber = userProfile.PhoneNumber;
        user.Location = userProfile.Location;
        user.AccountId = userProfile.AccountId;
        user.ImageUrl = userProfile.ImageUrl;

        if (userProfile == null || user == null)
        {
            throw new ArgumentNullException(nameof(userProfile), "User profile or user cannot be null");
        }
    }
}
