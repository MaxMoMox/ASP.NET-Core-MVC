using Academy.DAL.Interfaces;
using Academy.Domain.Entities;
using Academy.Domain.Enums;
using Academy.Domain.Responses;
using Academy.Service.Interfaces;

namespace Academy.Service.Implementations;

public class GroupService : IHierarchyService<Group>
{
    private readonly IBaseRepository<Group> _groupRepository;
    private readonly IBaseRepository<Student> _studentRepository;

    public GroupService(IBaseRepository<Group> groupRepository, IBaseRepository<Student> studentRepository)
    {
        _groupRepository = groupRepository;
        _studentRepository = studentRepository;
    }

    public async Task<BaseResponse<Group>> Create(Group model)
    {
        var baseResponse = new BaseResponse<Group>();

        try
        {
            if (string.IsNullOrEmpty(model.Name) || model.CourseId == null)
            {
                baseResponse.Description = "[GroupService.Create] : Invalid input data.";
                baseResponse.StatusCode = StatusCode.InputDataError;

                return baseResponse;
            }

            var group = new Group
            {
                Name = model.Name,
                Course = model.Course,
                CourseId = model.CourseId
            };

            await _groupRepository.Create(group);

            baseResponse.Data = group;
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

    public async Task<BaseResponse<Group>> Update(int groupId, Group model)
    {
        var baseResponse = new BaseResponse<Group>();

        try
        {
            if (string.IsNullOrEmpty(model.Name) || model.CourseId == null || groupId <= 0)
            {
                baseResponse.Description = "[GroupService.Update] : Invalid input data.";
                baseResponse.StatusCode = StatusCode.InputDataError;

                return baseResponse;
            }

            var group = await _groupRepository.GetById(groupId);

            if (group == null)
            {
                baseResponse.Description = "[GroupService.Update] : Wrong group ID. There are no groups with this ID.";
                baseResponse.StatusCode = StatusCode.GroupsNotFound;

                return baseResponse;
            }

            group.Name = model.Name;
            group.CourseId = model.CourseId;

            await _groupRepository.Update(group);

            baseResponse.Data = group;
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

    public async Task<BaseResponse<Group>> Delete(int groupId)
    {
        var baseResponse = new BaseResponse<Group>();

        try
        {
            if (groupId <= 0)
            {
                baseResponse.Description = "[GroupService.Delete] : Invalid input data.";
                baseResponse.StatusCode = StatusCode.InputDataError;

                return baseResponse;
            }

            var group = await _groupRepository.GetById(groupId);

            if (group == null)
            {
                baseResponse.Description = "[GroupService.Delete] : Wrong group ID. There are no groups with this ID.";
                baseResponse.StatusCode = StatusCode.GroupsNotFound;

                return baseResponse;
            }

            IEnumerable<Student> groupStudents = await _studentRepository.GetAll();

            if (groupStudents.Any(s => s.GroupId == groupId))
            {
                baseResponse.StatusCode = StatusCode.NotEmptyGroupRemoval;
                baseResponse.Data = group;

                return baseResponse;
            }

            await _groupRepository.Delete(group);

            baseResponse.StatusCode = StatusCode.Ok;
            baseResponse.Data = group;

            return baseResponse;
        }
        catch (Exception e)
        {
            baseResponse.Description = $"[CourseService.Create] : {e.Message}";
            baseResponse.StatusCode = StatusCode.InternalServerError;

            return baseResponse;
        }
    }

    public async Task<BaseResponse<List<Group>>> GetAll()
    {
        var baseResponse = new BaseResponse<List<Group>>();

        try
        {
            var groups = await _groupRepository.GetAll();

            baseResponse.Data = groups;
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

    public async Task<BaseResponse<Group>> GetById(int groupId)
    {
        var baseResponse = new BaseResponse<Group>();

        try
        {
            var group = await _groupRepository.GetById(groupId);

            if (group == null)
            {
                baseResponse.Description = "[GroupService.GetById] : Wrong group ID. There are no groups with this ID.";
                baseResponse.StatusCode = StatusCode.GroupsNotFound;

                return baseResponse;
            }

            baseResponse.Data = group;
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
}