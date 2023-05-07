﻿using CourseFlow.Models;
using CourseFlow.Repositories;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace CourseFlow.ViewModels
{
    public class CourseFlowsheetViewModel : ViewModelBase
    {
        // Fields
        private ICourseRepository _courseRepository;
        private IAcademicYearRepository _academicYearRepository;
        private IYearLevelRepository _yearLevelRepository;
        private ISemesterRepository _semesterRepository;
        private ISubjectRepository _subjectRepository;
        private ISubjectRelationshipRepository _subjectRelationshipRepository;
        private IRelationshipTypeRepository _relationshipTypeRepository;

        // Properties
        public CourseModel SelectedCourse { get; set; }
        public AcademicYearModel SelectedAcademicYear { get; set; }
        public YearLevelModel LoadedYearLevel { get; set; }
        public SemesterModel LoadedSemesterModel { get; set; }

        public ObservableCollection<CourseModel> Courses { get; set; }
        public ObservableCollection<AcademicYearModel> AcademicYears { get; set; }
        public ObservableCollection<YearLevelModel> YearLevels { get; set; }
        public ObservableCollection<SemesterModel> Semesters { get; set; }
        public ObservableCollection<SubjectModel> Subjects { get; set; }
        public ObservableCollection<SubjectRelationshipModel> SubjectRelationships { get; set; }
        public ObservableCollection<RelationshipTypeModel> RelationshipTypes { get; set; }

        public ObservableCollection<YearLevelData> FlowsheetData { get; set; }
        public class YearLevelData
        {
            public YearLevelData()
            {
                Semesters = new ObservableCollection<SemesterData>();
            }
            public YearLevelModel YearLevel { get; set; }

            public ObservableCollection<SemesterData> Semesters { get; set; }
        }
        public class SemesterData
        {
            public SemesterModel Semester { get; set; }
            public ObservableCollection<SubjectModel> Subjects { get; set; }
        }


        public ICommand LoadCoursesCommand { get; }
        public ICommand LoadAcademicYearsCommand { get; }
        public ICommand LoadYearLevelsCommand { get; }
        public ICommand LoadSemestersCommand { get; }
        public ICommand LoadSubjectsCommand { get; }
        public ICommand LoadSubjectRelationshipsCommand { get; }
        public ICommand LoadRelationshipTypesCommand { get; }

        public ICommand LoadFlowsheetCommand { get; }
        public ICommand OnPageLoadCommand { get; }


        // Constructors
        public CourseFlowsheetViewModel()
        {
            _courseRepository = new CourseRepository();
            _academicYearRepository = new AcademicYearRepository();
            _yearLevelRepository = new YearLevelRepository();
            _semesterRepository = new SemesterRepository();
            _subjectRepository = new SubjectRepository();
            _subjectRelationshipRepository = new SubjectRelationshipRepository();
            _relationshipTypeRepository = new RelationshipTypeRepository();

            Courses = new ObservableCollection<CourseModel>();
            AcademicYears = new ObservableCollection<AcademicYearModel>();
            YearLevels = new ObservableCollection<YearLevelModel>();
            Semesters = new ObservableCollection<SemesterModel>();
            Subjects = new ObservableCollection<SubjectModel>();
            SubjectRelationships = new ObservableCollection<SubjectRelationshipModel>();
            RelationshipTypes = new ObservableCollection<RelationshipTypeModel>();

            LoadCoursesCommand = new ViewModelCommand(param => LoadCourses());
            LoadAcademicYearsCommand = new ViewModelCommand(param => LoadAcademicYears());
            LoadYearLevelsCommand = new ViewModelCommand(param => LoadYearLevels());
            LoadFlowsheetCommand = new ViewModelCommand(param => LoadFlowsheet());

            FlowsheetData = new ObservableCollection<YearLevelData>();
            OnPageLoadCommand = new ViewModelCommand(param => OnPageLoad());
        }

        private void OnPageLoad()
        {
            LoadCourses();
            LoadAcademicYears();
        }

        private void LoadCourses()
        {
            Courses = new ObservableCollection<CourseModel>(_courseRepository.GetAll());
            OnPropertyChanged(nameof(Courses));
        }

        private void LoadAcademicYears()
        {
            AcademicYears = new ObservableCollection<AcademicYearModel>(_academicYearRepository.GetAll());
            OnPropertyChanged(nameof(AcademicYears));
        }

        private void LoadFlowsheet()
        {
            if (SelectedCourse == null || SelectedAcademicYear == null)
            {
                return;
            }

            FlowsheetData.Clear();
            LoadYearLevels();

            OnPropertyChanged(nameof(FlowsheetData));
        }

        private void LoadYearLevels()
        {
            var subjectsByCourseAndAcademicYear = _subjectRepository.GetSubjectsByCourseAndAcademicYear(SelectedCourse, SelectedAcademicYear);
            var yearLevelsInSubjects = subjectsByCourseAndAcademicYear.Select(s => s.YearLevelID).Distinct().OrderBy(id => id).ToList();

            foreach (var yearLevelID in yearLevelsInSubjects)
            {
                var yearLevel = _yearLevelRepository.GetById(yearLevelID);
                var yearLevelData = new YearLevelData { YearLevel = yearLevel };

                LoadSemesters(yearLevelData);

                FlowsheetData.Add(yearLevelData);
            }
        }

        private void LoadSemesters(YearLevelData yearLevelData)
        {
            var subjectsByYearLevelCourseAndAcademicYear = _subjectRepository.GetSubjectsByYearLevelSemesterAndCourse(yearLevelData.YearLevel, null, SelectedCourse, SelectedAcademicYear);
            var semesterIdsInSubjects = subjectsByYearLevelCourseAndAcademicYear.Select(s => s.SemesterID).Distinct().OrderBy(id => id).ToList();

            var semesters = _semesterRepository.GetAll().Where(s => semesterIdsInSubjects.Contains(s.Id));
            foreach (var semester in semesters)
            {
                var semesterData = new SemesterData { Semester = semester };
                semesterData.Subjects = new ObservableCollection<SubjectModel>();

                var subjects = _subjectRepository.GetSubjectsByYearLevelSemesterAndCourse(yearLevelData.YearLevel, semester, SelectedCourse, SelectedAcademicYear);
                foreach (var subject in subjects)
                {
                    semesterData.Subjects.Add(subject);
                }

                yearLevelData.Semesters.Add(semesterData);
            }
        }     
    }
}