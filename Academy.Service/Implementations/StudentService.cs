using Academy.DAL.Interfaces;
using Academy.Domain.Entities;
using Academy.Domain.Enums;
using Academy.Domain.Responses;
using Academy.Service.Interfaces;

namespace Academy.Service.Implementations;

public class StudentService : IHierarchyService<Student>
{
    private readonly IBaseRepository<Student> _repository;

    public StudentService(IBaseRepository<Student> repository)
    {
        _repository = repository;
    }

    public async Task<BaseResponse<Student>> Create(Student model)
    {
        var baseResponse = new BaseResponse<Student>();

        try
        {
            if (string.IsNullOrEmpty(model.FirstName) || string.IsNullOrEmpty(model.LastName) || model.GroupId == null || model.CourseId == null)
            {
                baseResponse.Description = "[StudentService.Create] : Invalid input data.";
                baseResponse.StatusCode = StatusCode.InputDataError;

                return baseResponse;
            }

            var student = new Student
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                GroupId = model.GroupId,
                CourseId = model.CourseId
            };

            await _repository.Create(student);

            baseResponse.Data = student;
            baseResponse.StatusCode = StatusCode.Ok;

            return baseResponse;
        }
        catch (Exception e)
        {
            baseResponse.Description = $"[StudentService.Create] : {e.Message}";
            baseResponse.StatusCode = StatusCode.InternalServerError;

            return baseResponse;
        }
    }

    public async Task<BaseResponse<Student>> Update(int studentId, Student model)
    {
        var baseResponse = new BaseResponse<Student>();

        try
        {
            if (string.IsNullOrEmpty(model.FirstName) || string.IsNullOrEmpty(model.LastName) || model.GroupId == null || model.CourseId == null || studentId <= 0)
            {
                baseResponse.Description = "[StudentService.Update] : Invalid input data.";
                baseResponse.StatusCode = StatusCode.InputDataError;

                return baseResponse;
            }
            var student = await _repository.GetById(studentId);

            if (student == null)
            {
                baseResponse.Description = "[StudentService.Update] : Wrong student ID. There are no students with this ID.";
                baseResponse.StatusCode = StatusCode.StudentsNotFound;

                return baseResponse;
            }

            student.FirstName = model.FirstName;
            student.LastName = model.LastName;
            student.GroupId = model.GroupId;
            student.CourseId = model.CourseId;

            await _repository.Update(student);

            baseResponse.Data = student;
            baseResponse.StatusCode = StatusCode.Ok;

            return baseResponse;
        }
        catch (Exception e)
        {
            baseResponse.Description = $"[StudentService.Update] : {e.Message}";
            baseResponse.StatusCode = StatusCode.InternalServerError;

            return baseResponse;
        }
    }

    public async Task<BaseResponse<Student>> Delete(int studentId)
    {
        var baseResponse = new BaseResponse<Student>();

        try
        {
            if (studentId <= 0)
            {
                baseResponse.Description = "[StudentService.Delete] : Invalid input data.";
                baseResponse.StatusCode = StatusCode.InputDataError;

                return baseResponse;
            }

            var student = await _repository.GetById(studentId);

            if (student == null)
            {
                baseResponse.Description = "[StudentService.Delete] : Wrong student ID. There are no students with this ID.";
                baseResponse.StatusCode = StatusCode.StudentsNotFound;

                return baseResponse;
            }

            await _repository.Delete(student);

            baseResponse.Data = student;
            baseResponse.StatusCode = StatusCode.Ok;

            return baseResponse;
        }
        catch (Exception e)
        {
            baseResponse.Description = $"[StudentService.Delete] : {e.Message}";
            baseResponse.StatusCode = StatusCode.InternalServerError;

            return baseResponse;
        }
    }

    public async Task<BaseResponse<List<Student>>> GetAll()
    {
        var baseResponse = new BaseResponse<List<Student>>();

        try
        {
            var students = await _repository.GetAll();

            baseResponse.Data = students;
            baseResponse.StatusCode = StatusCode.Ok;

            return baseResponse;
        }
        catch (Exception e)
        {
            baseResponse.Description = $"[StudentService.GetAll] : {e.Message}";
            baseResponse.StatusCode = StatusCode.InternalServerError;

            return baseResponse;
        }
    }

    public async Task<BaseResponse<Student>> GetById(int studentId)
    {
        var baseResponse = new BaseResponse<Student>();

        try
        {
            var student = await _repository.GetById(studentId);

            if (student == null)
            {
                baseResponse.Description = "[StudentService.GetById] : Wrong student ID. There are no students with this ID.";
                baseResponse.StatusCode = StatusCode.StudentsNotFound;

                return baseResponse;
            }

            baseResponse.Data = student;
            baseResponse.StatusCode = StatusCode.Ok;

            return baseResponse;
        }
        catch (Exception e)
        {
            baseResponse.Description = $"[StudentService.GetById] : {e.Message}";
            baseResponse.StatusCode = StatusCode.InternalServerError;

            return baseResponse;
        }
    }
}