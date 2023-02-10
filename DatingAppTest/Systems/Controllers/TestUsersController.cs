using API.Controllers;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using API.Repository;
using AutoMapper;
using DatingAppTest.Fixtures;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatingAppTest.Systems.Controllers
{
    public class TestUsersController
    {
        [Fact]
        public async Task Get_OnSuccess_ReturnsStatusCode200()
        {
            // arrange
            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(repo => repo.GetMembersAsync()).ReturnsAsync(UsersFixture.GetTestUsers()); // setup mock repo to return test users
            var mockMapper = new Mock<IMapper>();
           // mockMapper.Setup(mapper => mapper.Map<IEnumerable<MemberDto>>(It.IsAny<IEnumerable<AppUser>>())).Returns(GetTestUsers());
            var mockPhotoService = new Mock<IPhotoService>();
            var controller = new UsersController(mockUserRepository.Object, mockMapper.Object, mockPhotoService.Object);
            // act
            var result = await  controller.GetUsers();
            // assert
            result.Result.Should().BeOfType<OkObjectResult>();
        }
        [Fact]
        public async Task Get_OnSuccess_InvokesUserRepoExactlyOnce()
        {
            // arrange
            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(repo => repo.GetMembersAsync()).ReturnsAsync(UsersFixture.GetTestUsers());
            var mockMapper = new Mock<IMapper>();
          //  mockMapper.Setup(mapper => mapper.Map<IEnumerable<MemberDto>>(It.IsAny<IEnumerable<AppUser>>())).Returns(GetTestUsers());
            var mockPhotoService = new Mock<IPhotoService>();
            var controller = new UsersController(mockUserRepository.Object, mockMapper.Object, mockPhotoService.Object);
            // act
            var result = await controller.GetUsers();
            // assert
            //Assert.IsType<ActionResult<IEnumerable<MemberDto>>>(result);
            mockUserRepository.Verify(repo => repo.GetMembersAsync(), Times.Once); // verify that GetMembersAsync is called once
        }
        
        [Fact]
        public async Task Get_OnSuccess_ReturnListOfMembersDto()
        {
            // arrange
            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(repo => repo.GetMembersAsync()).ReturnsAsync(UsersFixture.GetTestUsers());
            var mockMapper = new Mock<IMapper>();
            //  mockMapper.Setup(mapper => mapper.Map<IEnumerable<MemberDto>>(It.IsAny<IEnumerable<AppUser>>())).Returns(GetTestUsers());
            var mockPhotoService = new Mock<IPhotoService>();
            var controller = new UsersController(mockUserRepository.Object, mockMapper.Object, mockPhotoService.Object);
            // act
            var result = await controller.GetUsers();
            // assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var objectResult = (OkObjectResult)result.Result;
            objectResult.Value.Should().BeOfType<List<MemberDto>>();
        }

        [Fact]
        public async Task Get_OnNoUsersFound_Return404()
        {
            // arrange
            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(repo => repo.GetMembersAsync()).ReturnsAsync(new List<MemberDto>());
            var mockMapper = new Mock<IMapper>();
            //  mockMapper.Setup(mapper => mapper.Map<IEnumerable<MemberDto>>(It.IsAny<IEnumerable<AppUser>>())).Returns(GetTestUsers());
            var mockPhotoService = new Mock<IPhotoService>();
            var controller = new UsersController(mockUserRepository.Object, mockMapper.Object, mockPhotoService.Object);
            // act
            var result = await controller.GetUsers();
            // assert
            result.Result.Should().BeOfType<NotFoundResult>();
            var objectResult = (NotFoundResult)result.Result;
            objectResult.StatusCode.Should().Be(404);
        }
       
    }
}
