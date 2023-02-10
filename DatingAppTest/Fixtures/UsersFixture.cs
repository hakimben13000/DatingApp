using API.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatingAppTest.Fixtures
{
    internal static class UsersFixture
    {
        public static IEnumerable<MemberDto> GetTestUsers()
        {
            var members = new List<MemberDto>()
            {
                new MemberDto()
                {
                    Id=1,
                    UserName="hakim",
                    PhotoUrl="https://randomuser.me/api/portraits/women/50.jpg",
                    Age=30,
                    KnownAs="hakim",
                    Created=DateTime.Now.AddYears(-30),
                    LastActive=DateTime.Now.AddYears(-30),
                    Gender="Male",
                    Introduction="lorem",
                    LookingFor="lorem",
                    Interests="lorem",
                    City="tlemcen",
                    Country="algeria",
                    Photos=new List<PhotoDto>()
                    {
                        new PhotoDto
                        {
                            Id=2,
                            Url="https://randomuser.me/api/portraits/women/50.jpg",
                            IsMain=true
                        }
                    }

                }
            };

            return members;
        }
    }
}
