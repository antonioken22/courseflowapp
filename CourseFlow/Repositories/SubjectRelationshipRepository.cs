﻿using CourseFlow.Models;
using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace CourseFlow.Repositories
{
    public class SubjectRelationshipRepository : RepositoryBase, ISubjectRelationshipRepository
    {
        public void Add(SubjectRelationshipModel subjectRelationshipModel)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new OleDbCommand("INSERT INTO SubjectRelationships (SubjectID, RelatedSubjectID, RelationshipTypeID) VALUES (@subjectID, @relatedSubjectID, @relationshipTypeID)", connection))
                {
                    command.Parameters.AddWithValue("@subjectID", subjectRelationshipModel.SubjectID);
                    command.Parameters.AddWithValue("@relatedSubjectID", subjectRelationshipModel.RelatedSubjectID);
                    command.Parameters.AddWithValue("@relationshipTypeID", subjectRelationshipModel.RelationshipTypeID);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Edit(SubjectRelationshipModel subjectRelationshipModel)
        {
            throw new NotImplementedException();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<SubjectRelationshipModel> GetAll()
        {
            throw new NotImplementedException();
        }

        public SubjectRelationshipModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<SubjectRelationshipModel> GetBySubjectID(int subjectID)
        {
            throw new NotImplementedException();
        }

        public List<SubjectRelationshipModel> GetRelatedSubjects(SubjectModel subject)
        {
            throw new NotImplementedException();
        }

        public List<SubjectRelationshipModel> GetSubjectRelationshipsBySubject(SubjectModel subject)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new OleDbCommand("SELECT * FROM SubjectRelationships WHERE SubjectID = @subjectID", connection))
                {
                    command.Parameters.AddWithValue("@subjectID", subject.Id);
                    using (var reader = command.ExecuteReader())
                    {
                        var subjectRelationships = new List<SubjectRelationshipModel>();
                        while (reader.Read())
                        {
                            var subjectRelationship = new SubjectRelationshipModel
                            {
                                Id = Convert.ToInt32(reader["RelationshipID"]),
                                SubjectID = Convert.ToInt32(reader["SubjectID"]),
                                RelatedSubjectID = Convert.ToInt32(reader["RelatedSubjectID"]),
                                RelationshipTypeID = Convert.ToInt32(reader["RelationshipTypeID"])
                            };
                            subjectRelationships.Add(subjectRelationship);
                        }
                        return subjectRelationships;
                    }
                }   
            }
        }
    }
}
