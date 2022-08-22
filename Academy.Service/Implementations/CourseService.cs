using Academy.DAL.Interfaces;
using Academy.Domain.Entities;
using Academy.Domain.Enums;
using Academy.Domain.Responses;
using Academy.Service.Interfaces;

namespace Academy.Service.Implementations;

public class CourseService : IHierarchyService<Course>
{
    private readonly IBaseRepository<Course> _courseRepository;
    private readonly IBaseRepository<Group> _groupRepository;

    public CourseService(IBaseRepository<Course> courseRepository, IBaseRepository<Group> groupRepository)
    {
        _courseRepository = courseRepository;
        _groupRepository = groupRepository;
    }

    public async Task<BaseResponse<Course>> Create(Course model)
    {
        var baseResponse = new BaseResponse<Course>();

        try
        {
            if (string.IsNullOrEmpty(model.Name)) 
            {
                baseResponse.Description = "[CourseService.Create] : Invalid input data.";
                baseResponse.StatusCode = StatusCode.InputDataError;

                return baseResponse;
            }

            var course = new Course
            {
                Name = model.Name,
                Description = model.Description
            };

            await _courseRepository.Create(course);

            baseResponse.Data = course;
            baseResponse.StatusCode = StatusCode.Ok;

            return baseResponse;
        }
        catch (Exception e)
        {
            baseResponse.Description = $"[CourseService.Create] : {e.Message}";
            baseResponse.StatusCode = StatusCode.InternalServerError;

            return baseResponse;
        }
    }

    public async Task<BaseResponse<Course>> Update(int courseId, Course model)
    {
        var baseResponse = new BaseResponse<Course>();

        try
        { 
            if (string.IsNullOrEmpty(model.Name) || courseId <= 0)
            {
                baseResponse.Description = "[CourseService.Update] : Invalid input data.";
                baseResponse.StatusCode = StatusCode.InputDataError;

                return baseResponse;
            }

            var course = await _courseRepository.GetById(courseId);

            if (course == null)
            {
                baseResponse.Description = "[CourseService.Update] : Wrong course ID. There are no courses with this ID.";
                baseResponse.StatusCode = StatusCode.CoursesNotFound;

                return baseResponse;
            } 

            course.Name = model.Name;
            course.Description = model.Description;

            await _courseRepository.Update(course);

            baseResponse.Data = course;
            baseResponse.StatusCode = StatusCode.Ok;

            return baseResponse;
        }
        catch (Exception e)
        {
            baseResponse.Description = $"[CourseService.Update] : {e.Message}";
            baseResponse.StatusCode = StatusCode.InternalServerError;

            return baseResponse;
        }
    }

    public async Task<BaseResponse<Course>> Delete(int courseId)
    {
        var baseResponse = new BaseResponse<Course>();

        try
        {
            if (courseId <= 0)
            {
                baseResponse.Description = "[CourseService.Delete] : Invalid input data.";
                baseResponse.StatusCode = StatusCode.InputDataError;

                return baseResponse;
            }

            var course = await _courseRepository.GetById(courseId);

            if (course == null)
            {
                baseResponse.Description = "[CourseService.Delete] : Wrong course ID. There are no courses with this ID.";
                baseResponse.StatusCode = StatusCode.CoursesNotFound;

                return baseResponse;
            }

            IEnumerable<Group> courseGroups = await _groupRepository.GetAll();

            if (courseGroups.Any(g => g.CourseId == courseId))
            {
                baseResponse.StatusCode = StatusCode.NotEmptyCourseRemoval;
                baseResponse.Data = course;

                return baseResponse;
            }

            await _courseRepository.Delete(course);

            baseResponse.StatusCode = StatusCode.Ok;
            baseResponse.Data = course;

            return baseResponse;
        }
        catch (Exception e)
        {
            baseResponse.Description = $"[CourseService.Delete] : {e.Message}";
            baseResponse.StatusCode = StatusCode.InternalServerError;

            return baseResponse;
        }
    }

    public async Task<BaseResponse<List<Course>>> GetAll()
    {
        var baseResponse = new BaseResponse<List<Course>>();

        try
        { 
            var courses = await _courseRepository.GetAll();

            baseResponse.Data = courses;
            baseResponse.StatusCode = StatusCode.Ok;

            return baseResponse;
        }
        catch (Exception e)
        {
            baseResponse.Description = $"[CourseService.GetAll] : {e.Message}";
            baseResponse.StatusCode = StatusCode.InternalServerError;

            return baseResponse;
        }
    }

    public async Task<BaseResponse<Course>> GetById(int courseId)
    {
        var baseResponse = new BaseResponse<Course>();

        try
        {
            var course = await _courseRepository.GetById(courseId);

            if (course == null)
            {
                baseResponse.Description = "[CourseService.GetById] : Wrong course ID. There are no courses with this ID.";
                baseResponse.StatusCode = StatusCode.CoursesNotFound;

                return baseResponse;
            }

            baseResponse.Data = course;
            baseResponse.StatusCode = StatusCode.Ok;

            return baseResponse;
        }
        catch (Exception e)
        {
            baseResponse.Description = $"[CourseService.GetById] : {e.Message}";
            baseResponse.StatusCode = StatusCode.InternalServerError;

            return baseResponse;
        }
    }
}