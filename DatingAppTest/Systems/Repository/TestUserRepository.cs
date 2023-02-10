using API.Data;
using API.DTOs;
using API.Entities;
using API.Repository;
using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatingAppTest.Systems.Repository
{

    public  class TestUserRepository
    {
        [Fact]
        public async Task GetMembers_WhenCalled_InvokesDataContext()
        {
            // arrange
            var mockMapper = new Mock<IMapper>();
            var mockDataContext = new Mock<DataContext>();
            var urt = new UserRepository(mockDataContext.Object, mockMapper.Object);
            // act
            await urt.GetMembersAsync();
            // assert
            
            
        }
        
    }
}
