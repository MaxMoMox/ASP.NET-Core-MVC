using Academy.Domain.Entities;
using Academy.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Academy.Controllers;

public class CourseController : Controller
{
    private readonly IHierarchyService<Course> _courseService;
    private readonly IHierarchyService<Group> _groupService;


    public CourseController(IHierarchyService<Course> courseService, IHierarchyService<Group> groupService)
    {
        _courseService = courseService;
        _groupService = groupService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var courseResponse = await _courseService.GetAll();

        if (courseResponse.StatusCode == Domain.Enums.StatusCode.Ok && courseResponse.Data != null)
        {
            return View(courseResponse.Data.ToList());
        }

        return RedirectToAction("Error" ,"Error");
    }

    [HttpGet]
    public async Task<IActionResult> Select(int courseId)
    {
        var groupResponse = await _groupService.GetAll();
        var courseResponse = await _courseService.GetById(courseId);

        if (courseResponse.Data != null && groupResponse.Data != null && groupResponse.StatusCode == Domain.Enums.StatusCode.Ok && courseResponse.StatusCode == Domain.Enums.StatusCode.Ok && courseId > 0)
        {

            ViewData["Groups"] = groupResponse.Data.Where(g => g.CourseId == courseId).ToList();
            ViewData["Course"] =  courseResponse.Data;

            return View();
        }

        return RedirectToAction("Error", "Error");
    }

    [HttpGet]
    public async Task<IActionResult> Create(int courseId)
    {
        if (courseId == 0)
        {
            return await Task.FromResult(View(new Course()
            {
                Id = courseId
            }));
        }

        var courseResponse = await _courseService.GetById(courseId);

        if (courseResponse.StatusCode == Domain.Enums.StatusCode.Ok)
        {
            return View(courseResponse.Data);
        }

        return RedirectToAction("Error", "Error");
    }

    [HttpPost]
    public async Task<IActionResult> Create(Course model)
    {
        if (ModelState.IsValid)
        {
            if (model.Id == 0)
            {
                await _courseService.Create(model);

                TempData["CourseSuccessAlertMessage"] = $"{model.Name} course created successfully.";

                return RedirectToAction("GetAll");
            }

            await _courseService.Update(model.Id, model);

            TempData["CourseSuccessAlertMessage"] = $"{model.Name} course updated successfully.";

            return RedirectToAction("GetAll");
        }

        return await Task.FromResult(View(model));
    }

    public async Task<IActionResult> Delete(int courseId)
    {
        var courseResponse = await _courseService.Delete(courseId);

        if (courseResponse.StatusCode == Domain.Enums.StatusCode.NotEmptyCourseRemoval && courseResponse.Data != null)
        {
            TempData["CourseErrorAlertMessage"] = $"{courseResponse.Data.Name} course contains some groups. Delete all groups inside {courseResponse.Data.Name} course first.";

            return RedirectToAction("GetAll");
        }

        if (courseResponse.StatusCode == Domain.Enums.StatusCode.Ok && courseResponse.Data != null)
        {
            TempData["CourseSuccessAlertMessage"] = $"{courseResponse.Data.Name} course deleted successfully.";

            return RedirectToAction("GetAll");
        }

        return RedirectToAction("Error","Error");
    }

    public async Task<SelectList?> GetCourseSelectList()
    {
        var courseResponse = await _courseService.GetAll();

        if(courseResponse.StatusCode == Domain.Enums.StatusCode.Ok && courseResponse.Data != null)
        {
            return new SelectList(courseResponse.Data, "Id", "Name");
        }

        return null;
    }
}