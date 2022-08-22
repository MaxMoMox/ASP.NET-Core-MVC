using Academy.DAL.Interfaces;
using Academy.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Academy.DAL.Repositories;

public class StudentRepository : IBaseRepository<Student>
{
    private readonly ApplicationDbContext _context;

    public StudentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Create(Student student)
    {
        await _context.AddAsync(student);
        await _context.SaveChangesAsync();
    }

    public async Task<Student> Update(Student student)
    {
        _context.Student.Update(student);
        await _context.SaveChangesAsync();
       
        return student;
    }

    public async Task Delete(Student student)
    {
        _context.Student.Remove(student);
        await _context.SaveChangesAsync();
    }

    public Task<List<Student>> GetAll()
    {
        return _context.Student.ToListAsync();
    }

    public async Task<Student?> GetById(int studentId)
    {
        return await _context.Student.FirstOrDefaultAsync(s => s.Id == studentId);
    }
}