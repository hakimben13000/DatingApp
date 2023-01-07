using API.Data;
using API.DTOs;
using API.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext context;
        private readonly IMapper mapper;

        public UserRepository(DataContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<MemberDto> GetMemberAsync(string username)
        {
            return await context.Users
               // .Include(p => p.Photos)
                .ProjectTo<MemberDto>(mapper.ConfigurationProvider) // Map from AppUser to MemberDto with ProjectTo extension method of AutoMapper 
                .SingleOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<IEnumerable<MemberDto>> GetMembersAsync()
        {
            return await context.Users
              //  .Include(p => p.Photos)
                .ProjectTo<MemberDto>(mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await context.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await context.Users
                 .Include(u => u.Photos)
                .SingleOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await context.Users
                .Include(u=>u.Photos)
                .ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await context.SaveChangesAsync()>0;
        }

        public void Update(AppUser user)
        {
            context.Entry(user).State = EntityState.Modified;
        }
    }
}
