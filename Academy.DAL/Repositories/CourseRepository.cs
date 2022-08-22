using Academy.DAL.Interfaces;
using Academy.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Academy.DAL.Repositories;

public class CourseRepository : IBaseRepository<Course>
{
    private readonly ApplicationDbContext _context;

    public CourseRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Create(Course course)
    {
        await _context.AddAsync(course);
        await _context.SaveChangesAsync();
    }

    public async Task<Course> Update(Course course)
    {
        _context.Course.Update(course);
        await _context.SaveChangesAsync();
        
        return course;
    }

    public async Task Delete(Course course)
    {
        _context.Course.Remove(course);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Course>> GetAll()
    {
        return await _context.Course.ToListAsync();
    }

    public async Task<Course?> GetById(int courseId)
    {
        return await _context.Course.FirstOrDefaultAsync(c => c.Id == courseId);
    }
}