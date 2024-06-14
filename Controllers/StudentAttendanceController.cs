﻿using Microsoft.AspNetCore.Mvc;
using aitus.Interfaces;
using aitus.Models;
using AutoMapper;
using System.Linq;
using System.Collections.Generic;
using aitus.Dto;
using aituss.Interfaces;

namespace aitus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentAttendanceController : ControllerBase
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly ISubjectRepository _subjectRepository;
        private readonly ITeacherRepository _teacherRepository;
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly IMapper _mapper;

        public StudentAttendanceController(
            IStudentRepository studentRepository,
            IGroupRepository groupRepository,
            ISubjectRepository subjectRepository,
            ITeacherRepository teacherRepository,
            IAttendanceRepository attendanceRepository,
            IMapper mapper)
        {
            _studentRepository = studentRepository;
            _groupRepository = groupRepository;
            _subjectRepository = subjectRepository;
            _teacherRepository = teacherRepository;
            _attendanceRepository = attendanceRepository;
            _mapper = mapper;
        }

        [HttpGet("student/{studentId}/subject/{subjectId}")]
        public IActionResult GetStudentAttendance(int studentId, int subjectId)
        {
            if (!_studentRepository.StudentExists(studentId))
                return NotFound($"Student with ID {studentId} not found.");
            if (!_subjectRepository.SubjectExists(subjectId))
                return NotFound($"Subject with ID {subjectId} not found.");
            var student = _studentRepository.GetStudent(studentId);
            var group = _groupRepository.GetGroup(student.GroupId);
            var subject = _subjectRepository.GetSubject(subjectId);
            var subjectTeacher = _teacherRepository.GetTeacherNameBySubjectAndGroup(subjectId, group.GroupId);
            var studentBarcode = _studentRepository.GetStudentBarcode(student.Email);
            var attendances = _attendanceRepository.GetAttendancesByStudentIdAndSubject(studentId, subjectId);
            var attendancePercent = _attendanceRepository.GetAttendancePercent(studentId, subjectId);
            var studentAttendanceDto = new StudentAttendanceDto
            {
                StudentBarcode = studentBarcode,
                StudentName = student.Name,
                StudentSurname = student.Surname,
                GroupName = group.GroupName,
                TeacherName = subjectTeacher.Name,
                TeacherSurname = subjectTeacher.Surname,
                SubjectName = subject.SubjectName,
                AttendancePercent = attendancePercent,
                Attendances = _mapper.Map<List<AttendanceDto>>(attendances)
            };
            return Ok(studentAttendanceDto);
        }
    }
}
