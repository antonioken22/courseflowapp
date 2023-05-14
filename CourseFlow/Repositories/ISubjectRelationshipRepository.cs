﻿using CourseFlow.Models;
using System.Collections.Generic;

namespace CourseFlow.Repositories
{
    public interface ISubjectRelationshipRepository
    {
        void AddOrEdit(SubjectRelationshipModel subjectRelationshipModel);
        void Add(SubjectRelationshipModel subjectRelationshipModel);
        void Edit(SubjectRelationshipModel subjectRelationshipModel);
        void Remove(int id);
        SubjectRelationshipModel GetById(int id);
        IEnumerable<SubjectRelationshipModel> GetAll();
        public List<SubjectRelationshipModel> GetSubjectRelationshipsBySubject(int id);
    }
}
