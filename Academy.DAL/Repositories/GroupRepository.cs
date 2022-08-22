using Academy.DAL.Interfaces;
using Academy.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Academy.DAL.Repositories;

public class GroupRepository :IBaseRepository<Group>
{
    private readonly ApplicationDbContext _context;

    public GroupRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Create(Group group)
    {
        await _context.AddAsync(group);
        await _context.SaveChangesAsync();
    }

    public async Task<Group> Update(Group group)
    {
        _context.Group.Update(group);
        await _context.SaveChangesAsync();
       
        return group;
    }

    public async Task Delete(Group group)
    {
        _context.Group.Remove(group);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Group>> GetAll()
    {
        return await _context.Group.ToListAsync();
    }

    public async Task<Group?> GetById(int groupId)
    {
        return await _context.Group.FirstOrDefaultAsync(g => g.Id == groupId);
    }
}